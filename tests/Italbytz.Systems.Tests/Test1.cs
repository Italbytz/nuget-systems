using Italbytz.ComputingSystems;
using Italbytz.ComputingSystems.Abstractions;
using Italbytz.Networking;
using Italbytz.Networking.Abstractions;
using Italbytz.OperatingSystems;
using Italbytz.OperatingSystems.Abstractions;

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

    [TestMethod]
    public void Priority_scheduling_solver_returns_expected_average_time()
    {
        ISchedulingSolver solver = new PrioritySchedulingSolver();

        var solution = solver.Solve(new SchedulingParameters(
            [4, 3, 8, 1, 5],
            ["sehr hoch", "mittel", "sehr niedrig", "hoch", "niedrig"]));

        Assert.AreEqual(10.2, solution.Time, 0.0001);
    }

    [TestMethod]
    public void Fifo_page_replacement_solver_counts_page_faults()
    {
        IPageReplacementSolver solver = new FIFOSolver();

        var solution = solver.Solve(new PageReplacementParameters([1, 2, 3, 4, 1, 2, 5, 1, 2, 3, 4, 5])
        {
            MemorySize = 3
        });

        Assert.AreEqual(9, solution.Steps[^1].Count);
    }

    [TestMethod]
    public void Buddy_solver_records_allocation_history()
    {
        IBuddySolver solver = new BuddySolver();

        var solution = solver.Solve(new BuddyParameters
        {
            Processes = ["A", "B", "C", "D", "E"],
            Requests = [65, 30, 94, 34, 136],
            FreeOrder = ["D"]
        });

        Assert.IsTrue(solution.History.Count > 0);
    }
}
