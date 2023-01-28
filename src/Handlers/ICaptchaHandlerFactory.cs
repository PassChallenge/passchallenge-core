using System;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Handlers;

public interface ICaptchaHandlerFactory
{
    public ICaptchaHandler<TCaptcha, TSolution> CreateHandler<TCaptcha, TSolution>(
        IServiceProvider serviceProvider) 
        where TCaptcha : ICaptcha
        where TSolution : ISolution;
}