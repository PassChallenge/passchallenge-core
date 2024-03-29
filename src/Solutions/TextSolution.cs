using System;

namespace PassChallenge.Core.Solutions;

public class TextSolution : ISolution
{
    public TextSolution(string? answer, SolutionResultType resultType = SolutionResultType.Solved)
    {
        ResultType = resultType;
        Answer = answer;
    }

    public string? Answer { get; }

    public string? ErrorMessage { get; private set; }

    public SolutionResultType ResultType { get; }

    public static TextSolution ErrorSolution(string errorMessage)
    {
        TextSolution result = new(null, SolutionResultType.Error)
        {
            ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage))
        };

        return result;
    }

    public override string ToString()
    {
        return $"ResultType: {ResultType}, Answer: {Answer}, ErrorMessage: {ErrorMessage}";
    }
}