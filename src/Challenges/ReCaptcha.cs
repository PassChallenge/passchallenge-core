using System;

namespace PassChallenge.Core.Challenges;

public class ReCaptcha : IChallenge
{
    public ReCaptcha(Uri pageUrl, string? siteKey = default)
    {
        PageUrl = pageUrl ?? throw new ArgumentNullException(nameof(pageUrl));
        SiteKey = siteKey;
    }

    public Uri PageUrl { get; }

    public string? SiteKey { get; }

    public override string ToString()
    {
        return $"PageUrl: {PageUrl}, SiteKey: {SiteKey}";
    }
}