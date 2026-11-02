namespace Italbytz.ComputingSystems.Abstractions;

public interface IEspressoStep
{
    string Phase { get; set; }
    int Iteration { get; set; }
    string Title { get; set; }
    string Description { get; set; }
    string[] CurrentCover { get; set; }
    string ActionDetail { get; set; }
}

