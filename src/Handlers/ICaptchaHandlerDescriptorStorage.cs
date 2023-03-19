using System.Collections.Generic;
using PassChallenge.Core.Captcha;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Handlers;

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