using System;
using System.Collections.Generic;
using System.Linq;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class AlgebraicMinimizationSolver : IAlgebraicMinimizationSolver
{
    private static readonly string[] VariableLetters = ["a", "b", "c", "d"];

    public IAlgebraicMinimizationSolution Solve(IAlgebraicMinimizationParameters parameters)
    {
        var varCount = parameters.VariableCount;
        var truth = parameters.TruthValues;

        BooleanMinimizationEngine.ValidateTruthValues(varCount, truth, allowDontCare: false);

        var mintermIndices = Enumerable.Range(0, truth.Length).Where(i => truth[i] == 1).ToArray();
        var steps = new List<string>();

        if (mintermIndices.Length == 0)
        {
            steps.Add("Keine Minterme vorhanden (Funktion ist identisch 0).");
            return new AlgebraicMinimizationSolution
            {
                Kdnf = "0",
                MinimalDnf = "0",
                MintermIndices = [],
                InitialLiteralCount = 0,
                MinimalLiteralCount = 0,
                InitialGateCount = 0,
                MinimalGateCount = 0,
                Steps = steps
            };
        }

        if (mintermIndices.Length == truth.Length)
        {
            steps.Add("Alle Minterme vorhanden (Funktion ist identisch 1 / Tautologie).");
            return new AlgebraicMinimizationSolution
            {
                Kdnf = "1",
                MinimalDnf = "1",
                MintermIndices = mintermIndices,
                InitialLiteralCount = mintermIndices.Length * varCount,
                MinimalLiteralCount = 0,
                InitialGateCount = 0,
                MinimalGateCount = 0,
                Steps = steps
            };
        }

        // 1. KDNF
        var mintermStrings = mintermIndices.Select(m => FormatMinterm(varCount, m)).ToList();
        var kdnf = string.Join(" ∨ ", mintermStrings);
        steps.Add($"Kanonische DNF (KDNF) über Einsmenge {{{string.Join(", ", mintermIndices)}}}:");
        steps.Add($"y = {kdnf}");

        // 2. Erste Ordnung: Nachbarschaften (Hamming-Distanz 1)
        var firstOrderPairings = new List<AlgebraicPairing>();
        var neighborMap = new Dictionary<int, List<int>>();
        foreach (var m in mintermIndices)
        {
            neighborMap[m] = new List<int>();
        }

        for (int i = 0; i < mintermIndices.Length; i++)
        {
            for (int j = i + 1; j < mintermIndices.Length; j++)
            {
                int mA = mintermIndices[i];
                int mB = mintermIndices[j];
                int diff = mA ^ mB;

                if (BitCount(diff) == 1)
                {
                    int differingBit = (int)Math.Round(Math.Log(diff, 2));
                    string diffVar = GetVariableName(varCount, differingBit);
                    string commonFactor = FormatCommonFactor(varCount, mA, diff);
                    string combined = commonFactor;

                    firstOrderPairings.Add(new AlgebraicPairing
                    {
                        MintermA = mA,
                        MintermB = mB,
                        TermA = FormatMinterm(varCount, mA),
                        TermB = FormatMinterm(varCount, mB),
                        DifferingVariable = diffVar,
                        CommonFactor = commonFactor,
                        CombinedTerm = combined
                    });

                    neighborMap[mA].Add(mB);
                    neighborMap[mB].Add(mA);

                    steps.Add($"Paarung m{mA} und m{mB}: Ausklammern von {commonFactor} & Komplement {diffVar} ∨ ¬{diffVar} = 1 → {combined}");
                }
            }
        }

        // 3. Idempotenz-Nutzung
        var idempotenceUsages = new List<IdempotenceUsage>();
        foreach (var m in mintermIndices)
        {
            if (neighborMap[m].Count > 1)
            {
                var neighbors = neighborMap[m].Distinct().OrderBy(x => x).ToArray();
                var explanation = $"Minterm m_{m} wird über das Idempotenzgesetz (m_{m} = m_{m} ∨ m_{m}) {neighbors.Length}× verwendet und mit den Nachbarn {string.Join(", ", neighbors.Select(n => $"m_{n}"))} verschmolzen.";
                idempotenceUsages.Add(new IdempotenceUsage
                {
                    MintermIndex = m,
                    MintermName = $"m_{m}",
                    PairedWithMinterms = neighbors,
                    Explanation = explanation
                });
                steps.Add($"Idempotenz: {explanation}");
            }
        }

        // 4. Zweite Ordnung (z.B. Zusammenfassen von 2er-Gruppen zu 4er-Gruppen)
        var secondOrderPairings = new List<AlgebraicPairing>();
        for (int i = 0; i < firstOrderPairings.Count; i++)
        {
            for (int j = i + 1; j < firstOrderPairings.Count; j++)
            {
                var pA = firstOrderPairings[i];
                var pB = firstOrderPairings[j];

                // Check if they share the same differing variable but have different other bits
                int diffA = pA.MintermA ^ pA.MintermB;
                int diffB = pB.MintermA ^ pB.MintermB;
                if (diffA == diffB)
                {
                    int maskA = diffA;
                    int baseA = pA.MintermA & ~maskA;
                    int baseB = pB.MintermA & ~maskA;
                    int diff2 = baseA ^ baseB;

                    if (BitCount(diff2) == 1)
                    {
                        int differingBit2 = (int)Math.Round(Math.Log(diff2, 2));
                        string diffVar2 = GetVariableName(varCount, differingBit2);
                        string commonFactor2 = FormatCommonFactor(varCount, baseA, maskA | diff2);

                        // Avoid duplicates
                        if (!secondOrderPairings.Any(existing => existing.CombinedTerm == commonFactor2))
                        {
                            secondOrderPairings.Add(new AlgebraicPairing
                            {
                                MintermA = pA.MintermA,
                                MintermB = pB.MintermA,
                                TermA = pA.CombinedTerm,
                                TermB = pB.CombinedTerm,
                                DifferingVariable = diffVar2,
                                CommonFactor = commonFactor2,
                                CombinedTerm = commonFactor2
                            });
                            steps.Add($"2. Ordnung: Zusammenfassung ({pA.CombinedTerm}) ∨ ({pB.CombinedTerm}) über {diffVar2} ∨ ¬{diffVar2} = 1 → {commonFactor2}");
                        }
                    }
                }
            }
        }

        // 5. Minimalform via QMC Engine
        var qmcResult = BooleanMinimizationEngine.Solve(varCount, truth);
        var minimalDnf = TranslateExpressionToLetters(varCount, qmcResult.MinimalDnf);

        int initialLiterals = mintermIndices.Length * varCount;
        int minimalLiterals = CountLiterals(minimalDnf);

        int initialGates = mintermIndices.Length + (mintermIndices.Length > 1 ? 1 : 0);
        int minimalTerms = minimalDnf.Split(['|', '∨'], StringSplitOptions.RemoveEmptyEntries).Length;
        int minimalGates = minimalTerms + (minimalTerms > 1 ? 1 : 0);

        return new AlgebraicMinimizationSolution
        {
            Kdnf = kdnf,
            MinimalDnf = minimalDnf,
            MintermIndices = mintermIndices,
            FirstOrderPairings = firstOrderPairings,
            IdempotenceUsages = idempotenceUsages,
            SecondOrderPairings = secondOrderPairings,
            InitialLiteralCount = initialLiterals,
            MinimalLiteralCount = minimalLiterals,
            InitialGateCount = initialGates,
            MinimalGateCount = minimalGates,
            Steps = steps
        };
    }

    private static string GetVariableName(int varCount, int bitIndex)
    {
        // MSB (varCount - 1) is 'a', bit 0 is last letter
        int letterIndex = varCount - 1 - bitIndex;
        if (letterIndex >= 0 && letterIndex < VariableLetters.Length)
        {
            return VariableLetters[letterIndex];
        }
        return $"x{bitIndex}";
    }

    private static string FormatMinterm(int varCount, int minterm)
    {
        var literals = new List<string>();
        for (int bit = varCount - 1; bit >= 0; bit--)
        {
            bool isHigh = ((minterm >> bit) & 1) == 1;
            string v = GetVariableName(varCount, bit);
            literals.Add(isHigh ? v : $"¬{v}");
        }
        return "(" + string.Join(" ∧ ", literals) + ")";
    }

    private static string FormatCommonFactor(int varCount, int baseValue, int ignoredMask)
    {
        var literals = new List<string>();
        for (int bit = varCount - 1; bit >= 0; bit--)
        {
            if (((ignoredMask >> bit) & 1) != 0)
            {
                continue;
            }

            bool isHigh = ((baseValue >> bit) & 1) == 1;
            string v = GetVariableName(varCount, bit);
            literals.Add(isHigh ? v : $"¬{v}");
        }

        return literals.Count switch
        {
            0 => "1",
            1 => literals[0],
            _ => "(" + string.Join(" ∧ ", literals) + ")"
        };
    }

    private static string TranslateExpressionToLetters(int varCount, string qmcExpr)
    {
        if (string.IsNullOrWhiteSpace(qmcExpr) || qmcExpr == "0" || qmcExpr == "1")
        {
            return qmcExpr;
        }

        // QMC engine produces expressions like (!x2 & !x1) | (x1 & x0)
        string result = qmcExpr.Replace("&", "∧").Replace("|", "∨");
        for (int bit = varCount - 1; bit >= 0; bit--)
        {
            string letter = GetVariableName(varCount, bit);
            result = result.Replace($"!x{bit}", $"¬{letter}");
            result = result.Replace($"x{bit}", letter);
        }

        // Clean single-literal terms like (¬b) -> ¬b
        var terms = result.Split([" ∨ ", "∨"], StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.Trim())
            .Select(t => (t.StartsWith("(") && t.EndsWith(")") && !t.Substring(1, t.Length - 2).Contains("∧")) ? t.Substring(1, t.Length - 2) : t);

        return string.Join(" ∨ ", terms);
    }


    private static int CountLiterals(string expr)
    {
        if (string.IsNullOrWhiteSpace(expr) || expr == "0" || expr == "1") return 0;
        int count = 0;
        foreach (char c in expr)
        {
            if (c >= 'a' && c <= 'z' && c != 'v') // exclude 'v' in case or other symbols
            {
                count++;
            }
        }
        return count;
    }

    private static int BitCount(int value)
    {
        int count = 0;
        while (value != 0)
        {
            count += value & 1;
            value >>= 1;
        }
        return count;
    }
}
