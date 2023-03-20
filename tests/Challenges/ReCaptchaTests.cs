using NUnit.Framework;
using PassChallenge.Core.Challenges;

namespace PassChallenge.Core.Tests.Challenges;

public class ReCaptchaTests
{
    [Test]
    public void Constructor_Is_Correct()
    {
        Uri expectedUrl = new("http://localhost");
        string expectedSiteKey = "site-key";
        ReCaptcha challenge = new(expectedUrl, expectedSiteKey);
        Assert.That(challenge.PageUrl, Is.EqualTo(expectedUrl));
        Assert.That(challenge.SiteKey, Is.EqualTo(expectedSiteKey));
    }
    [Test]
    public void Constructor_With_SiteKey_Is_Null_Is_Correct()
    {
        Uri expectedUrl = new("http://localhost");
        ReCaptcha challenge = new(expectedUrl);
        Assert.That(challenge.PageUrl, Is.EqualTo(expectedUrl));
        Assert.That(challenge.SiteKey, Is.Null);
    }
    
    [Test]
    public void Constructor_When_Url_Is_Null_Throws_ArgumentNullException()
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => new ReCaptcha(null!));
    }

    [Test]
    public void ToString_Is_Correct()
    {
        Uri expectedUrl = new("http://localhost");
        string expectedSiteKey = "site-key";
        
        string expectedValue = $"PageUrl: {expectedUrl}, SiteKey: {expectedSiteKey}";
        ReCaptcha challenge = new(expectedUrl, expectedSiteKey);
        Assert.That(challenge.ToString(), Is.EqualTo(expectedValue));
    }
}