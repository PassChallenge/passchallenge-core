using KillDNS.CaptchaSolver.Core.Solutions;
using NUnit.Framework;

namespace KillDNS.CaptchaSolver.Core.Tests.Solutions;

public class TextSolutionTests
{
    [Test]
    [TestCase(SolutionResultType.Canceled)]
    [TestCase(SolutionResultType.Renew)]
    [TestCase(SolutionResultType.Skipped)]
    [TestCase(SolutionResultType.Solved)]
    [TestCase(SolutionResultType.Error)]
    public void TextSolution_Constructor_Is_Correct(SolutionResultType solutionResultType)
    {
        string expectedValue = "test";
        TextSolution solution = new(expectedValue, solutionResultType);
        Assert.Multiple(() =>
        {
            Assert.That(solution.Answer, Is.EqualTo(expectedValue));
            Assert.That(solution.ResultType, Is.EqualTo(solutionResultType));
        });
    }

    [Test]
    public void ErrorSolution_Is_Correct()
    {
        string expectedErrorMessage = "test";
        TextSolution solution = TextSolution.ErrorSolution(expectedErrorMessage);
        Assert.Multiple(() =>
        {
            Assert.Null(solution.Answer);
            Assert.That(solution.ErrorMessage, Is.EqualTo(expectedErrorMessage));
            Assert.That(solution.ResultType, Is.EqualTo(SolutionResultType.Error));
        });
    }
}