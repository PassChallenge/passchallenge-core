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
    private readonly ICaptchaHandlerDescriptorStorage _captchaHandlerDescriptorStorage;

    public CaptchaHandlerFactory(ICaptchaHandlerDescriptorStorage captchaHandlerDescriptorStorage)
    {
        _captchaHandlerDescriptorStorage = captchaHandlerDescriptorStorage ??
                                           throw new ArgumentNullException(nameof(captchaHandlerDescriptorStorage));
    }

    public string GetDefaultHandlerName<TCaptcha, TSolution>() where TCaptcha : ICaptcha where TSolution : ISolution
    {
        return _captchaHandlerDescriptorStorage.GetDefaultDescriptorName<TCaptcha, TSolution>();
    }

    public IReadOnlyCollection<string> GetHandlerNames<TCaptcha, TSolution>()
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        return _captchaHandlerDescriptorStorage.GetDescriptors<TCaptcha, TSolution>().Select(x => x.HandlerName!)
            .ToList();
    }

    public bool CanProduce<TCaptcha, TSolution>(string? handlerName = default)
        where TCaptcha : ICaptcha where TSolution : ISolution
    {
        return _captchaHandlerDescriptorStorage.ContainsDescriptor<TCaptcha, TSolution>(handlerName);
    }

    public ICaptchaHandler<TCaptcha, TSolution> CreateHandler<TCaptcha, TSolution>(IServiceProvider serviceProvider,
        string? handlerName = default)
        where TCaptcha : ICaptcha
        where TSolution : ISolution
    {
        if (serviceProvider == null)
            throw new ArgumentNullException(nameof(serviceProvider));

        CaptchaHandlerDescriptor handlerDescriptor =
            _captchaHandlerDescriptorStorage.GetDescriptor<TCaptcha, TSolution>(handlerName);

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

        //if handler class
        var parameters = handlerDescriptor.HandlerType!.GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0]
            .GetParameters()
            .Select(x => serviceProvider.GetRequiredService(x.ParameterType)).ToArray();

        return (ICaptchaHandler<TCaptcha, TSolution>)Activator.CreateInstance(handlerDescriptor.HandlerType,
            parameters);
    }
}