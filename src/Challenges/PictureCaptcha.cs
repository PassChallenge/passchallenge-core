using System;

namespace PassChallenge.Core.Challenges;

public class PictureCaptcha : IChallenge
{
    public PictureCaptcha(byte[] imageData)
    {
        ImageData = imageData ?? throw new ArgumentNullException(nameof(imageData));
    }

    public byte[] ImageData { get; }

    public override string ToString()
    {
        return Convert.ToBase64String(ImageData);
    }
}