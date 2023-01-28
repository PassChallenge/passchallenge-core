using System;

namespace KillDNS.CaptchaSolver.Core.Solutions;

public class TextSolution : ISolution
{
    public TextSolution(string? answer, SolutionResultType resultType)
    {
        ResultType = resultType;
        Answer = answer;
    }
    
    public string? Answer { get; }
    public SolutionResultType ResultType { get; }
}