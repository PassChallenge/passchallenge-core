namespace KillDNS.CaptchaSolver.Core.Solutions;

public class TextSolution : ISolution
{
    public TextSolution(string? answer, SolutionResultType resultType = SolutionResultType.Solved)
    {
        ResultType = resultType;
        Answer = answer;
    }

    public string? Answer { get; }
    public SolutionResultType ResultType { get; }
}