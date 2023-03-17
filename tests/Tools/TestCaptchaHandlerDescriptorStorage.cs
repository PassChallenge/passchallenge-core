using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Handlers;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Tests.Tools;

public class TestCaptchaHandlerDescriptorStorage : ICaptchaHandlerDescriptorStorage
{
    public IReadOnlyCollection<CaptchaHandlerDescriptor> Descriptors { get; }

    public bool ContainsDescriptor<TCaptcha, TSolution>(string? descriptorName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        throw new NotImplementedException();
    }

    public string GetDefaultDescriptorName<TCaptcha, TSolution>() where TCaptcha : ICaptcha where TSolution : ISolution
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<CaptchaHandlerDescriptor> GetDescriptors<TCaptcha, TSolution>()
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        throw new NotImplementedException();
    }

    public CaptchaHandlerDescriptor GetDescriptor<TCaptcha, TSolution>(string? descriptorName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        throw new NotImplementedException();
    }
}