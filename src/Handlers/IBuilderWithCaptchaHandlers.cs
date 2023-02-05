using System;
using System.Threading.Tasks;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Handlers;

public interface IBuilderWithCaptchaHandlers
{
    IBuilderWithCaptchaHandlers AddCaptchaHandler<TCaptcha, TSolution, THandler>()
        where TCaptcha : ICaptcha
        where TSolution : ISolution
        where THandler : ICaptchaHandler<TCaptcha, TSolution>;

    IBuilderWithCaptchaHandlers AddCaptchaHandler<TCaptcha, TSolution, THandler>(
        Func<IServiceProvider, THandler> factory)
        where TCaptcha : ICaptcha
        where TSolution : ISolution
        where THandler : ICaptchaHandler<TCaptcha, TSolution>;

    IBuilderWithCaptchaHandlers AddCaptchaHandler<TCaptcha, TSolution>(
        Func<IServiceProvider, TCaptcha, Task<TSolution>> func)
        where TCaptcha : ICaptcha
        where TSolution : ISolution;
}