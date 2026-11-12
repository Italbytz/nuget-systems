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

        var snapshots = Italbytz.Systems.Blazor.Components.ComputingSystems.QuineMcCluskeyVisualizer.BuildSnapshots(4, truthValues, qmcSolution);
        Assert.IsTrue(snapshots.Count > 0, "Snapshots should not be empty");

        // Verify that consecutive snapshots always have visual differences
        for (int i = 1; i < snapshots.Count; i++)
        {
            var prev = snapshots[i - 1];
            var curr = snapshots[i];

            bool hasChange = curr.StepText != prev.StepText &&
                (curr.ActivePrimeImplicantTerm != prev.ActivePrimeImplicantTerm ||
                 curr.ActiveUniqueMinterm != prev.ActiveUniqueMinterm ||
                 curr.FoundPrimeImplicants.Count != prev.FoundPrimeImplicants.Count ||
                 curr.EssentialPrimeImplicants.Count != prev.EssentialPrimeImplicants.Count ||
                 curr.SelectedPrimeImplicants.Count != prev.SelectedPrimeImplicants.Count ||
                 curr.Orders.Count != prev.Orders.Count ||
                 curr.Orders.Sum(o => o.Value.Count) != prev.Orders.Sum(o => o.Value.Count) ||
                 curr.Orders.Any(kv => kv.Value.Any(e => {
                     var pe = prev.Orders.TryGetValue(kv.Key, out var pl) ? pl.FirstOrDefault(x => x.Term == e.Term) : null;
                     return pe == null || pe.IsCombined != e.IsCombined || pe.IsPrime != e.IsPrime || pe.IsActiveSource != e.IsActiveSource || pe.IsActiveTarget != e.IsActiveTarget;
                 })));

            Assert.IsTrue(hasChange, $"Snapshot #{i} ('{curr.StepText}') must produce a visible difference compared to snapshot #{i-1} ('{prev.StepText}')!");
        }

        // Verify final snapshot:
        var finalSnap = snapshots[^1];
        var order1 = finalSnap.Orders[1];
        var entry00_0 = order1.FirstOrDefault(e => e.BinaryPattern == "00-0" || e.Term == "(!x3 & !x2 & !x0)");
        Assert.IsNotNull(entry00_0, "00-0 must be present in order 1");
        Assert.IsTrue(entry00_0.IsPrime, "00-0 must be marked as prime implicant (PI)");

        // All rows across all orders in the final snapshot must be either combined (✓) or prime (PI)
        foreach (var kvp in finalSnap.Orders)
        {
            int orderNum = kvp.Key;
            var entries = kvp.Value;
            foreach (var e in entries)
            {
                Assert.IsTrue(e.IsCombined || e.IsPrime, $"Order {orderNum} entry '{e.Term}' ({e.BinaryPattern}) is neither combined nor marked as prime!");
            }
        }

        Assert.AreEqual("(!x3 & !x2 & !x0) | (x1)", qmcSolution.MinimalDnf);
    }

    [TestMethod]
    public void TestGenerateExamGradeFunction()
    {
        var solver = new QuineMcCluskeySolver();

        for (int i = 0; i < 20; i++)
        {
            var tv4 = Italbytz.Systems.Blazor.Components.ComputingSystems.QuineMcCluskeyVisualizer.GenerateExamGradeFunction(4);
            int ones4 = tv4.Count(v => v == 1);
            Assert.IsTrue(ones4 >= 5 && ones4 <= 8, $"4-var function must have 5..8 ones, but got {ones4}");

            var sol4 = solver.Solve(new QuineMcCluskeyParameters(4, tv4));
            Assert.AreNotEqual("0", sol4.MinimalDnf);
            Assert.AreNotEqual("1", sol4.MinimalDnf);
            Assert.IsTrue(sol4.PrimeImplicants.Length >= 3 && sol4.PrimeImplicants.Length <= 5, $"PI count must be 3..5, got {sol4.PrimeImplicants.Length}");

            var tv3 = Italbytz.Systems.Blazor.Components.ComputingSystems.QuineMcCluskeyVisualizer.GenerateExamGradeFunction(3);
            int ones3 = tv3.Count(v => v == 1);
            Assert.IsTrue(ones3 >= 3 && ones3 <= 5, $"3-var function must have 3..5 ones, but got {ones3}");

            var sol3 = solver.Solve(new QuineMcCluskeyParameters(3, tv3));
            Assert.AreNotEqual("0", sol3.MinimalDnf);
            Assert.AreNotEqual("1", sol3.MinimalDnf);
            Assert.IsTrue(sol3.PrimeImplicants.Length >= 2 && sol3.PrimeImplicants.Length <= 4, $"PI count must be 2..4, got {sol3.PrimeImplicants.Length}");
        }
    }

    [TestMethod]
    public void TestPlaCustomTermEvaluation()
    {
        // Test TiPla Variant A Teil 1 terms: 00-1, 0-10, 1-0-
        var plaText = ".i 4\n.o 1\n.p 3\n00-1 1\n0-10 1\n1-0- 1\n.e";
        var parsed = PlaFormat.TryParsePla(plaText, out var vars, out var truthValues, out var err);
        Assert.IsTrue(parsed, err);
        Assert.AreEqual(4, vars);

        // Check expected minterms from TIPLA Teil 1:
        // 00-1: 0001 (1), 0011 (3)
        // 0-10: 0010 (2), 0110 (6)
        // 1-0-: 1000 (8), 1001 (9), 1100 (12), 1101 (13)
        // Total ones: {1, 2, 3, 6, 8, 9, 12, 13}
        int[] expectedOnes = [1, 2, 3, 6, 8, 9, 12, 13];
        for (int i = 0; i < 16; i++)
        {
            var expected = expectedOnes.Contains(i) ? 1 : 0;
            Assert.AreEqual(expected, truthValues[i], $"Mismatch at minterm {i}");
        }
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
