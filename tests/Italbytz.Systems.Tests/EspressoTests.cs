using System.Linq;
using Italbytz.ComputingSystems;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Italbytz.Systems.Tests;

[TestClass]
public class EspressoTests
{
    [TestMethod]
    public void TestLecture4VariableFunction()
    {
        // 4 variables: m in {1, 3, 5, 7, 11, 13}
        var truthValues = new int[16];
        int[] minterms = [1, 3, 5, 7, 11, 13];
        foreach (var m in minterms) truthValues[m] = 1;

        var solver = new EspressoSolver();
        var solution = solver.Solve(new EspressoParameters(4, truthValues));

        Assert.IsNotNull(solution);
        Assert.AreEqual(3, solution.FinalCubeCount);
        Assert.AreEqual(8, solution.FinalLiteralCount);

        // Also check Quine-McCluskey comparison
        var qmcSolver = new QuineMcCluskeySolver();
        var qmcSolution = qmcSolver.Solve(new QuineMcCluskeyParameters(4, truthValues));
        Assert.AreEqual(qmcSolution.MinimalDnf, solution.MinimalDnf);
    }

    [TestMethod]
    public void TestLectureDontCareX()
    {
        // 3 variables: m in {0, 2, 3, 4, 5, 7}, d in {1, 6} -> Minimal DNF is 1
        var truthValues = new int[8];
        int[] minterms = [0, 2, 3, 4, 5, 7];
        int[] dontCares = [1, 6];
        foreach (var m in minterms) truthValues[m] = 1;
        foreach (var d in dontCares) truthValues[d] = 2;

        var solver = new EspressoSolver();
        var solution = solver.Solve(new EspressoParameters(3, truthValues));

        Assert.IsNotNull(solution);
        Assert.AreEqual("1", solution.MinimalDnf);
    }

    [TestMethod]
    public void TestLectureDontCareY()
    {
        // 3 variables: m in {0, 1, 4}, d in {5, 7} -> Minimal DNF is (!x1) [which is !b]
        var truthValues = new int[8];
        int[] minterms = [0, 1, 4];
        int[] dontCares = [5, 7];
        foreach (var m in minterms) truthValues[m] = 1;
        foreach (var d in dontCares) truthValues[d] = 2;

        var solver = new EspressoSolver();
        var solution = solver.Solve(new EspressoParameters(3, truthValues));

        Assert.IsNotNull(solution);
        Assert.AreEqual("(!x1)", solution.MinimalDnf);
    }

    [TestMethod]
    public void TestTrivialZero()
    {
        var truthValues = new int[8];
        var solver = new EspressoSolver();
        var solution = solver.Solve(new EspressoParameters(3, truthValues));

        Assert.IsNotNull(solution);
        Assert.AreEqual("0", solution.MinimalDnf);
    }

    [TestMethod]
    public void TestMajority3()
    {
        // Majority of 3 variables: m in {3, 5, 6, 7}
        var truthValues = new int[8];
        int[] minterms = [3, 5, 6, 7];
        foreach (var m in minterms) truthValues[m] = 1;

        var solver = new EspressoSolver();
        var solution = solver.Solve(new EspressoParameters(3, truthValues));

        Assert.IsNotNull(solution);
        Assert.AreEqual(3, solution.FinalCubeCount);
        // Compare with QMC
        var qmcSolver = new QuineMcCluskeySolver();
        var qmcSolution = qmcSolver.Solve(new QuineMcCluskeyParameters(3, truthValues));
        Assert.AreEqual(qmcSolution.MinimalDnf, solution.MinimalDnf);
    }

    [TestMethod]
    public void TestPlaFormatGenerationAndParsing()
    {
        var truthValues = new int[8];
        int[] minterms = [3, 5, 6, 7]; // Majority 3
        foreach (var m in minterms) truthValues[m] = 1;

        var pla = PlaFormat.GeneratePla(3, truthValues);
        Assert.IsTrue(pla.Contains(".i 3"));
        Assert.IsTrue(pla.Contains(".o 1"));
        Assert.IsTrue(pla.Contains(".p 4"));
        Assert.IsTrue(pla.Contains("011 1")); // m3
        Assert.IsTrue(pla.Contains("101 1")); // m5
        Assert.IsTrue(pla.Contains("110 1")); // m6
        Assert.IsTrue(pla.Contains("111 1")); // m7
        Assert.IsTrue(pla.Contains(".e"));

        var parsed = PlaFormat.TryParsePla(pla, out var vars, out var parsedValues, out var error);
        Assert.IsTrue(parsed, error);
        Assert.AreEqual(3, vars);
        CollectionAssert.AreEqual(truthValues, parsedValues);
    }

    [TestMethod]
    public void TestPlaFromMinimizedCubes()
    {
        var cubes = new[] { "(!x3 & x2 & !x0)", "(x1 & x0)" };
        var pattern1 = PlaFormat.FormatCubeToPlaPattern(4, cubes[0]);
        var pattern2 = PlaFormat.FormatCubeToPlaPattern(4, cubes[1]);

        Assert.AreEqual("01-0", pattern1);
        Assert.AreEqual("--11", pattern2);

        var pla = PlaFormat.GeneratePlaFromCubes(4, cubes);
        Assert.IsTrue(pla.Contains(".i 4"));
        Assert.IsTrue(pla.Contains(".p 2"));
        Assert.IsTrue(pla.Contains("01-0 1"));
        Assert.IsTrue(pla.Contains("--11 1"));
    }

    [TestMethod]
    public void TestQmcAllCombinationsRecordedForLecturePreset()
    {
        // 4 variables: m in {1, 3, 5, 7, 11, 13} -> Vector: 0,1,0,1,0,1,0,1,0,0,0,1,0,1,0,0
        var truthValues = new int[16];
        int[] minterms = [1, 3, 5, 7, 11, 13];
        foreach (var m in minterms) truthValues[m] = 1;

        var qmcSolver = new QuineMcCluskeySolver();
        var qmcSolution = qmcSolver.Solve(new QuineMcCluskeyParameters(4, truthValues));

        // Both alternative paths to 0--1 (!x3 & x0) must be recorded:
        // 1) (!x3 & !x2 & x0) [00-1] + (!x3 & x2 & x0) [01-1] -> (!x3 & x0) [0--1]
        // 2) (!x3 & !x1 & x0) [0-01] + (!x3 & x1 & x0) [0-11] -> (!x3 & x0) [0--1]
        var path1 = qmcSolution.Steps.Any(s => s.Contains("(!x3 & !x2 & x0)") && s.Contains("(!x3 & x2 & x0)"));
        var path2 = qmcSolution.Steps.Any(s => s.Contains("(!x3 & !x1 & x0)") && s.Contains("(!x3 & x1 & x0)"));

        Assert.IsTrue(path1, "Path 1 (00-1 + 01-1 -> 0--1) must be in QMC steps.");
        Assert.IsTrue(path2, "Path 2 (0-01 + 0-11 -> 0--1) must be in QMC steps.");
    }

    class MockEntry
    {
        public string Term { get; set; } = "";
        public bool IsCombined { get; set; }
        public bool IsPrime { get; set; }
        public int Order { get; set; }
    }

    private static int BitCount(int value)
    {
        int count = 0;
        while (value > 0)
        {
            count += value & 1;
            value >>= 1;
        }
        return count;
    }

    private static (int fixedBits, int mask) ParseImplicantTerm(string term, int variableCount)
    {
        term = term.Trim();
        if (term == "1") return (0, (1 << variableCount) - 1);
        if (term == "0") return (0, 0);

        var clean = term.Trim('(', ')', ' ');
        var literals = clean.Split('&', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var mask = (1 << variableCount) - 1;
        var fixedBits = 0;

        foreach (var literal in literals)
        {
            var isNegated = literal.StartsWith("!");
            var varName = isNegated ? literal[1..] : literal;
            if (varName.StartsWith("x") && int.TryParse(varName[1..], out int bitIndex) && bitIndex < variableCount)
            {
                var bitFlag = 1 << bitIndex;
                mask &= ~bitFlag;
                if (!isNegated)
                {
                    fixedBits |= bitFlag;
                }
            }
        }

        return (fixedBits, mask);
    }

    [TestMethod]
    public void TestQmcCheckAllPossiblePairs()
    {
        int[] truthValues = [1,0,1,1,0,0,1,1,0,0,1,1,0,0,1,1];
        var qmcSolver = new QuineMcCluskeySolver();
        var qmcSolution = qmcSolver.Solve(new QuineMcCluskeyParameters(4, truthValues));

        var order0 = new List<MockEntry>();
        for (int i = 0; i < truthValues.Length; i++)
        {
            if (truthValues[i] == 1 || truthValues[i] == 2)
            {
                order0.Add(new MockEntry { Term = FormatTerm(4, i, 0), Order = 0 });
            }
        }

        var orders = new Dictionary<int, List<MockEntry>> { [0] = order0 };
        var foundPrimes = new List<string>();

        int noOpCount = 0;
        int stepIdx = 0;

        for (int s = 0; s < qmcSolution.Steps.Count; s++)
        {
            var stepText = qmcSolution.Steps[s];
            if (stepText.StartsWith("Round ") || stepText.StartsWith("Petrick selection required")) continue;

            bool hadVisualChange = false;

            if (stepText.StartsWith("Combine "))
            {
                var match = System.Text.RegularExpressions.Regex.Match(stepText, @"Combine\s+(.+?)\s+with\s+(.+?)\s+->\s+(.+?)\.");
                if (match.Success)
                {
                    var termA = match.Groups[1].Value.Trim();
                    var termB = match.Groups[2].Value.Trim();
                    var termC = match.Groups[3].Value.Trim();

                    var (_, maskA) = ParseImplicantTerm(termA, 4);
                    int orderA = BitCount(maskA);

                    if (orders.TryGetValue(orderA, out var listA))
                    {
                        var eA = listA.FirstOrDefault(e => e.Term == termA);
                        if (eA != null && !eA.IsCombined) { eA.IsCombined = true; hadVisualChange = true; }
                        var eB = listA.FirstOrDefault(e => e.Term == termB);
                        if (eB != null && !eB.IsCombined) { eB.IsCombined = true; hadVisualChange = true; }
                    }

                    var (_, maskC) = ParseImplicantTerm(termC, 4);
                    int orderC = BitCount(maskC);
                    if (!orders.TryGetValue(orderC, out var listC))
                    {
                        listC = new List<MockEntry>();
                        orders[orderC] = listC;
                    }
                    var eC = listC.FirstOrDefault(e => e.Term == termC);
                    if (eC == null)
                    {
                        listC.Add(new MockEntry { Term = termC, Order = orderC });
                        hadVisualChange = true;
                    }
                }
            }
            else if (stepText.StartsWith("Prime implicant found: "))
            {
                var match = System.Text.RegularExpressions.Regex.Match(stepText, @"Prime implicant found:\s+(.+?)\s+covering\s+(.+?)\.");
                if (match.Success)
                {
                    var termP = match.Groups[1].Value.Trim();
                    var (_, maskP) = ParseImplicantTerm(termP, 4);
                    int orderP = BitCount(maskP);
                    if (orders.TryGetValue(orderP, out var listP))
                    {
                        var eP = listP.FirstOrDefault(e => e.Term == termP);
                        if (eP != null && !eP.IsPrime) { eP.IsPrime = true; hadVisualChange = true; }
                    }
                    if (!foundPrimes.Contains(termP))
                    {
                        foundPrimes.Add(termP);
                        hadVisualChange = true;
                    }
                }
            }
            else
            {
                hadVisualChange = true;
            }

            if (!hadVisualChange) noOpCount++;
            System.Console.WriteLine($"Snapshot #{stepIdx++}: VisualChange={hadVisualChange,-5} | {stepText}");
        }

        System.Console.WriteLine($"\nTOTAL NO-OP STEPS: {noOpCount}");
        Assert.AreEqual(0, noOpCount, "There should be zero steps without visual change in QMC!");
        Assert.AreEqual("(!x3 & !x2 & !x0) | (x1)", qmcSolution.MinimalDnf);
    }

    private static string FormatTerm(int variableCount, int fixedBits, int mask)
    {
        var literals = new List<string>();
        for (int i = variableCount - 1; i >= 0; i--)
        {
            if ((mask & (1 << i)) == 0)
            {
                literals.Add((fixedBits & (1 << i)) != 0 ? $"x{i}" : $"!x{i}");
            }
        }
        return literals.Count == 0 ? "1" : $"({string.Join(" & ", literals)})";
    }
}
