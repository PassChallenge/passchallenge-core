using NUnit.Framework;
using PassChallenge.Core.Challenges;

namespace PassChallenge.Core.Tests.Challenges;

public class PictureCaptchaTests
{
    [Test]
    public void Constructor_Is_Correct()
    {
        byte[] expectedValue = Array.Empty<byte>();
        PictureCaptcha Challenge = new(expectedValue);
        Assert.That(Challenge.ImageData, Is.EqualTo(expectedValue));
    }

    [Test]
    public void Constructor_When_Data_Is_Null_Throws_ArgumentNullException()
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => new PictureCaptcha(null!));
    }

    [Test]
    [TestCase(new byte[] { 0, 3, 4, 6, 1, 8, 22 }, "AAMEBgEIFg==")]
    [TestCase(new byte[] { }, "")]
    public void ToString_Is_Correct(byte[] array, string expectedString)
    {
        string expectedValue = Convert.ToBase64String(array);
        PictureCaptcha challenge = new(array);
        Assert.That(challenge.ToString(), Is.EqualTo(expectedValue));
    }
}