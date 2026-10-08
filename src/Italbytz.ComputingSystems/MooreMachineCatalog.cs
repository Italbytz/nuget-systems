using System;
using System.Collections.Generic;

namespace Italbytz.ComputingSystems;

public static class MooreMachineCatalog
{
    private static readonly Dictionary<int, MooreMachineModel> Models = new()
    {
        [1] = CreateModel1(),
        [2] = CreateModel2(),
        [3] = CreateModel3()
    };

    public static IReadOnlyCollection<MooreMachineModel> GetAllModels() => Models.Values;

    public static MooreMachineModel GetModel(int question)
    {
        if (Models.TryGetValue(question, out var model))
        {
            return model;
        }

        throw new ArgumentOutOfRangeException(nameof(question), $"Unknown MooreMachineModel question id: {question}");
    }

    private static MooreMachineModel CreateModel1()
    {
        return new MooreMachineModel
        {
            Id = 1,
            Name = "Zähler 0, 1, 2 (mit Sättigung)",
            TaskDescription = @"Modellieren Sie einen Automaten zum Zählen, der die Zahlen $0$,$1$ und $2$
ausgeben kann und dabei je nach Eingabe vorwärts oder rückwärts zählt.
Gehen Sie dabei davon aus, dass eine alleine Eingabe die Zählrichtung
bestimmt und beim Vorwärtszählen nach einer $2$ weiterhin eine $2$
ausgegeben wird, beim Rückwärtszählen nach einer $0$ weiterhin eine $0$.",
            TaskImage = "TIUebung05_03.png",
            SolutionImage = "TIUebung05_02.png",
            InputLabel = "Schalter",
            IncludeKvIntro = false,
            Transitions = new List<MooreTransitionRow>
            {
                new(0, 0, 0, 0, 0),
                new(0, 0, 1, 0, 1),
                new(0, 1, 0, 0, 0),
                new(0, 1, 1, 1, 0),
                new(1, 0, 0, 0, 1),
                new(1, 0, 1, 1, 0),
                new(1, 1, 0, -1, -1),
                new(1, 1, 1, -1, -1)
            },
            Outputs = new List<MooreOutputRow>
            {
                new(0, 0, 0, 0),
                new(0, 1, 0, 1),
                new(1, 0, 1, 0),
                new(1, 1, -1, -1)
            },
            NextZ0Minterms = new List<int> { 5, 6 },
            NextZ0DontCares = new List<int> { 3, 7 },
            NextZ0Implicants = new List<string> { @"\implicant{5}{7}", @"\implicant{7}{6}" },
            NextZ0Formula = @"(e\wedge z_{0}) \vee (e\wedge z_{1})",
            NextZ1Minterms = new List<int> { 2, 4 },
            NextZ1DontCares = new List<int> { 3, 7 },
            NextZ1Implicants = new List<string> { @"\implicant{4}{4}", @"\implicant{3}{2}" },
            NextZ1Formula = @"(e\wedge\neg z_{0}\wedge\neg z_{1}) \vee (\neg e\wedge z_{0})",
            OutputKvVarX = "Ausgabe",
            OutputKvVarY = "Zustand",
            A0Minterms = new List<int> { 2 },
            A0DontCares = new List<int> { 3 },
            A0Implicants = new List<string> { @"\implicant{2}{3}" },
            A0Formula = @"z_0",
            A1Minterms = new List<int> { 1 },
            A1DontCares = new List<int> { 3 },
            A1Implicants = new List<string> { @"\implicant{1}{3}" },
            A1Formula = @"z_1"
        };
    }

    private static MooreMachineModel CreateModel2()
    {
        return new MooreMachineModel
        {
            Id = 2,
            Name = "Zähler 1, 2, 3 (zyklisch)",
            TaskDescription = @"Modellieren Sie einen Automaten zum Zählen, der die Zahlen $1$,$2$ und $3$
ausgeben kann und dabei je nach Eingabe vorwärts oder rückwärts zählt.
Gehen Sie dabei davon aus, dass eine alleine Eingabe die Zählrichtung
bestimmt und beim Vorwärtszählen nach einer $3$ eine $1$
ausgegeben wird, beim Rückwärtszählen nach einer $1$ eine $3$.",
            TaskImage = "TIUebung05_03.png",
            SolutionImage = "TIMoore_02_01.png",
            InputLabel = "Eingang",
            IncludeKvIntro = false,
            Transitions = new List<MooreTransitionRow>
            {
                new(0, 0, 0, 1, 0),
                new(0, 0, 1, 0, 1),
                new(0, 1, 0, 0, 0),
                new(0, 1, 1, 1, 0),
                new(1, 0, 0, 0, 1),
                new(1, 0, 1, 0, 0),
                new(1, 1, 0, -1, -1),
                new(1, 1, 1, -1, -1)
            },
            Outputs = new List<MooreOutputRow>
            {
                new(0, 0, 0, 1),
                new(0, 1, 1, 0),
                new(1, 0, 1, 1),
                new(1, 1, -1, -1)
            },
            NextZ0Minterms = new List<int> { 0, 5 },
            NextZ0DontCares = new List<int> { 3, 7 },
            NextZ0Implicants = new List<string> { @"\implicant{0}{0}", @"\implicant{5}{7}" },
            NextZ0Formula = @"(e\wedge z_{0}) \vee ( \neg e\wedge \neg z_{1}\wedge \neg z_{0})",
            NextZ1Minterms = new List<int> { 2, 4 },
            NextZ1DontCares = new List<int> { 3, 7 },
            NextZ1Implicants = new List<string> { @"\implicant{4}{4}", @"\implicant{3}{2}" },
            NextZ1Formula = @"(e\wedge\neg z_{0}\wedge\neg z_{1}) \vee (\neg e\wedge z_{0})",
            OutputKvVarX = @"$z_1$",
            OutputKvVarY = @"$z_0$",
            A0Minterms = new List<int> { 1, 2 },
            A0DontCares = new List<int> { 3 },
            A0Implicants = new List<string> { @"\implicant{1}{3}", @"\implicant{2}{3}" },
            A0Formula = @"z_0 \vee z_1",
            A1Minterms = new List<int> { 0, 2 },
            A1DontCares = new List<int> { 3 },
            A1Implicants = new List<string> { @"\implicant{0}{2}" },
            A1Formula = @"\neg z_1"
        };
    }

    private static MooreMachineModel CreateModel3()
    {
        return new MooreMachineModel
        {
            Id = 3,
            Name = "2-Bit Gray-Code Zähler (zyklisch)",
            TaskDescription = @"Modellieren Sie einen Automaten zum Zählen von 2-Bit-Zahlen gemäß Gray-Code-Reihenfolge (also $00$, $01$, $11$, $10$). Der Automat soll je nach Eingabe vorwärts oder rückwärts zählen. Gehen Sie dabei davon aus, dass eine Eingabe alleine die Zählrichtung bestimmt und zyklisch gezählt wird, d.h. nach $10$ kommt wieder $00$, vor $00$ kommt $10$.",
            TaskImage = "TIMoore_03_01.png",
            SolutionImage = "TIMoore_03_02.png",
            InputLabel = "Eingang",
            IncludeKvIntro = true,
            Transitions = new List<MooreTransitionRow>
            {
                new(0, 0, 0, 1, 1),
                new(0, 0, 1, 0, 1),
                new(0, 1, 0, 0, 0),
                new(0, 1, 1, 1, 0),
                new(1, 0, 0, 0, 1),
                new(1, 0, 1, 1, 1),
                new(1, 1, 0, 1, 0),
                new(1, 1, 1, 0, 0)
            },
            Outputs = new List<MooreOutputRow>
            {
                new(0, 0, 0, 0),
                new(0, 1, 0, 1),
                new(1, 0, 1, 1),
                new(1, 1, 1, 0)
            },
            NextZ0Minterms = new List<int> { 0, 3, 5, 6 },
            NextZ0DontCares = new List<int>(),
            NextZ0Implicants = new List<string>
            {
                @"\implicant{0}{0}",
                @"\implicant{3}{3}",
                @"\implicant{5}{5}",
                @"\implicant{6}{6}"
            },
            NextZ0Formula = @"(\neg e\wedge \neg z_{1} \wedge \neg z_{0}) \vee ( e\wedge \neg z_{1}\wedge \neg z_{0}) \vee ( \neg e\wedge z_{1}\wedge z_{0}) \vee ( e\wedge z_{1}\wedge \neg z_{0})",
            NextZ1Minterms = new List<int> { 0, 2, 4, 6 },
            NextZ1DontCares = new List<int>(),
            NextZ1Implicants = new List<string> { @"\implicantedge{0}{4}{2}{6}" },
            NextZ1Formula = @"\neg z_0",
            OutputKvVarX = @"$z_1$",
            OutputKvVarY = @"$z_0$",
            A0Minterms = new List<int> { 1, 3 },
            A0DontCares = new List<int>(),
            A0Implicants = new List<string> { @"\implicant{1}{3}" },
            A0Formula = @"z_1",
            A1Minterms = new List<int> { 1, 2 },
            A1DontCares = new List<int>(),
            A1Implicants = new List<string> { @"\implicant{1}{1}", @"\implicant{2}{2}" },
            A1Formula = @"(\neg z_1 \wedge z_0) \vee ( z_1 \wedge \neg z_0)"
        };
    }
}
