using System;
using System.Collections.Generic;
using System.Linq;

namespace Italbytz.ComputingSystems;

internal static class BooleanMinimizationEngine
{
    internal static void ValidateTruthValues(int variableCount, int[] truthValues, bool allowDontCare)
    {
        if (variableCount < 1 || variableCount > 8)
        {
            throw new ArgumentOutOfRangeException(nameof(variableCount), "VariableCount must stay between 1 and 8.");
        }

        var expectedRowCount = 1 << variableCount;
        if (truthValues.Length != expectedRowCount)
        {
            throw new ArgumentException($"TruthValues must contain exactly {expectedRowCount} entries for {variableCount} variables.", nameof(truthValues));
        }

        var validValues = allowDontCare ? new[] { 0, 1, 2 } : new[] { 0, 1 };
        if (truthValues.Any(value => !validValues.Contains(value)))
        {
            throw new ArgumentException(allowDontCare
                ? "TruthValues may only contain 0, 1, or 2 (don't care)."
                : "TruthValues may only contain 0 or 1.", nameof(truthValues));
        }
    }

    internal static bool[] ToBits(int row, int variableCount)
    {
        var bits = new bool[variableCount];
        for (var bit = 0; bit < variableCount; bit++)
        {
            var shift = variableCount - bit - 1;
            bits[bit] = ((row >> shift) & 0x1) == 1;
        }

        return bits;
    }

    internal static string FormatAssignment(IReadOnlyList<bool> bits) =>
        string.Join(", ", bits.Select((value, index) => $"x{bits.Count - index - 1}={(value ? 1 : 0)}"));

    internal static string BuildMinterm(IReadOnlyList<bool> bits) =>
        "(" + string.Join(" & ", bits.Select((value, index) => value ? $"x{bits.Count - index - 1}" : $"!x{bits.Count - index - 1}")) + ")";

    internal static string BuildMaxterm(IReadOnlyList<bool> bits) =>
        "(" + string.Join(" | ", bits.Select((value, index) => value ? $"!x{bits.Count - index - 1}" : $"x{bits.Count - index - 1}")) + ")";

    internal static int GrayCode(int value) => value ^ (value >> 1);

    internal static string ToBinaryCode(int value, int width) => Convert.ToString(value, 2).PadLeft(width, '0');

    internal static string FormatImplicant(int variableCount, int fixedBits, int mask)
    {
        var literals = new List<string>();
        for (var position = 0; position < variableCount; position++)
        {
            var bitIndex = variableCount - position - 1;
            var bitFlag = 1 << bitIndex;
            if ((mask & bitFlag) != 0)
            {
                continue;
            }

            var isOne = (fixedBits & bitFlag) != 0;
            literals.Add(isOne ? $"x{bitIndex}" : $"!x{bitIndex}");
        }

        return literals.Count == 0 ? "1" : "(" + string.Join(" & ", literals) + ")";
    }

    internal static MinimizationResult Solve(int variableCount, int[] truthValues)
    {
        ValidateTruthValues(variableCount, truthValues, allowDontCare: true);

        var minterms = Enumerable.Range(0, truthValues.Length).Where(index => truthValues[index] == 1).ToArray();
        var dontCares = Enumerable.Range(0, truthValues.Length).Where(index => truthValues[index] == 2).ToArray();
        var steps = new List<string>
        {
            $"Start QMC with {variableCount} variables.",
            $"Minterms: {(minterms.Length > 0 ? string.Join(", ", minterms) : "none")}.",
            $"Don't cares: {(dontCares.Length > 0 ? string.Join(", ", dontCares) : "none")}.",
        };

        if (minterms.Length == 0)
        {
            steps.Add("No minterms with value 1 exist, so the minimal function is 0.");
            return new MinimizationResult([], [], [], steps, "0");
        }

        var current = minterms.Concat(dontCares)
            .Distinct()
            .Select(value => new ImplicantData(value, 0, [value], minterms.Contains(value) ? [value] : []))
            .OrderBy(item => item.FixedBits)
            .ToList();

        var primes = new List<ImplicantData>();
        var round = 1;
        while (current.Count > 0)
        {
            steps.Add($"Round {round}: {current.Count} implicants enter combination.");
            var next = new List<ImplicantData>();
            var combinedIndices = new HashSet<int>();

            for (var indexA = 0; indexA < current.Count; indexA++)
            {
                for (var indexB = indexA + 1; indexB < current.Count; indexB++)
                {
                    var first = current[indexA];
                    var second = current[indexB];
                    if (first.Mask != second.Mask)
                    {
                        continue;
                    }

                    var difference = (first.FixedBits ^ second.FixedBits) & ~first.Mask;
                    if (BitCount(difference) != 1)
                    {
                        continue;
                    }

                    combinedIndices.Add(indexA);
                    combinedIndices.Add(indexB);

                    var merged = new ImplicantData(
                        first.FixedBits & second.FixedBits,
                        first.Mask | difference,
                        first.CoveredIndices.Union(second.CoveredIndices).OrderBy(value => value).ToArray(),
                        first.CoveredMinterms.Union(second.CoveredMinterms).OrderBy(value => value).ToArray());

                    if (!next.Any(existing => existing.FixedBits == merged.FixedBits && existing.Mask == merged.Mask && existing.CoveredIndices.SequenceEqual(merged.CoveredIndices)))
                    {
                        next.Add(merged);
                        steps.Add($"Combine {FormatImplicant(variableCount, first.FixedBits, first.Mask)} with {FormatImplicant(variableCount, second.FixedBits, second.Mask)} -> {FormatImplicant(variableCount, merged.FixedBits, merged.Mask)}.");
                    }
                }
            }

            for (var index = 0; index < current.Count; index++)
            {
                if (!combinedIndices.Contains(index) && current[index].CoveredMinterms.Length > 0)
                {
                    primes.Add(current[index]);
                    steps.Add($"Prime implicant found: {FormatImplicant(variableCount, current[index].FixedBits, current[index].Mask)} covering {string.Join(", ", current[index].CoveredMinterms)}.");
                }
            }

            current = next
                .OrderBy(item => item.Mask)
                .ThenBy(item => item.FixedBits)
                .ToList();
            round++;
        }

        var primeList = primes
            .DistinctBy(implicant => (implicant.FixedBits, implicant.Mask))
            .ToList();

        var essential = new List<ImplicantData>();
        var remaining = new HashSet<int>(minterms);

        foreach (var minterm in minterms)
        {
            var covering = primeList.Where(implicant => implicant.CoveredMinterms.Contains(minterm)).ToList();
            if (covering.Count == 1)
            {
                var essentialPrime = covering[0];
                if (!essential.Any(existing => existing.FixedBits == essentialPrime.FixedBits && existing.Mask == essentialPrime.Mask))
                {
                    essential.Add(essentialPrime);
                    steps.Add($"Essential prime implicant: {FormatImplicant(variableCount, essentialPrime.FixedBits, essentialPrime.Mask)} uniquely covers minterm {minterm}.");
                }
            }
        }

        foreach (var essentialPrime in essential)
        {
            foreach (var minterm in essentialPrime.CoveredMinterms)
            {
                remaining.Remove(minterm);
            }
        }

        var selected = new List<ImplicantData>(essential);
        if (remaining.Count > 0)
        {
            steps.Add($"Petrick selection required for remaining minterms: {string.Join(", ", remaining.OrderBy(value => value))}.");
            var additional = SolveWithPetrick(variableCount, remaining, primeList.Except(essential).ToList(), steps);
            selected.AddRange(additional);
        }

        var minimalDnf = selected.Count > 0
            ? string.Join(" | ", selected.Select(implicant => FormatImplicant(variableCount, implicant.FixedBits, implicant.Mask)).Distinct())
            : "0";

        steps.Add($"Minimal DNF: {minimalDnf}.");

        return new MinimizationResult(primeList, essential, selected, steps, minimalDnf);
    }

    private static IReadOnlyList<ImplicantData> SolveWithPetrick(int variableCount, HashSet<int> remainingMinterms, IReadOnlyList<ImplicantData> candidates, List<string> steps)
    {
        var clauses = remainingMinterms
            .OrderBy(value => value)
            .Select(minterm => candidates
                .Select((implicant, index) => (implicant, index))
                .Where(entry => entry.implicant.CoveredMinterms.Contains(minterm))
                .Select(entry => entry.index)
                .ToArray())
            .ToArray();

        var products = new List<HashSet<int>> { new() };
        foreach (var clause in clauses)
        {
            var expanded = new List<HashSet<int>>();
            foreach (var product in products)
            {
                foreach (var index in clause)
                {
                    var next = new HashSet<int>(product) { index };
                    expanded.Add(next);
                }
            }

            products = SimplifyProducts(expanded);
        }

        var best = products
            .OrderBy(product => product.Count)
            .ThenBy(product => product.Sum(index => LiteralCount(candidates[index])))
            .First();

        var selection = best
            .Select(index => candidates[index])
            .ToArray();

        steps.Add($"Petrick selected {string.Join(", ", selection.Select(implicant => FormatImplicant(variableCount, implicant.FixedBits, implicant.Mask)))}.");
        return selection;
    }

    private static List<HashSet<int>> SimplifyProducts(IEnumerable<HashSet<int>> products)
    {
        var distinct = products
            .Select(product => product.OrderBy(value => value).ToArray())
            .Distinct(new SequenceComparer<int>())
            .Select(product => new HashSet<int>(product))
            .ToList();

        return distinct
            .Where(candidate => !distinct.Any(other => !ReferenceEquals(candidate, other) && other.IsSubsetOf(candidate)))
            .ToList();
    }

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

    private static int LiteralCount(ImplicantData implicant) => implicant.CoveredIndices.Length == 0
        ? 0
        : implicant.CoveredIndices.Length == 1 && implicant.Mask == 0
            ? ToBinaryCode(implicant.CoveredIndices[0], 1).Length
            : implicant.CoveredIndices.Length;

    internal sealed record ImplicantData(int FixedBits, int Mask, int[] CoveredIndices, int[] CoveredMinterms);

    internal sealed record MinimizationResult(
        IReadOnlyList<ImplicantData> PrimeImplicants,
        IReadOnlyList<ImplicantData> EssentialPrimeImplicants,
        IReadOnlyList<ImplicantData> SelectedPrimeImplicants,
        IReadOnlyList<string> Steps,
        string MinimalDnf);

    private sealed class SequenceComparer<T> : IEqualityComparer<T[]>
    {
        public bool Equals(T[]? x, T[]? y) => x is not null && y is not null && x.SequenceEqual(y);

        public int GetHashCode(T[] obj)
        {
            var hash = new HashCode();
            foreach (var item in obj)
            {
                hash.Add(item);
            }

            return hash.ToHashCode();
        }
    }
}