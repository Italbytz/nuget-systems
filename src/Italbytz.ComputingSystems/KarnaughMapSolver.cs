using System;
using System.Collections.Generic;
using System.Linq;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class KarnaughMapSolver : IKarnaughMapSolver
{
    public IKarnaughMapSolution Solve(IKarnaughMapParameters parameters)
    {
        BooleanMinimizationEngine.ValidateTruthValues(parameters.VariableCount, parameters.TruthValues, allowDontCare: true);

        var rowBits = parameters.VariableCount / 2;
        var columnBits = (parameters.VariableCount + 1) / 2;
        var rowCount = 1 << rowBits;
        var columnCount = 1 << columnBits;
        var cells = new List<IKarnaughMapCell>();
        var steps = new List<string>
        {
            $"Build Karnaugh map for {parameters.VariableCount} variables with {rowCount} rows and {columnCount} columns.",
            $"Rows use {rowBits} Gray-coded bits, columns use {columnBits} Gray-coded bits."
        };

        for (var row = 0; row < rowCount; row++)
        {
            var rowGray = BooleanMinimizationEngine.GrayCode(row);
            for (var column = 0; column < columnCount; column++)
            {
                var columnGray = BooleanMinimizationEngine.GrayCode(column);
                var minterm = (rowGray << columnBits) | columnGray;
                cells.Add(new KarnaughMapCell
                {
                    RowIndex = row,
                    ColumnIndex = column,
                    MintermIndex = minterm,
                    Value = parameters.TruthValues[minterm],
                    RowCode = BooleanMinimizationEngine.ToBinaryCode(rowGray, Math.Max(rowBits, 1)),
                    ColumnCode = BooleanMinimizationEngine.ToBinaryCode(columnGray, Math.Max(columnBits, 1))
                });
            }
        }

        var qmcResult = BooleanMinimizationEngine.Solve(parameters.VariableCount, parameters.TruthValues);
        steps.AddRange(qmcResult.Steps.Select(step => $"QMC basis: {step}"));

        var groups = qmcResult.SelectedPrimeImplicants
            .Select(implicant => new KarnaughMapGroup
            {
                Term = BooleanMinimizationEngine.FormatImplicant(parameters.VariableCount, implicant.FixedBits, implicant.Mask),
                CoveredMintermIndices = implicant.CoveredMinterms.OrderBy(value => value).ToArray(),
                CoveredCellIndices = cells
                    .Select((cell, index) => (cell, index))
                    .Where(entry => implicant.CoveredIndices.Contains(entry.cell.MintermIndex))
                    .Select(entry => entry.index)
                    .ToArray(),
                IsEssential = qmcResult.EssentialPrimeImplicants.Any(essential => essential.FixedBits == implicant.FixedBits && essential.Mask == implicant.Mask)
            })
            .Cast<IKarnaughMapGroup>()
            .ToArray();

        foreach (var group in groups)
        {
            steps.Add($"KV group {group.Term} covers minterms {string.Join(", ", group.CoveredMintermIndices)}.");
        }

        return new KarnaughMapSolution
        {
            RowCount = rowCount,
            ColumnCount = columnCount,
            Cells = cells.ToArray(),
            Groups = groups,
            Steps = steps
        };
    }
}