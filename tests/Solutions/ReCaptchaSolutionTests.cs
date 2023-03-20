using NUnit.Framework;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Tests.Solutions;

public class ReCaptchaSolutionTests
{
    [Test]
    [TestCase(SolutionResultType.Canceled)]
    [TestCase(SolutionResultType.Renew)]
    [TestCase(SolutionResultType.Skipped)]
    [TestCase(SolutionResultType.Solved)]
    [TestCase(SolutionResultType.Error)]
    public void Constructor_Is_Correct(SolutionResultType solutionResultType)
    {
        string expectedValue = "test";
        ReCaptchaSolution solution = new(expectedValue, solutionResultType);
        Assert.Multiple(() =>
        {
            Assert.That(solution.Response, Is.EqualTo(expectedValue));
            Assert.That(solution.ResultType, Is.EqualTo(solutionResultType));
        });
    }

    [Test]
    public void ErrorSolution_Is_Correct()
    {
        string expectedErrorMessage = "test";
        ReCaptchaSolution solution = ReCaptchaSolution.ErrorSolution(expectedErrorMessage);
        Assert.Multiple(() =>
        {
            Assert.Null(solution.Response);
            Assert.That(solution.ErrorMessage, Is.EqualTo(expectedErrorMessage));
            Assert.That(solution.ResultType, Is.EqualTo(SolutionResultType.Error));
        });
    }

    [Test]
    public void ToString_Not_Error_Is_Correct()
    {
        ReCaptchaSolution solution = new("answer", SolutionResultType.Solved);
        string expectedString = "ResultType: Solved, Response: answer, ErrorMessage: ";
        Assert.That(solution.ToString(), Is.EqualTo(expectedString));
    }

    [Test]
    public void ToString_Error_Is_Correct()
    {
        ReCaptchaSolution solution = ReCaptchaSolution.ErrorSolution("Wrong data");
        string expectedString = "ResultType: Error, Response: , ErrorMessage: Wrong data";
        Assert.That(solution.ToString(), Is.EqualTo(expectedString));
    }
}