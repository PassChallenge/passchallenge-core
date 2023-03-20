using PassChallenge.Core.Exceptions;
using Moq;
using NUnit.Framework;

namespace PassChallenge.Core.Tests.Exceptions;

public class ChallengeExceptionTests
{
    [Test]
    public void Constructor_Is_Correct()
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.DoesNotThrow(() => new ChallengeException());
    }

    [Test]
    public void Constructor_With_Message_Is_Correct()
    {
        string expectedMessage = "message";
        ChallengeException exception = new(expectedMessage);

        Assert.That(exception.Message, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void Constructor_With_Message_And_InnerException_Is_Correct()
    {
        string expectedMessage = "message";
        Exception expectedInnerException = new Mock<Exception>().Object;

        ChallengeException exception = new(expectedMessage, expectedInnerException);

        Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        Assert.That(exception.InnerException, Is.EqualTo(expectedInnerException));
    }
}