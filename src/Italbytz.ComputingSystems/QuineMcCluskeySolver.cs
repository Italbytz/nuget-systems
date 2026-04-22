using System.Linq;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class QuineMcCluskeySolver : IQuineMcCluskeySolver
{
    public IQuineMcCluskeySolution Solve(IQuineMcCluskeyParameters parameters)
    {
        var result = BooleanMinimizationEngine.Solve(parameters.VariableCount, parameters.TruthValues);

        return new QuineMcCluskeySolution
        {
            MinimalDnf = result.MinimalDnf,
            PrimeImplicants = result.PrimeImplicants.Select(implicant => BooleanMinimizationEngine.FormatImplicant(parameters.VariableCount, implicant.FixedBits, implicant.Mask)).ToArray(),
            EssentialPrimeImplicants = result.EssentialPrimeImplicants.Select(implicant => BooleanMinimizationEngine.FormatImplicant(parameters.VariableCount, implicant.FixedBits, implicant.Mask)).ToArray(),
            SelectedPrimeImplicants = result.SelectedPrimeImplicants.Select(implicant => BooleanMinimizationEngine.FormatImplicant(parameters.VariableCount, implicant.FixedBits, implicant.Mask)).ToArray(),
            Steps = result.Steps.ToList()
        };
    }
}