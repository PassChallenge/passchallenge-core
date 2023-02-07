using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Tests.Tools;

public class TestSolution : ISolution
{
    public TestSolution(SolutionResultType resultType)
    {
        ResultType = resultType;
    }

    public SolutionResultType ResultType { get; }
}