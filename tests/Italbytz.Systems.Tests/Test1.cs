using Italbytz.ComputingSystems;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.Systems.Tests;

[TestClass]
public sealed class ComputingSystemsTests
{
    [TestMethod]
    public void Binary_addition_solver_sums_operands()
    {
        IBinaryAdditionSolver solver = new BinaryAdditionSolver();

        var solution = solver.Solve(new BinaryAdditionParameters(3, 4));

        Assert.AreEqual((ushort)7, solution.Sum);
    }

    [TestMethod]
    public void Twos_complement_solver_returns_negative_signed_value()
    {
        ITwosComplementSolver solver = new TwosComplementSolver();

        var solution = solver.Solve(new TwosComplementParameters(5));

        Assert.AreEqual((sbyte)-5, solution.ComplementBinary);
    }
}
