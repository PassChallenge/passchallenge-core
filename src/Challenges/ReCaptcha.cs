using System;

namespace PassChallenge.Core.Challenges;

public class ReChallenge : IChallenge
{
    public ReChallenge(Uri pageUrl, string? siteKey = default)
    {
        PageUrl = pageUrl ?? throw new ArgumentNullException(nameof(pageUrl));
        SiteKey = siteKey;
    }

    public Uri PageUrl { get; }

    public string? SiteKey { get; }
}