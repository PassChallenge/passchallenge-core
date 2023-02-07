using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Solutions;
using Microsoft.Extensions.DependencyInjection;

namespace KillDNS.CaptchaSolver.Core.Handlers;

public class CaptchaHandlerFactory : ICaptchaHandlerFactory
{
    private readonly IReadOnlyDictionary<(Type captchaType, Type solutionType), CaptchaHandlerDescriptor> _handlerTypes;

    public CaptchaHandlerFactory(IEnumerable<CaptchaHandlerDescriptor> handlers)
    {
        if (handlers == null)
            throw new ArgumentNullException(nameof(handlers));

        _handlerTypes = handlers.ToDictionary(x => (x.CaptchaType, x.SolutionType), x => x);
    }

    public ICaptchaHandler<TCaptcha, TSolution> CreateHandler<TCaptcha, TSolution>(IServiceProvider serviceProvider)
        where TCaptcha : ICaptcha
        where TSolution : ISolution
    {
        if (serviceProvider == null)
            throw new ArgumentNullException(nameof(serviceProvider));

        if (!_handlerTypes.TryGetValue((typeof(TCaptcha), typeof(TSolution)),
                out CaptchaHandlerDescriptor handlerDescriptor))
        {
            throw new InvalidOperationException(
                $"Can't find handler descriptor for captcha type '{typeof(TCaptcha)}' and solution type '{typeof(TSolution)}'");
        }

        //if using handler function
        if (handlerDescriptor.SolverFunction != null)
        {
            return new FixedCaptchaHandler<TCaptcha, TSolution>(serviceProvider,
                (provider, captcha) => (Task<TSolution>)handlerDescriptor.SolverFunction.Invoke(provider, captcha));
        }

        //if using handler creator function
        if (handlerDescriptor.ImplementationFactory != null)
        {
            return (ICaptchaHandler<TCaptcha, TSolution>)handlerDescriptor.ImplementationFactory
                .Invoke(serviceProvider);
        }

        //if handler class is specified by default 
        var parameters = handlerDescriptor.HandlerType!.GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0]
            .GetParameters()
            .Select(x => serviceProvider.GetRequiredService(x.ParameterType)).ToArray();

        return (ICaptchaHandler<TCaptcha, TSolution>)Activator.CreateInstance(handlerDescriptor.HandlerType,
            parameters);
    }
}