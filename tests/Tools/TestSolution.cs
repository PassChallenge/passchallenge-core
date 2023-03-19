using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Tests.Tools;

public class TestSolution : ISolution
{
    public TestSolution(SolutionResultType resultType)
    {
        ResultType = resultType;
    }

    public SolutionResultType ResultType { get; }
    
    public string? ErrorMessage { get; } = default;
}