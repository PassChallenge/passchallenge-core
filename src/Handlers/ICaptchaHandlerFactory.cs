using System;
using System.Collections.Generic;
using PassChallenge.Core.Captcha;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Handlers;

public interface ICaptchaHandlerFactory
{
    string GetDefaultHandlerName<TCaptcha, TSolution>() where TCaptcha : ICaptcha where TSolution : ISolution;

    IReadOnlyCollection<string> GetHandlerNames<TCaptcha, TSolution>()
        where TCaptcha : ICaptcha where TSolution : ISolution;

    bool CanProduce<TCaptcha, TSolution>(string? handlerName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution;

    ICaptchaHandler<TCaptcha, TSolution> CreateHandler<TCaptcha, TSolution>(
        IServiceProvider serviceProvider, string? handlerName = default)
        where TCaptcha : ICaptcha
        where TSolution : ISolution;
}