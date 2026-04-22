using System.Linq;
using Italbytz.ComputingSystems;
using Italbytz.ComputingSystems.Abstractions;
using Italbytz.Networking;
using Italbytz.Networking.Abstractions;
using Italbytz.OperatingSystems;
using Italbytz.OperatingSystems.Abstractions;
using Italbytz.Systems.Abstractions;

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
    public void Binary_addition_solver_exposes_bitwise_trace()
    {
        IBinaryAdditionSolver solver = new BinaryAdditionSolver();

        var solution = solver.Solve(new BinaryAdditionParameters(255, 5));

        Assert.AreEqual((ushort)260, solution.Sum);
        Assert.IsGreaterThanOrEqualTo(8, solution.Steps.Count);
        StringAssert.Contains(solution.Steps[0], "Bit 0");
    }

    [TestMethod]
    public void Decimal_to_binary_solver_converts_via_division_trace()
    {
        IDecimalToBinarySolver solver = new DecimalToBinarySolver();

        var solution = solver.Solve(new DecimalToBinaryParameters(42));

        Assert.AreEqual(101010, solution.Binary);
        Assert.IsNotEmpty(solution.Steps);
        StringAssert.Contains(solution.Steps[0], "/ 2");
    }

    [TestMethod]
    public void Binary_to_decimal_solver_converts_via_weighted_sum_trace()
    {
        IBinaryToDecimalSolver solver = new BinaryToDecimalSolver();

        var solution = solver.Solve(new BinaryToDecimalParameters(45));

        Assert.AreEqual(45, solution.Decimal);
        Assert.HasCount(8, solution.Steps);
        StringAssert.Contains(solution.Steps[0], "Bit 0");
    }

    [TestMethod]
    public void Twos_complement_solver_exposes_invert_and_increment_trace()
    {
        ITwosComplementSolver solver = new TwosComplementSolver();

        var solution = solver.Solve(new TwosComplementParameters(5));

        Assert.AreEqual((sbyte)-5, solution.ComplementBinary);
        Assert.HasCount(3, solution.Steps);
        StringAssert.Contains(solution.Steps[1], "Invert bits");
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
    public void Scheduling_solvers_expose_trace_steps_via_shared_contract()
    {
        ISchedulingSolver solver = new FCFSSolver();

        var solution = solver.Solve(new SchedulingParameters(
            [6, 2, 8, 3, 5],
            ["mittel", "sehr hoch", "niedrig", "hoch", "sehr niedrig"]));

        Assert.IsTrue(solution is ITracedSolution);
        Assert.IsNotEmpty(solution.Steps);
    }

    [TestMethod]
    public void Realtime_scheduling_solvers_expose_trace_steps_via_shared_contract()
    {
        IRealtimeSchedulingSolver rmsSolver = new RMSSolver();
        IRealtimeSchedulingSolver edfSolver = new EDFSolver();

        var parameters = new RealtimeSchedulingParameters
        {
            Requests = [(1, 4), (2, 5), (4, 10)]
        };

        var rmsSolution = rmsSolver.Solve(parameters);
        var edfSolution = edfSolver.Solve(parameters);

        Assert.IsTrue(rmsSolution is ITracedSolution);
        Assert.IsTrue(edfSolution is ITracedSolution);
        Assert.IsNotEmpty(rmsSolution.Steps);
        Assert.IsNotEmpty(edfSolution.Steps);
        Assert.IsGreaterThanOrEqualTo(32, rmsSolution.Processes.Length);
        Assert.IsGreaterThanOrEqualTo(32, edfSolution.Processes.Length);
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

        Assert.IsNotEmpty(solution.History);
    }

    [TestMethod]
    public void Bitencoding_solver_exposes_trace_steps()
    {
        IBitencodingSolver solver = new BitencodingSolver();

        var solution = solver.Solve(new BitencodingParameters([1, 0, 1, 1, 0]));

        Assert.IsNotEmpty(solution.Steps);
        StringAssert.Contains(solution.Steps[0], "Input bits");
    }

    [TestMethod]
    public void Crc_solver_exposes_trace_steps()
    {
        ICRCSolver solver = new CRCSolver();

        var solution = solver.Solve(new CRCParameters(42));

        Assert.IsNotEmpty(solution.Steps);
        Assert.IsTrue(solution.Steps.Any(step => step.Contains("Generator polynomial")));
    }

    [TestMethod]
    public void Netmask_solver_exposes_trace_steps()
    {
        INetmaskSolver solver = new NetmaskSolver();

        var solution = solver.Solve(new NetmaskParameters("192.168.10.42", 24));

        Assert.IsNotEmpty(solution.Steps);
        Assert.IsTrue(solution.Steps.Any(step => step.Contains("Subnet mask")));
    }
}
