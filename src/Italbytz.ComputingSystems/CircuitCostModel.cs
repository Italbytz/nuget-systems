using System;
using System.Collections.Generic;
using System.Linq;

namespace Italbytz.ComputingSystems;

public enum CircuitGateType
{
    And,
    Or,
    Not,
    Nand,
    Nor,
    Xor,
    Xnor
}

public class CircuitGateInfo
{
    public string Id { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public CircuitGateType Type { get; set; }
    public int Stage { get; set; } = 1;
    public int InputCount { get; set; } = 2;
    public string[] Inputs { get; set; } = Array.Empty<string>();
    public bool IsCritical { get; set; } = false;
}

public class CircuitVariant
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Architecture { get; set; } = string.Empty;
    public string Formula { get; set; } = string.Empty;
    public int CircuitDepth { get; set; }
    public int GateCount { get; set; }
    public int TotalFanIn { get; set; }
    public int WeightedCost => (100 * CircuitDepth) + TotalFanIn;
    public string CriticalPathDescription { get; set; } = string.Empty;
    public List<CircuitGateInfo> Gates { get; set; } = new();
    public Func<bool[], bool>? EvaluationFunc { get; set; }

    public bool Evaluate(bool[] inputs) => EvaluationFunc?.Invoke(inputs) ?? false;
}

public class CircuitComparisonModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FunctionDescription { get; set; } = string.Empty;
    public int VariableCount { get; set; } = 4;
    public string[] VariableNames { get; set; } = Array.Empty<string>();
    public CircuitVariant VariantA { get; set; } = new();
    public CircuitVariant VariantB { get; set; } = new();
    public string TradeoffExplanation { get; set; } = string.Empty;
    public string NandFormula { get; set; } = string.Empty;
    public int NandGateCount { get; set; }
    public int NandDepth { get; set; }
}

public static class CircuitCostCatalog
{
    private static readonly Dictionary<int, CircuitComparisonModel> Models = new()
    {
        [0] = CreateParityModel(),
        [1] = CreateMajorityModel(),
        [2] = CreateComparatorModel()
    };

    public static IReadOnlyCollection<CircuitComparisonModel> GetAllModels() => Models.Values;

    public static CircuitComparisonModel GetModel(int id)
    {
        if (Models.TryGetValue(id, out var model))
        {
            return model;
        }

        return Models[0];
    }

    private static CircuitComparisonModel CreateParityModel()
    {
        return new CircuitComparisonModel
        {
            Id = 0,
            Name = "Paritätsfunktion 4-Bit: Serielle Kette vs. Paritätsbaum",
            FunctionDescription = "y = x3 ⊕ x2 ⊕ x1 ⊕ x0 (Ungerade Parität / Exklusiv-ODER-Verknüpfung)",
            VariableCount = 4,
            VariableNames = new[] { "x0", "x1", "x2", "x3" },
            VariantA = new CircuitVariant
            {
                Id = "chain",
                Name = "Schaltung A (Serielle Kaskade / Kette)",
                Architecture = "Kette (Seriell)",
                Formula = "y = ((x0 ⊕ x1) ⊕ x2) ⊕ x3",
                CircuitDepth = 3,
                GateCount = 3,
                TotalFanIn = 6, // 3 * 2 Eingänge
                CriticalPathDescription = "x0 / x1 → XOR 1 → XOR 2 → XOR 3 → y (Länge = 3 Stufen)",
                Gates = new List<CircuitGateInfo>
                {
                    new() { Id = "G1", Label = "XOR 1", Type = CircuitGateType.Xor, Stage = 1, InputCount = 2, Inputs = new[] { "x0", "x1" }, IsCritical = true },
                    new() { Id = "G2", Label = "XOR 2", Type = CircuitGateType.Xor, Stage = 2, InputCount = 2, Inputs = new[] { "G1", "x2" }, IsCritical = true },
                    new() { Id = "G3", Label = "XOR 3", Type = CircuitGateType.Xor, Stage = 3, InputCount = 2, Inputs = new[] { "G2", "x3" }, IsCritical = true }
                },
                EvaluationFunc = (inputs) => (inputs[0] ^ inputs[1]) ^ (inputs[2] ^ inputs[3])
            },
            VariantB = new CircuitVariant
            {
                Id = "tree",
                Name = "Schaltung B (Paritätsbaum / Baumstruktur)",
                Architecture = "Baum (Logarithmisch)",
                Formula = "y = (x0 ⊕ x1) ⊕ (x2 ⊕ x3)",
                CircuitDepth = 2,
                GateCount = 3,
                TotalFanIn = 6, // 3 * 2 Eingänge
                CriticalPathDescription = "Parallele Stufe 1 (G_A, G_B) → Zusammenführung Stufe 2 (G_C) (Länge = 2 Stufen)",
                Gates = new List<CircuitGateInfo>
                {
                    new() { Id = "Ga", Label = "XOR A", Type = CircuitGateType.Xor, Stage = 1, InputCount = 2, Inputs = new[] { "x0", "x1" }, IsCritical = true },
                    new() { Id = "Gb", Label = "XOR B", Type = CircuitGateType.Xor, Stage = 1, InputCount = 2, Inputs = new[] { "x2", "x3" }, IsCritical = true },
                    new() { Id = "Gc", Label = "XOR C", Type = CircuitGateType.Xor, Stage = 2, InputCount = 2, Inputs = new[] { "Ga", "Gb" }, IsCritical = true }
                },
                EvaluationFunc = (inputs) => (inputs[0] ^ inputs[1]) ^ (inputs[2] ^ inputs[3])
            },
            TradeoffExplanation = "Beide Schaltungen besitzen den exakt gleichen Bauteil- und Transistorbedarf (C_A = 3 Gatter, C_A' = 6 Eingänge). " +
                                  "Die Baumstruktur reduziert jedoch die Schaltungstiefe von 3 auf 2 Stufen (logarithmische Laufzeit: ⌈log₂(4)⌉ = 2 statt n - 1 = 3), " +
                                  "wodurch die Signallaufzeit um 33 % sinkt und eine deutlich höhere Maximal-Taktfrequenz erreicht werden kann.",
            NandFormula = "y = ((x0 ⊕ x1) ⊕ (x2 ⊕ x3)) [XOR aus jeweils 4 NAND-Gattern]",
            NandGateCount = 12,
            NandDepth = 6
        };
    }

    private static CircuitComparisonModel CreateMajorityModel()
    {
        return new CircuitComparisonModel
        {
            Id = 1,
            Name = "Mehrheitsentscheid 3-Bit: Zweistufige DNF vs. Mehrstufige Faktorisierung",
            FunctionDescription = "f(x2, x1, x0) = 1, wenn mindestens 2 der 3 Eingänge gleich 1 sind (Majoritätsfunktion)",
            VariableCount = 3,
            VariableNames = new[] { "x0", "x1", "x2" },
            VariantA = new CircuitVariant
            {
                Id = "dnf",
                Name = "Schaltung A (Zweistufige DNF)",
                Architecture = "Zweistufig (AND-OR)",
                Formula = "y = (x2 ∧ x1) ∨ (x2 ∧ x0) ∨ (x1 ∧ x0)",
                CircuitDepth = 2,
                GateCount = 4, // 3 AND + 1 OR
                TotalFanIn = 9, // (2 + 2 + 2) + 3
                CriticalPathDescription = "Eingänge → AND-Stufe (Stufe 1) → 3-fach OR (Stufe 2) → y (Länge = 2 Stufen)",
                Gates = new List<CircuitGateInfo>
                {
                    new() { Id = "G1", Label = "AND 1", Type = CircuitGateType.And, Stage = 1, InputCount = 2, Inputs = new[] { "x2", "x1" }, IsCritical = true },
                    new() { Id = "G2", Label = "AND 2", Type = CircuitGateType.And, Stage = 1, InputCount = 2, Inputs = new[] { "x2", "x0" }, IsCritical = true },
                    new() { Id = "G3", Label = "AND 3", Type = CircuitGateType.And, Stage = 1, InputCount = 2, Inputs = new[] { "x1", "x0" }, IsCritical = true },
                    new() { Id = "G4", Label = "OR (Fan-In 3)", Type = CircuitGateType.Or, Stage = 2, InputCount = 3, Inputs = new[] { "G1", "G2", "G3" }, IsCritical = true }
                },
                EvaluationFunc = (inputs) => (inputs[2] && inputs[1]) || (inputs[2] && inputs[0]) || (inputs[1] && inputs[0])
            },
            VariantB = new CircuitVariant
            {
                Id = "factored",
                Name = "Schaltung B (Mehrstufig faktorisierte Schaltung)",
                Architecture = "Mehrstufig (Kaskadiert)",
                Formula = "y = (x2 ∧ (x1 ∨ x0)) ∨ (x1 ∧ x0)",
                CircuitDepth = 3,
                GateCount = 4, // 1 OR + 1 AND + 1 AND + 1 OR
                TotalFanIn = 8, // 4 * 2
                CriticalPathDescription = "x1/x0 → OR (Stufe 1) → AND mit x2 (Stufe 2) → OR-Ausgang (Stufe 3) (Länge = 3 Stufen)",
                Gates = new List<CircuitGateInfo>
                {
                    new() { Id = "Ga", Label = "OR 1", Type = CircuitGateType.Or, Stage = 1, InputCount = 2, Inputs = new[] { "x1", "x0" }, IsCritical = true },
                    new() { Id = "Gb", Label = "AND 1", Type = CircuitGateType.And, Stage = 1, InputCount = 2, Inputs = new[] { "x1", "x0" }, IsCritical = false },
                    new() { Id = "Gc", Label = "AND 2", Type = CircuitGateType.And, Stage = 2, InputCount = 2, Inputs = new[] { "x2", "Ga" }, IsCritical = true },
                    new() { Id = "Gd", Label = "OR 2", Type = CircuitGateType.Or, Stage = 3, InputCount = 2, Inputs = new[] { "Gc", "Gb" }, IsCritical = true }
                },
                EvaluationFunc = (inputs) => (inputs[2] && (inputs[1] || inputs[0])) || (inputs[1] && inputs[0])
            },
            TradeoffExplanation = "Die zweistufige DNF (Schaltung A) ist mit Tiefe C_S = 2 maximal schnell, benötigt aber ein OR-Gatter mit 3 Eingängen (Gesamt-Fan-In C_A' = 9). " +
                                  "Die faktorisierte Variante B kommt ausschließlich mit elementaren 2-Eingangs-Gattern aus (C_A' = 8), erkauft dies jedoch mit einer zusätzlichen Gatterlaufzeitstufe (C_S = 3).",
            NandFormula = "y = ((x2 NAND x1) NAND (x2 NAND x0) NAND (x1 NAND x0))",
            NandGateCount = 4,
            NandDepth = 2
        };
    }

    private static CircuitComparisonModel CreateComparatorModel()
    {
        return new CircuitComparisonModel
        {
            Id = 2,
            Name = "2-Bit Komparator (A == B): Bitweise XNOR-Stufe vs. 4-Bit-DNF",
            FunctionDescription = "A = (a1, a0), B = (b1, b0) → y = 1 genau dann, wenn A == B",
            VariableCount = 4,
            VariableNames = new[] { "a0", "a1", "b0", "b1" },
            VariantA = new CircuitVariant
            {
                Id = "comp-tree",
                Name = "Schaltung A (Bitweise Äquivalenz XNOR + AND)",
                Architecture = "Baumstruktur (XNOR-AND)",
                Formula = "y = (a1 XNOR b1) ∧ (a0 XNOR b0)",
                CircuitDepth = 2,
                GateCount = 3, // 2 XNOR + 1 AND
                TotalFanIn = 6,
                CriticalPathDescription = "Eingänge → XNOR-Stufe (Stufe 1) → AND (Stufe 2) → y (Länge = 2 Stufen)",
                Gates = new List<CircuitGateInfo>
                {
                    new() { Id = "G1", Label = "XNOR 1", Type = CircuitGateType.Xnor, Stage = 1, InputCount = 2, Inputs = new[] { "a1", "b1" }, IsCritical = true },
                    new() { Id = "G2", Label = "XNOR 0", Type = CircuitGateType.Xnor, Stage = 1, InputCount = 2, Inputs = new[] { "a0", "b0" }, IsCritical = true },
                    new() { Id = "G3", Label = "AND", Type = CircuitGateType.And, Stage = 2, InputCount = 2, Inputs = new[] { "G1", "G2" }, IsCritical = true }
                },
                EvaluationFunc = (inputs) => (inputs[1] == inputs[3]) && (inputs[0] == inputs[2])
            },
            VariantB = new CircuitVariant
            {
                Id = "comp-dnf",
                Name = "Schaltung B (Kanonische zweistufige DNF)",
                Architecture = "Zweistufig (4 Minterme)",
                Formula = "y = (!a1!a0!b1!b0) ∨ (!a1 a0!b1 b0) ∨ (a1!a0 b1!b0) ∨ (a1 a0 b1 b0)",
                CircuitDepth = 2,
                GateCount = 5, // 4 AND-4 + 1 OR-4
                TotalFanIn = 20, // 4 * 4 + 4
                CriticalPathDescription = "Eingänge → 4x AND-4 (Stufe 1) → OR-4 (Stufe 2) (Länge = 2 Stufen)",
                Gates = new List<CircuitGateInfo>
                {
                    new() { Id = "M0", Label = "AND m0", Type = CircuitGateType.And, Stage = 1, InputCount = 4, Inputs = new[] { "!a1", "!a0", "!b1", "!b0" }, IsCritical = true },
                    new() { Id = "M5", Label = "AND m5", Type = CircuitGateType.And, Stage = 1, InputCount = 4, Inputs = new[] { "!a1", "a0", "!b1", "b0" }, IsCritical = true },
                    new() { Id = "M10", Label = "AND m10", Type = CircuitGateType.And, Stage = 1, InputCount = 4, Inputs = new[] { "a1", "!a0", "b1", "!b0" }, IsCritical = true },
                    new() { Id = "M15", Label = "AND m15", Type = CircuitGateType.And, Stage = 1, InputCount = 4, Inputs = new[] { "a1", "a0", "b1", "b0" }, IsCritical = true },
                    new() { Id = "OR4", Label = "OR-4", Type = CircuitGateType.Or, Stage = 2, InputCount = 4, Inputs = new[] { "M0", "M5", "M10", "M15" }, IsCritical = true }
                },
                EvaluationFunc = (inputs) => (inputs[1] == inputs[3]) && (inputs[0] == inputs[2])
            },
            TradeoffExplanation = "Die modulare Äquivalenzschaltung A nutzt die algebraische Struktur aus und spart drastisch Gatter (3 statt 5) und Transistoren (C_A' = 6 statt 20), " +
                                  "während sie die minimale Schaltungstiefe von C_S = 2 beibehält.",
            NandFormula = "y = ((a1 XNOR b1) NAND (a0 XNOR b0)) [mit Inverter]",
            NandGateCount = 10,
            NandDepth = 4
        };
    }
}
