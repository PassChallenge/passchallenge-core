using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Solutions;
using Microsoft.Extensions.DependencyInjection;

namespace KillDNS.CaptchaSolver.Core.Handlers;

public class CaptchaHandlerFactory : ICaptchaHandlerFactory
{
    private readonly IReadOnlyDictionary<(Type captchaType, Type solutionType), Type> _handlerTypes;

    public CaptchaHandlerFactory(IReadOnlyDictionary<(Type captchaType, Type solutionType), Type> handlerTypes)
    {
        _handlerTypes = handlerTypes;
    }

    public ICaptchaHandler<TCaptcha, TSolution> CreateHandler<TCaptcha, TSolution>(IServiceProvider serviceProvider)
        where TCaptcha : ICaptcha
        where TSolution : ISolution
    {
        if (!_handlerTypes.TryGetValue((typeof(TCaptcha), typeof(TSolution)), out Type handlerType))
        {
            throw new InvalidOperationException(
                $"Can't find handler type '{handlerType}' for captcha type '{typeof(TCaptcha)}' and solution type '{typeof(TSolution)}'");
        }

        var parameters = handlerType.GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0]
            .GetParameters()
            .Select(x => serviceProvider.GetRequiredService(x.ParameterType)).ToArray();

        return (ICaptchaHandler<TCaptcha, TSolution>)Activator.CreateInstance(handlerType, parameters);
    }
}