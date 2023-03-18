using System;

namespace KillDNS.CaptchaSolver.Core.Captcha;

public class PictureCaptcha : ICaptcha
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