using System.Collections.Generic;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Handlers;

public interface ICaptchaHandlerDescriptorStorage
{
    IReadOnlyCollection<CaptchaHandlerDescriptor> Descriptors { get; }

    bool ContainsDescriptor<TCaptcha, TSolution>(string? descriptorName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution;

    string GetDefaultDescriptorName<TCaptcha, TSolution>() where TCaptcha : ICaptcha where TSolution : ISolution;

    IReadOnlyCollection<CaptchaHandlerDescriptor> GetDescriptors<TCaptcha, TSolution>()
        where TCaptcha : ICaptcha where TSolution : ISolution;

    CaptchaHandlerDescriptor GetDescriptor<TCaptcha, TSolution>(string? descriptorName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution;
}