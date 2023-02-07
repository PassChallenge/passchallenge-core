using KillDNS.CaptchaSolver.Core.Captcha;
using NUnit.Framework;

namespace KillDNS.CaptchaSolver.Core.Tests.Captcha;

public class PictureCaptchaTests
{
    [Test]
    public void PictureCaptchaTests_Constructor_Is_Correct()
    {
        byte[] expectedValue = Array.Empty<byte>();
        PictureCaptcha captcha = new(expectedValue);
        Assert.That(captcha.ImageData, Is.EqualTo(expectedValue));
    }

    [Test]
    public void PictureCaptchaTests_Constructor_When_Data_Is_Null_Throws_ArgumentNullException()
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => new PictureCaptcha(null!));
    }
}