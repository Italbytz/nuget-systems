namespace Italbytz.ComputingSystems.Abstractions;

public interface IFloatingPointSolver
{
    IFloatingPointSolution Solve(IFloatingPointParameters parameters);
}
