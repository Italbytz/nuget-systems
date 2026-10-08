using System;
using System.Collections.Generic;

namespace Italbytz.ComputingSystems;

public class HazardTransitionQuestion
{
    public int StartState { get; set; }
    public int EndState { get; set; }
    public string ExpectedSolutionText { get; set; } = string.Empty;

    public HazardTransitionQuestion() { }

    public HazardTransitionQuestion(int startState, int endState, string solutionText)
    {
        StartState = startState;
        EndState = endState;
        ExpectedSolutionText = solutionText;
    }
}

public class HazardModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int[] FunctionVector { get; set; } = new int[16];
    public List<int> Minterms { get; set; } = new();
    public List<HazardTransitionQuestion> Questions { get; set; } = new();
    public string DecouplingRowsHint { get; set; } = string.Empty;
}

public static class HazardCatalog
{
    private static readonly Dictionary<int, HazardModel> Models = new()
    {
        [1] = CreateModel1(),
        [2] = CreateModel2(),
        [3] = CreateModel3()
    };

    public static IReadOnlyCollection<HazardModel> GetAllModels() => Models.Values;

    public static HazardModel GetModel(int question)
    {
        if (Models.TryGetValue(question, out var model))
        {
            return model;
        }

        throw new ArgumentOutOfRangeException(nameof(question), $"Unknown HazardModel question id: {question}");
    }

    private static HazardModel CreateModel1()
    {
        return new HazardModel
        {
            Id = 1,
            Name = "Funktions-Hazards TI 01",
            FunctionVector = new int[]
            {
                1, 0, 0, 1,
                0, 0, 1, 1,
                1, 1, 0, 1,
                0, 0, 1, 1
            },
            Minterms = new List<int> { 0, 3, 6, 7, 8, 9, 11, 14, 15 },
            DecouplingRowsHint = "Zeile $00$: $1, 0, 1, 0$; Zeile $01$: $0, 0, 1, 1$; Zeile $11$: $0, 0, 1, 1$; Zeile $10$: $1, 1, 1, 0$; alle übrigen Zellen sind $0$",
            Questions = new List<HazardTransitionQuestion>
            {
                new(8, 11, "Ja, beim Weg 1000, 1010, 1011 tritt ein statischer Hazard auf."),
                new(2, 15, "Nein, auf jedem kürzesten Weg wechselt der Wert nur ein Mal."),
                new(0, 13, "Ja, beim Weg 0000, 0001, 1001, 1101 tritt ein dynamischer Hazard auf."),
                new(4, 13, "Nein, alle kürzesten Wege sind konstant 0.")
            }
        };
    }

    private static HazardModel CreateModel2()
    {
        return new HazardModel
        {
            Id = 2,
            Name = "Funktions-Hazards TI 02",
            FunctionVector = new int[]
            {
                1, 1, 0, 0,
                1, 1, 1, 0,
                0, 0, 0, 0,
                1, 1, 0, 1
            },
            Minterms = new List<int> { 0, 1, 4, 5, 6, 12, 13, 15 },
            DecouplingRowsHint = "Zeile $00$: $1, 1, 0, 0$; Zeile $01$: $1, 1, 0, 1$; Zeile $11$: $1, 1, 1, 0$; Zeile $10$: $0, 0, 0, 0$; alle übrigen Zellen sind $0$",
            Questions = new List<HazardTransitionQuestion>
            {
                new(1, 13, "Ja, beim Weg 0001, 1001, 1101 tritt ein statischer Hazard auf."),
                new(4, 13, "Nein, alle kürzesten Wege sind konstant 1."),
                new(8, 6, "Ja, beim Weg 0111, 1111, 1110, 1100 tritt ein statischer Hazard auf."),
                new(7, 12, "Ja, beim Weg 0111, 1111, 1110, 1100 tritt ein statischer Hazard auf.")
            }
        };
    }

    private static HazardModel CreateModel3()
    {
        return new HazardModel
        {
            Id = 3,
            Name = "Funktions-Hazards TI 03",
            FunctionVector = new int[]
            {
                1, 0, 1, 0,
                0, 0, 1, 0,
                0, 1, 0, 1,
                1, 1, 1, 1
            },
            Minterms = new List<int> { 0, 2, 6, 9, 11, 12, 13, 14, 15 },
            DecouplingRowsHint = "Zeile $00$: $1, 0, 0, 1$; Zeile $01$: $0, 0, 0, 1$; Zeile $11$: $1, 1, 1, 1$; Zeile $10$: $0, 1, 1, 0$; alle übrigen Zellen sind $0$",
            Questions = new List<HazardTransitionQuestion>
            {
                new(1, 7, "Nein, alle kürzesten Wege sind konstant 0."),
                new(1, 14, "Ja, beim Weg 0001, 1001, 1011, 1010, 1110 tritt ein Hazard auf."),
                new(9, 2, "Ja, beim Weg 1001, 1011, 1010, 0010 tritt ein Hazard auf."),
                new(0, 7, "Ja, beim Weg 0000, 0100, 0110, 0111 tritt ein Hazard auf.")
            }
        };
    }
}
