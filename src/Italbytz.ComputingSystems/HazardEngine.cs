using System;
using System.Collections.Generic;
using System.Linq;

namespace Italbytz.ComputingSystems;

public class HazardPathResult
{
    public List<int> States { get; set; } = new();
    public List<int> Values { get; set; } = new();
    public bool HasHazard { get; set; }
    public string HazardType { get; set; } = "Kein Hazard";
    public string HazardLabel { get; set; } = "Stabil";
    public string HazardExplanation { get; set; } = string.Empty;
}

public class HazardAnalysisResult
{
    public int StartState { get; set; }
    public int EndState { get; set; }
    public int HammingDistance { get; set; }
    public List<HazardPathResult> ShortestPaths { get; set; } = new();

    public bool HasAnyHazard => ShortestPaths.Any(p => p.HasHazard);

    public string SummaryTitle
    {
        get
        {
            if (!HasAnyHazard) return "Kein Funktions-Hazard auf kürzesten Wegen";
            var hazards = ShortestPaths.Where(p => p.HasHazard).Select(p => p.HazardType).Distinct();
            return $"Funktions-Hazard erkannt: {string.Join(" & ", hazards)}";
        }
    }

    public string SummaryDescription
    {
        get
        {
            int total = ShortestPaths.Count;
            int hazardCount = ShortestPaths.Count(p => p.HasHazard);
            if (hazardCount == 0)
            {
                return $"Auf allen {total} kürzesten Wegen von {Convert.ToString(StartState, 2).PadLeft(4, '0')} nach {Convert.ToString(EndState, 2).PadLeft(4, '0')} verhält sich die Schaltung fehlerfrei (konstant oder monoton).";
            }
            return $"{hazardCount} von {total} kürzesten Wegen weisen einen Funktions-Hazard auf.";
        }
    }
}

public static class HazardEngine
{
    public static HazardAnalysisResult Analyze(int[] functionVector, int startState, int endState)
    {
        var result = new HazardAnalysisResult
        {
            StartState = startState,
            EndState = endState,
            HammingDistance = CountBitDifferences(startState, endState)
        };

        result.ShortestPaths = ComputeShortestPaths(functionVector, startState, endState);
        return result;
    }

    public static List<HazardPathResult> ComputeShortestPaths(int[] functionVector, int startState, int endState)
    {
        var shortestPaths = new List<HazardPathResult>();

        int diff = startState ^ endState;
        var bitIndices = new List<int>();
        for (int b = 0; b < 4; b++)
        {
            if (((diff >> b) & 1) == 1)
            {
                bitIndices.Add(b);
            }
        }

        if (bitIndices.Count == 0)
        {
            var res = new HazardPathResult();
            res.States.Add(startState);
            res.Values.Add(functionVector[startState]);
            res.HasHazard = false;
            res.HazardLabel = "Kein Wechsel";
            shortestPaths.Add(res);
            return shortestPaths;
        }

        var perms = GetPermutations(bitIndices, bitIndices.Count);
        foreach (var perm in perms)
        {
            var res = new HazardPathResult();
            int current = startState;
            res.States.Add(current);
            res.Values.Add(functionVector[current]);

            foreach (var b in perm)
            {
                current ^= (1 << b);
                res.States.Add(current);
                res.Values.Add(functionVector[current]);
            }

            AnalyzePathHazard(res);
            shortestPaths.Add(res);
        }

        return shortestPaths;
    }

    public static void AnalyzePathHazard(HazardPathResult res)
    {
        int vStart = res.Values.First();
        int vEnd = res.Values.Last();

        int alternations = 0;
        for (int i = 0; i < res.Values.Count - 1; i++)
        {
            if (res.Values[i] != res.Values[i + 1])
                alternations++;
        }

        if (vStart == vEnd)
        {
            bool hasDeviation = res.Values.Any(v => v != vStart);
            if (hasDeviation)
            {
                res.HasHazard = true;
                if (vStart == 1)
                {
                    res.HazardType = "Statischer 1-Hazard";
                    res.HazardLabel = "Statischer 1-Hazard (1 → 0 → 1)";
                    res.HazardExplanation = "Endwerte sind 1, Zwischenzustand fällt auf 0 ab!";
                }
                else
                {
                    res.HazardType = "Statischer 0-Hazard";
                    res.HazardLabel = "Statischer 0-Hazard (0 → 1 → 0)";
                    res.HazardExplanation = "Endwerte sind 0, Zwischenzustand springt kurzzeitig auf 1!";
                }
            }
            else
            {
                res.HasHazard = false;
                res.HazardLabel = "Stabil (Kein Hazard)";
                res.HazardExplanation = $"Funktionswert bleibt konstant {vStart}.";
            }
        }
        else
        {
            if (alternations > 1)
            {
                res.HasHazard = true;
                res.HazardType = "Dynamischer Hazard";
                res.HazardLabel = "Dynamischer Hazard";
                res.HazardExplanation = $"Mehrfacher Pegelwechsel ({alternations} Wechsel)! Signal fluktuiert vor dem Einpegeln.";
            }
            else
            {
                res.HasHazard = false;
                res.HazardLabel = "Monotoner Übergang";
                res.HazardExplanation = "Genau ein sauberer Pegelwechsel, kein Glitch.";
            }
        }
    }

    public static int CountBitDifferences(int a, int b)
    {
        int diff = a ^ b;
        int count = 0;
        while (diff > 0)
        {
            count += (diff & 1);
            diff >>= 1;
        }
        return count;
    }

    private static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
    {
        if (length == 1) return list.Select(t => new[] { t });
        return GetPermutations(list, length - 1)
            .SelectMany(t => list.Where(e => !t.Contains(e)),
                (t1, t2) => t1.Concat(new[] { t2 }));
    }
}
