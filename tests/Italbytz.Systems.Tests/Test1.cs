using Italbytz.ComputingSystems;
using Italbytz.ComputingSystems.Abstractions;
using Italbytz.Networking;
using Italbytz.Networking.Abstractions;

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

    [TestMethod]
    public void Bitencoding_solver_returns_expected_line_codes()
    {
        IBitencodingSolver solver = new BitencodingSolver();

        var solution = solver.Solve(new BitencodingParameters([1, 0, 1, 1, 0]));

        CollectionAssert.AreEqual(new[] { "+", "-", "+", "+", "-" }, solution.NRZ);
        CollectionAssert.AreEqual(new[] { "+", "+", "-", "+", "+" }, solution.NRZI);
        CollectionAssert.AreEqual(new[] { "+", "+", "0", "-", "-" }, solution.MLT3);
    }

    [TestMethod]
    public void Crc_solver_generates_expected_remainder_and_check()
    {
        ICRCSolver solver = new CRCSolver();

        var solution = solver.Solve(new CRCParameters(42));

        StringAssert.EndsWith(solution.Calculation, "10110");
        StringAssert.EndsWith(solution.Check, "00000");
    }

    [TestMethod]
    public void Netmask_solver_splits_network_and_host_address()
    {
        INetmaskSolver solver = new NetmaskSolver();

        var solution = solver.Solve(new NetmaskParameters("192.168.10.42", 24));

        Assert.AreEqual("192.168.10.0", solution.NetworkAddress.ToString());
        Assert.AreEqual("0.0.0.42", solution.HostAddress.ToString());
    }
}
