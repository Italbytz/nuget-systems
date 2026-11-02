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
}
