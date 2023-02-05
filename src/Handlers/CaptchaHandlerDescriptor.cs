using System;
using System.Threading.Tasks;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Handlers;

public class CaptchaHandlerDescriptor
{
    private CaptchaHandlerDescriptor(Type captchaType, Type solutionType, Type handlerType,
        Func<IServiceProvider, object> factory) : this(captchaType, solutionType, handlerType)
    {
        ImplementationFactory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    private CaptchaHandlerDescriptor(Type captchaType, Type solutionType, Type handlerType)
    {
        CaptchaType = captchaType ?? throw new ArgumentNullException(nameof(captchaType));
        SolutionType = solutionType ?? throw new ArgumentNullException(nameof(solutionType));
        HandlerType = handlerType ?? throw new ArgumentNullException(nameof(handlerType));
    }

    private CaptchaHandlerDescriptor(Type captchaType, Type solutionType,
        Func<IServiceProvider, object, object> solverFunction)
    {
        CaptchaType = captchaType ?? throw new ArgumentNullException(nameof(captchaType));
        SolutionType = solutionType ?? throw new ArgumentNullException(nameof(solutionType));
        SolverFunction = solverFunction ?? throw new ArgumentNullException(nameof(solverFunction));
    }

    public Type CaptchaType { get; }

    public Type SolutionType { get; }

    public Type? HandlerType { get; }

    public Func<IServiceProvider, object>? ImplementationFactory { get; }

    public Func<IServiceProvider, object, object>? SolverFunction { get; }

    public static CaptchaHandlerDescriptor Create<TCaptcha, TSolution, THandler>()
        where TCaptcha : ICaptcha
        where TSolution : ISolution
        where THandler : ICaptchaHandler<TCaptcha, TSolution>
    {
        return new CaptchaHandlerDescriptor(typeof(TCaptcha), typeof(TSolution), typeof(THandler));
    }

    public static CaptchaHandlerDescriptor Create<TCaptcha, TSolution, THandler>(
        Func<IServiceProvider, THandler> factory)
        where TCaptcha : ICaptcha
        where TSolution : ISolution
        where THandler : ICaptchaHandler<TCaptcha, TSolution>
    {
        return new CaptchaHandlerDescriptor(typeof(TCaptcha), typeof(TSolution), typeof(THandler),
            provider => factory.Invoke(provider));
    }

    public static CaptchaHandlerDescriptor Create<TCaptcha, TSolution>(
        Func<IServiceProvider, TCaptcha, Task<TSolution>> func)
        where TCaptcha : ICaptcha
        where TSolution : ISolution
    {
        return new CaptchaHandlerDescriptor(typeof(TCaptcha), typeof(TSolution),
            (provider, captcha) => func.Invoke(provider, (TCaptcha)captcha));
    }

    public override string ToString()
    {
        string result = $"{CaptchaType}: {SolutionType}";

        if (SolverFunction != null)
        {
            return result + ". Has solver function";
        }

        if (HandlerType != null)
        {
            result += $", Handler: {HandlerType}";
        }
        
        if (ImplementationFactory != null)
        {
            return result + $", {nameof(ImplementationFactory)}: {ImplementationFactory.Method}";
        }

        return result;
    }
}