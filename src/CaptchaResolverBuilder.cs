using System;
using System.Collections.Generic;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Consumer;
using KillDNS.CaptchaSolver.Core.Handlers;
using KillDNS.CaptchaSolver.Core.Solutions;
using Microsoft.Extensions.DependencyInjection;

namespace KillDNS.CaptchaSolver.Core;

public class CaptchaResolverBuilder : IBuilderWithCaptchaHandlers
{

    public CaptchaResolverBuilder(IServiceCollection serviceCollection)
    {
        ServiceCollection = serviceCollection;
        MessageHandlers = new List<Type>();
    }
    public IServiceCollection ServiceCollection { get; }
    
    internal List<Type> MessageHandlers { get; }

    internal Type? ConsumerType { get; private set; }
    
    internal Func<IServiceProvider, IConsumer>? Factory { get; private set; }

    public IBuilderWithCaptchaHandlers AddCaptchaHandler<TCaptcha, TSolution, THandler>()
        where TCaptcha : ICaptcha
        where TSolution : ISolution
        where THandler : ICaptchaHandler<TCaptcha, TSolution>
    {
        MessageHandlers.Add(typeof(THandler));
        return this;
    }

    public CaptchaResolverBuilder SetConsumer(Func<IServiceProvider, IConsumer> factory)
    {
        Factory = factory;
        return this;
    }

    public CaptchaResolverBuilder SetConsumer<TConsumer>() where TConsumer : IConsumer
    {
        ConsumerType = typeof(TConsumer);
        return this;
    }
}