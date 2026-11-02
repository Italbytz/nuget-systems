using System;
using System.Collections.Generic;
using System.Linq;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

internal static class EspressoEngine
{
    internal sealed record Cube(int FixedBits, int Mask)
    {
        public bool Covers(int minterm) => (minterm & ~Mask) == (FixedBits & ~Mask);

        public IEnumerable<int> GetCoveredIndices(int variableCount)
        {
            var max = 1 << variableCount;
            for (var index = 0; index < max; index++)
            {
                if (Covers(index))
                {
                    yield return index;
                }
            }
        }

        public int LiteralCount(int variableCount) => variableCount - BitCount(Mask);

        public bool IsSubsetOf(Cube other) =>
            (Mask & ~other.Mask) == 0 && (FixedBits & ~other.Mask) == (other.FixedBits & ~other.Mask);

        public string Format(int variableCount) =>
            BooleanMinimizationEngine.FormatImplicant(variableCount, FixedBits, Mask);
    }

    internal static IEspressoSolution Solve(int variableCount, int[] truthValues)
    {
        BooleanMinimizationEngine.ValidateTruthValues(variableCount, truthValues, allowDontCare: true);

        var minterms = Enumerable.Range(0, truthValues.Length).Where(index => truthValues[index] == 1).ToArray();
        var dontCares = Enumerable.Range(0, truthValues.Length).Where(index => truthValues[index] == 2).ToArray();
        var offSet = Enumerable.Range(0, truthValues.Length).Where(index => truthValues[index] == 0).ToHashSet();

        var detailedSteps = new List<IEspressoStep>();
        var summarySteps = new List<string>
        {
            $"Espresso-Start mit {variableCount} Variablen.",
            $"On-Set F (1): {(minterms.Length > 0 ? string.Join(", ", minterms) : "keine")}.",
            $"Don't-Care-Set D (X): {(dontCares.Length > 0 ? string.Join(", ", dontCares) : "keine")}.",
            $"Off-Set R (0): {(offSet.Count > 0 ? string.Join(", ", offSet.OrderBy(value => value)) : "keine")}."
        };

        // Edge Case 1: On-Set is empty -> Function is 0
        if (minterms.Length == 0)
        {
            var zeroStep = new EspressoStep
            {
                Phase = "CONVERGED",
                Iteration = 1,
                Title = "Triviale Nullfunktion",
                Description = "Da keine 1-Minterme vorhanden sind, ist die minimale Funktion identisch 0.",
                CurrentCover = ["0"],
                ActionDetail = "Keine Implikanten erforderlich."
            };
            detailedSteps.Add(zeroStep);
            summarySteps.Add("Minimale DNF: 0");

            return new EspressoSolution
            {
                MinimalDnf = "0",
                FinalCover = ["0"],
                IterationCount = 1,
                InitialCubeCount = 0,
                FinalCubeCount = 0,
                InitialLiteralCount = 0,
                FinalLiteralCount = 0,
                DetailedSteps = detailedSteps,
                Steps = summarySteps
            };
        }

        // Edge Case 2: Off-Set is empty -> Function is 1
        if (offSet.Count == 0)
        {
            var oneCube = new Cube(0, (1 << variableCount) - 1);
            var oneStep = new EspressoStep
            {
                Phase = "CONVERGED",
                Iteration = 1,
                Title = "Triviale Einsfunktion",
                Description = "Da alle Belegungen 1 oder Don't Care sind, überdeckt ein einziger n-dimensionaler Hyperkubus den gesamten Raum.",
                CurrentCover = ["1"],
                ActionDetail = "Vollständige Reduktion zu 1."
            };
            detailedSteps.Add(oneStep);
            summarySteps.Add("Minimale DNF: 1");

            return new EspressoSolution
            {
                MinimalDnf = "1",
                FinalCover = ["1"],
                IterationCount = 1,
                InitialCubeCount = minterms.Length,
                FinalCubeCount = 1,
                InitialLiteralCount = minterms.Length * variableCount,
                FinalLiteralCount = 0,
                DetailedSteps = detailedSteps,
                Steps = summarySteps
            };
        }

        // STEP 0: Initial Cover from On-Set
        var currentCover = minterms.Select(minterm => new Cube(minterm, 0)).ToList();
        var initialCubeCount = currentCover.Count;
        var initialLiteralCount = currentCover.Sum(cube => cube.LiteralCount(variableCount));

        detailedSteps.Add(new EspressoStep
        {
            Phase = "INIT",
            Iteration = 0,
            Title = "Initialüberdeckung (On-Set)",
            Description = $"Start mit {currentCover.Count} elementaren Minterm-Würfeln (jeweils {variableCount} Literale).",
            CurrentCover = currentCover.Select(cube => cube.Format(variableCount)).ToArray(),
            ActionDetail = $"Initialisiert aus Mintermen: {string.Join(", ", minterms)}"
        });
        summarySteps.Add($"Initialüberdeckung mit {currentCover.Count} Würfeln erstellt.");

        var iteration = 1;
        var maxIterations = 5;
        var converged = false;

        while (iteration <= maxIterations && !converged)
        {
            var beforeCost = CalculateCost(currentCover, variableCount);

            // PHASE 1: EXPAND
            var expandActions = new List<string>();
            var expandedCover = new List<Cube>();

            foreach (var cube in currentCover)
            {
                var expanded = ExpandCube(cube, variableCount, minterms, offSet);
                expandedCover.Add(expanded);
                if (expanded != cube)
                {
                    expandActions.Add($"{cube.Format(variableCount)} -> {expanded.Format(variableCount)}");
                }
            }

            // Absorption: remove sub-cubes
            var absorbedCover = AbsorbCover(expandedCover);
            var absorbedCount = expandedCover.Count - absorbedCover.Count;

            detailedSteps.Add(new EspressoStep
            {
                Phase = "EXPAND",
                Iteration = iteration,
                Title = $"Phase EXPAND (Runde {iteration})",
                Description = "Jeder Würfel wird maximal aufgeblasen (Literale werden gestrichen), ohne mit dem Off-Set (0-Stellen) zu kollidieren. Anschließend werden vollständig überdeckte Teilwürfel absorbiert.",
                CurrentCover = absorbedCover.Select(cube => cube.Format(variableCount)).ToArray(),
                ActionDetail = expandActions.Count > 0
                    ? $"Vergrößert: {string.Join("; ", expandActions.Take(4))}{(expandActions.Count > 4 ? $" (+{expandActions.Count - 4} weitere)" : "")}. {absorbedCount} redundante Teilwürfel absorbiert."
                    : "Alle Würfel waren bereits maximal expandiert."
            });
            summarySteps.Add($"Runde {iteration} EXPAND: {absorbedCover.Count} Würfel nach Absorption.");

            // PHASE 2: IRREDUNDANT COVER
            var (irredundantCover, removedCubes) = MakeIrredundant(absorbedCover, minterms, variableCount);

            detailedSteps.Add(new EspressoStep
            {
                Phase = "IRREDUNDANT",
                Iteration = iteration,
                Title = $"Phase IRREDUNDANT (Runde {iteration})",
                Description = "Bestimmt die minimale Teilmenge an Primwürfeln, die alle geforderten 1-Minterme überdeckt. Überflüssige Würfel werden verworfen.",
                CurrentCover = irredundantCover.Select(cube => cube.Format(variableCount)).ToArray(),
                ActionDetail = removedCubes.Count > 0
                    ? $"Entfernte redundante Würfel: {string.Join(", ", removedCubes.Select(cube => cube.Format(variableCount)))}"
                    : "Keine redundanten Würfel gefunden; alle aktuellen Terme sind essenziell."
            });
            summarySteps.Add($"Runde {iteration} IRREDUNDANT: {irredundantCover.Count} Würfel verbleiben.");

            currentCover = irredundantCover;
            var afterCost = CalculateCost(currentCover, variableCount);

            // Check if we improved or converged
            if (afterCost >= beforeCost && iteration > 1)
            {
                converged = true;
                break;
            }

            // PHASE 3: REDUCE (to escape local minima for the next iteration)
            if (iteration < maxIterations)
            {
                var (reducedCover, reducedActions) = ReduceCover(currentCover, minterms, variableCount);

                detailedSteps.Add(new EspressoStep
                {
                    Phase = "REDUCE",
                    Iteration = iteration,
                    Title = $"Phase REDUCE (Runde {iteration})",
                    Description = "Verkleinert nicht-essenzielle Teile von Würfeln auf ihren zwingend benötigten Kern. Dies befreit überdeckte Bereiche und ermöglicht im nächsten EXPAND-Lauf alternative Verschmelzungen.",
                    CurrentCover = reducedCover.Select(cube => cube.Format(variableCount)).ToArray(),
                    ActionDetail = reducedActions.Count > 0
                        ? $"Reduziert: {string.Join("; ", reducedActions)}"
                        : "Kein Würfel konnte verkleinert werden, da alle Terme essenziell gebunden sind."
                });
                summarySteps.Add($"Runde {iteration} REDUCE: {reducedActions.Count} Würfel verkleinert.");

                // If reduce changed nothing, we're at a fixed point
                if (reducedActions.Count == 0)
                {
                    converged = true;
                }
                else
                {
                    currentCover = reducedCover;
                }
            }

            iteration++;
        }

        // Final pass: ensure all cubes are prime & irredundant
        var finalExpanded = AbsorbCover(currentCover.Select(cube => ExpandCube(cube, variableCount, minterms, offSet)).ToList());
        var (finalCover, _) = MakeIrredundant(finalExpanded, minterms, variableCount);

        var finalDnf = finalCover.Count > 0
            ? string.Join(" | ", finalCover.Select(cube => cube.Format(variableCount)).Distinct())
            : "0";

        detailedSteps.Add(new EspressoStep
        {
            Phase = "CONVERGED",
            Iteration = iteration,
            Title = "Konvergenz & Minimalform",
            Description = "Die Heuristik ist konvergiert. Es existiert keine weitere Kostenverbesserung.",
            CurrentCover = finalCover.Select(cube => cube.Format(variableCount)).ToArray(),
            ActionDetail = $"Minimale DNF: y = {finalDnf}"
        });
        summarySteps.Add($"Espresso konvergiert in {iteration} Iterationen: y = {finalDnf}");

        return new EspressoSolution
        {
            MinimalDnf = finalDnf,
            FinalCover = finalCover.Select(cube => cube.Format(variableCount)).ToArray(),
            IterationCount = iteration,
            InitialCubeCount = initialCubeCount,
            FinalCubeCount = finalCover.Count,
            InitialLiteralCount = initialLiteralCount,
            FinalLiteralCount = finalCover.Sum(cube => cube.LiteralCount(variableCount)),
            DetailedSteps = detailedSteps,
            Steps = summarySteps
        };
    }

    private static Cube ExpandCube(Cube cube, int variableCount, int[] onSet, HashSet<int> offSet)
    {
        var currentMask = cube.Mask;
        var fixedBits = cube.FixedBits & ~currentMask;

        // Try expanding each unmasked bit
        var candidates = new List<int>();
        for (var bit = 0; bit < variableCount; bit++)
        {
            var flag = 1 << bit;
            if ((currentMask & flag) == 0)
            {
                candidates.Add(bit);
            }
        }

        var changed = true;
        while (changed)
        {
            changed = false;
            int? bestBit = null;
            var bestCoveredOnSetCount = -1;

            foreach (var bit in candidates)
            {
                var candidateMask = currentMask | (1 << bit);
                var candidateFixed = fixedBits & ~candidateMask;
                var testCube = new Cube(candidateFixed, candidateMask);

                // Check if testCube touches offSet
                var touchesOffSet = testCube.GetCoveredIndices(variableCount).Any(offSet.Contains);
                if (!touchesOffSet)
                {
                    // Count how many minterms in On-Set are covered
                    var coveredOnSet = testCube.GetCoveredIndices(variableCount).Count(onSet.Contains);
                    if (coveredOnSet > bestCoveredOnSetCount)
                    {
                        bestCoveredOnSetCount = coveredOnSet;
                        bestBit = bit;
                    }
                }
            }

            if (bestBit.HasValue)
            {
                currentMask |= (1 << bestBit.Value);
                fixedBits &= ~currentMask;
                candidates.Remove(bestBit.Value);
                changed = true;
            }
        }

        return new Cube(fixedBits, currentMask);
    }

    private static List<Cube> AbsorbCover(List<Cube> cubes)
    {
        var distinct = cubes
            .DistinctBy(cube => (cube.FixedBits & ~cube.Mask, cube.Mask))
            .OrderByDescending(cube => cube.Mask)
            .ToList();

        var result = new List<Cube>();
        foreach (var candidate in distinct)
        {
            // If candidate is a proper subset of an already accepted cube, absorb it
            var isAbsorbed = result.Any(accepted => candidate.IsSubsetOf(accepted));
            if (!isAbsorbed)
            {
                result.Add(candidate);
            }
        }

        return result;
    }

    private static (List<Cube> Cover, List<Cube> Removed) MakeIrredundant(List<Cube> cubes, int[] onSet, int variableCount)
    {
        if (cubes.Count <= 1)
        {
            return (cubes, []);
        }

        // Identify which cubes cover which On-Set minterms
        var mintermToCubes = new Dictionary<int, List<Cube>>();
        foreach (var minterm in onSet)
        {
            mintermToCubes[minterm] = cubes.Where(cube => cube.Covers(minterm)).ToList();
        }

        var essential = new HashSet<Cube>();
        foreach (var entry in mintermToCubes)
        {
            if (entry.Value.Count == 1)
            {
                essential.Add(entry.Value[0]);
            }
        }

        var remainingMinterms = onSet
            .Where(minterm => !essential.Any(cube => cube.Covers(minterm)))
            .ToList();

        var selected = new List<Cube>(essential);
        var nonEssential = cubes.Except(essential).ToList();

        if (remainingMinterms.Count > 0 && nonEssential.Count > 0)
        {
            // Greedy or small branch & bound to cover remaining minterms
            var unreached = new HashSet<int>(remainingMinterms);
            while (unreached.Count > 0 && nonEssential.Count > 0)
            {
                var bestCandidate = nonEssential
                    .OrderByDescending(cube => unreached.Count(cube.Covers))
                    .ThenBy(cube => cube.LiteralCount(variableCount))
                    .First();

                selected.Add(bestCandidate);
                nonEssential.Remove(bestCandidate);
                unreached.RemoveWhere(bestCandidate.Covers);
            }
        }

        var removed = cubes.Except(selected).ToList();
        return (selected, removed);
    }

    private static (List<Cube> Cover, List<string> Actions) ReduceCover(List<Cube> cubes, int[] onSet, int variableCount)
    {
        var result = new List<Cube>();
        var actions = new List<string>();

        for (var index = 0; index < cubes.Count; index++)
        {
            var cube = cubes[index];
            var otherCubes = cubes.Where((_, otherIndex) => otherIndex != index).ToList();

            // Minterms in onSet that strictly REQUIRE this cube (not covered by any other cube)
            var requiredMinterms = onSet
                .Where(minterm => cube.Covers(minterm) && !otherCubes.Any(other => other.Covers(minterm)))
                .ToArray();

            if (requiredMinterms.Length > 0 && requiredMinterms.Length < cube.GetCoveredIndices(variableCount).Count(onSet.Contains))
            {
                // Can we shrink the cube to the bounding box of requiredMinterms?
                var boundingCube = BoundingCube(requiredMinterms, variableCount);
                if (boundingCube.LiteralCount(variableCount) > cube.LiteralCount(variableCount))
                {
                    result.Add(boundingCube);
                    actions.Add($"{cube.Format(variableCount)} verkleinert zu {boundingCube.Format(variableCount)}");
                    continue;
                }
            }

            result.Add(cube);
        }

        return (result, actions);
    }

    private static Cube BoundingCube(int[] minterms, int variableCount)
    {
        if (minterms.Length == 0)
        {
            return new Cube(0, (1 << variableCount) - 1);
        }

        var commonOnes = (1 << variableCount) - 1;
        var commonZeros = (1 << variableCount) - 1;

        foreach (var minterm in minterms)
        {
            commonOnes &= minterm;
            commonZeros &= ~minterm;
        }

        var differing = ~(commonOnes | commonZeros) & ((1 << variableCount) - 1);
        var fixedBits = commonOnes;

        return new Cube(fixedBits, differing);
    }

    private static int CalculateCost(List<Cube> cubes, int variableCount) =>
        (cubes.Count * 1000) + cubes.Sum(cube => cube.LiteralCount(variableCount));

    private static int BitCount(int value)
    {
        var count = 0;
        while (value != 0)
        {
            count += value & 1;
            value >>= 1;
        }

        return count;
    }
}

