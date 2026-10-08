using System;
using System.Collections.Generic;
using System.Linq;

namespace Italbytz.ComputingSystems;

public class MooreMachineModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TaskDescription { get; set; } = string.Empty;
    public string TaskImage { get; set; } = string.Empty;
    public string SolutionImage { get; set; } = string.Empty;
    public string InputLabel { get; set; } = "Eingang";
    public bool IncludeKvIntro { get; set; } = false;

    // Transition table rows: (z0, z1, e) -> (next_z0, next_z1)
    // -1 represents Don't-Care ("-")
    public List<MooreTransitionRow> Transitions { get; set; } = new();

    // Output table rows: (z0, z1) -> (a0, a1)
    // -1 represents Don't-Care ("-")
    public List<MooreOutputRow> Outputs { get; set; } = new();

    // Teilaufgabe (c): Next state KV minimization
    public List<int> NextZ0Minterms { get; set; } = new();
    public List<int> NextZ0DontCares { get; set; } = new();
    public List<string> NextZ0Implicants { get; set; } = new();
    public string NextZ0Formula { get; set; } = string.Empty;

    public List<int> NextZ1Minterms { get; set; } = new();
    public List<int> NextZ1DontCares { get; set; } = new();
    public List<string> NextZ1Implicants { get; set; } = new();
    public string NextZ1Formula { get; set; } = string.Empty;

    // Teilaufgabe (d): Output KV minimization
    public string OutputKvVarX { get; set; } = "Ausgabe";
    public string OutputKvVarY { get; set; } = "Zustand";
    public List<int> A0Minterms { get; set; } = new();
    public List<int> A0DontCares { get; set; } = new();
    public List<string> A0Implicants { get; set; } = new();
    public string A0Formula { get; set; } = string.Empty;

    public List<int> A1Minterms { get; set; } = new();
    public List<int> A1DontCares { get; set; } = new();
    public List<string> A1Implicants { get; set; } = new();
    public string A1Formula { get; set; } = string.Empty;

    public (int nextZ0, int nextZ1) GetNextState(int z0, int z1, int e)
    {
        var row = Transitions.FirstOrDefault(t => t.Z0 == z0 && t.Z1 == z1 && t.E == e);
        if (row != null)
        {
            return (row.NextZ0, row.NextZ1);
        }
        return (-1, -1);
    }

    public (int a0, int a1) GetOutput(int z0, int z1)
    {
        var row = Outputs.FirstOrDefault(o => o.Z0 == z0 && o.Z1 == z1);
        if (row != null)
        {
            return (row.A0, row.A1);
        }
        return (-1, -1);
    }
}

public class MooreTransitionRow
{
    public int Z0 { get; set; }
    public int Z1 { get; set; }
    public int E { get; set; }
    public int NextZ0 { get; set; } // 0, 1, or -1 (DC)
    public int NextZ1 { get; set; } // 0, 1, or -1 (DC)

    public MooreTransitionRow() { }

    public MooreTransitionRow(int z0, int z1, int e, int nextZ0, int nextZ1)
    {
        Z0 = z0;
        Z1 = z1;
        E = e;
        NextZ0 = nextZ0;
        NextZ1 = nextZ1;
    }
}

public class MooreOutputRow
{
    public int Z0 { get; set; }
    public int Z1 { get; set; }
    public int A0 { get; set; } // 0, 1, or -1 (DC)
    public int A1 { get; set; } // 0, 1, or -1 (DC)

    public MooreOutputRow() { }

    public MooreOutputRow(int z0, int z1, int a0, int a1)
    {
        Z0 = z0;
        Z1 = z1;
        A0 = a0;
        A1 = a1;
    }
}
