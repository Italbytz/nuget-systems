using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Italbytz.ComputingSystems;

public static class PlaFormat
{
    public static string GeneratePla(int variableCount, int[] truthValues, string[]? variableNames = null, string outputName = "y")
    {
        var sb = new StringBuilder();
        sb.AppendLine($".i {variableCount}");
        sb.AppendLine(".o 1");

        var labels = variableNames != null && variableNames.Length == variableCount
            ? string.Join(" ", variableNames)
            : string.Join(" ", Enumerable.Range(0, variableCount).Reverse().Select(i => $"x{i}"));

        sb.AppendLine($".ilb {labels}");
        sb.AppendLine($".ob {outputName}");

        var activeEntries = new List<(string pattern, string output)>();
        for (var minterm = 0; minterm < truthValues.Length; minterm++)
        {
            if (truthValues[minterm] == 1)
            {
                var bin = Convert.ToString(minterm, 2).PadLeft(variableCount, '0');
                activeEntries.Add((bin, "1"));
            }
            else if (truthValues[minterm] == 2)
            {
                var bin = Convert.ToString(minterm, 2).PadLeft(variableCount, '0');
                activeEntries.Add((bin, "-"));
            }
        }

        sb.AppendLine($".p {activeEntries.Count}");
        foreach (var (pattern, output) in activeEntries)
        {
            sb.AppendLine($"{pattern} {output}");
        }

        sb.AppendLine(".e");
        return sb.ToString().TrimEnd();
    }

    public static string GeneratePlaFromCubes(int variableCount, IEnumerable<string> cubes, string[]? variableNames = null, string outputName = "y")
    {
        var sb = new StringBuilder();
        sb.AppendLine($".i {variableCount}");
        sb.AppendLine(".o 1");

        var labels = variableNames != null && variableNames.Length == variableCount
            ? string.Join(" ", variableNames)
            : string.Join(" ", Enumerable.Range(0, variableCount).Reverse().Select(i => $"x{i}"));

        sb.AppendLine($".ilb {labels}");
        sb.AppendLine($".ob {outputName}");

        var cubeList = cubes.Where(c => !string.IsNullOrWhiteSpace(c) && c != "0").ToList();

        sb.AppendLine($".p {cubeList.Count}");
        foreach (var cube in cubeList)
        {
            var pattern = FormatCubeToPlaPattern(variableCount, cube);
            sb.AppendLine($"{pattern} 1");
        }

        sb.AppendLine(".e");
        return sb.ToString().TrimEnd();
    }

    public static string FormatCubeToPlaPattern(int variableCount, string cubeStr)
    {
        if (string.IsNullOrWhiteSpace(cubeStr) || cubeStr == "0")
        {
            return string.Empty;
        }

        if (cubeStr == "1")
        {
            return new string('-', variableCount);
        }

        var chars = new char[variableCount];
        for (var pos = 0; pos < variableCount; pos++)
        {
            var bitIndex = variableCount - pos - 1;
            if (cubeStr.Contains($"!x{bitIndex}") || cubeStr.Contains($"~x{bitIndex}"))
            {
                chars[pos] = '0';
            }
            else if (Regex.IsMatch(cubeStr, $@"(?<![!~])\bx{bitIndex}\b"))
            {
                chars[pos] = '1';
            }
            else
            {
                chars[pos] = '-';
            }
        }

        return new string(chars);
    }

    public static bool TryParsePla(string plaText, out int variableCount, out int[] truthValues, out string? errorMessage)
    {
        variableCount = 4;
        truthValues = [];
        errorMessage = null;

        if (string.IsNullOrWhiteSpace(plaText))
        {
            errorMessage = "PLA-Text ist leer.";
            return false;
        }

        var lines = plaText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var numInputs = 0;
        var cubeEntries = new List<(string pattern, string output)>();

        foreach (var rawLine in lines)
        {
            var line = rawLine.Trim();
            if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
            {
                continue;
            }

            if (line.StartsWith(".i ", StringComparison.OrdinalIgnoreCase))
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2 && int.TryParse(parts[1], out var parsedI))
                {
                    numInputs = parsedI;
                }
            }
            else if (line.StartsWith(".e", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }
            else if (line.StartsWith("."))
            {
                continue;
            }
            else
            {
                var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 1)
                {
                    var pattern = parts[0].Trim();
                    var output = parts.Length >= 2 ? parts[1].Trim() : "1";
                    cubeEntries.Add((pattern, output));
                }
            }
        }

        if (numInputs <= 0 && cubeEntries.Count > 0)
        {
            numInputs = cubeEntries[0].pattern.Length;
        }

        if (numInputs is < 2 or > 4)
        {
            errorMessage = $"Nur 2 bis 4 Eingangsvariablen werden unterstützt (gefunden: {numInputs}).";
            return false;
        }

        variableCount = numInputs;
        var len = 1 << variableCount;
        truthValues = new int[len];

        foreach (var (pattern, output) in cubeEntries)
        {
            if (pattern.Length != variableCount)
            {
                errorMessage = $"Muster '{pattern}' hat nicht die erwartete Länge {variableCount}.";
                return false;
            }

            var outVal = 1;
            if (output == "0")
            {
                outVal = 0;
            }
            else if (output == "-" || output.Equals("x", StringComparison.OrdinalIgnoreCase) || output == "2")
            {
                outVal = 2;
            }

            for (var m = 0; m < len; m++)
            {
                var matches = true;
                for (var pos = 0; pos < variableCount; pos++)
                {
                    var bitIndex = variableCount - pos - 1;
                    var bit = (m >> bitIndex) & 1;
                    var c = pattern[pos];
                    if (c == '1' && bit != 1)
                    {
                        matches = false;
                        break;
                    }

                    if (c == '0' && bit != 0)
                    {
                        matches = false;
                        break;
                    }
                }

                if (matches)
                {
                    truthValues[m] = outVal;
                }
            }
        }

        return true;
    }
}
