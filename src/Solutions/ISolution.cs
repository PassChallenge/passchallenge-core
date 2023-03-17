namespace KillDNS.CaptchaSolver.Core.Solutions;

public interface ISolution
{
    SolutionResultType ResultType { get; }
    
    string? ErrorMessage { get; }
}