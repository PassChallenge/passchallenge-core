using System;

namespace PassChallenge.Core.Solutions;

public class ReCaptchaSolution : ISolution
{
    public ReCaptchaSolution(string? response, SolutionResultType resultType = SolutionResultType.Solved)
    {
        ResultType = resultType;
        Response = response;
    }

    public string? Response { get; }

    public string? ErrorMessage { get; private set; }

    public SolutionResultType ResultType { get; }

    public static ReCaptchaSolution ErrorSolution(string errorMessage)
    {
        ReCaptchaSolution result = new(null, SolutionResultType.Error)
        {
            ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage))
        };

        return result;
    }

    public override string ToString()
    {
        return $"ResultType: {ResultType}, Response: {Response}, ErrorMessage: {ErrorMessage}";
    }
}