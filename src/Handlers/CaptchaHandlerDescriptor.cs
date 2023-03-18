using System;
using System.Threading.Tasks;
using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Solutions;

namespace KillDNS.CaptchaSolver.Core.Handlers;

public class CaptchaHandlerDescriptor
{
    private CaptchaHandlerDescriptor(Type captchaType, Type solutionType, Type handlerType,
        Func<IServiceProvider, object> factory, string? handlerName = default) : this(captchaType, solutionType,
        handlerName)
    {
        ImplementationFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        HandlerType = handlerType ?? throw new ArgumentNullException(nameof(handlerType));
    }

    private CaptchaHandlerDescriptor(Type captchaType, Type solutionType, Type handlerType,
        string? handlerName = default) : this(captchaType, solutionType, handlerName)
    {
        if (handlerType.IsInterface)
            throw new ArgumentException("The handler must be a class.");

        HandlerType = handlerType ?? throw new ArgumentNullException(nameof(handlerType));
    }

    private CaptchaHandlerDescriptor(Type captchaType, Type solutionType,
        Func<IServiceProvider, object, object> solverFunction, string? handlerName = default) : this(captchaType,
        solutionType, handlerName)
    {
        SolverFunction = solverFunction ?? throw new ArgumentNullException(nameof(solverFunction));
    }

    private CaptchaHandlerDescriptor(Type captchaType, Type solutionType, string? handlerName = default)
    {
        CaptchaType = captchaType ?? throw new ArgumentNullException(nameof(captchaType));
        SolutionType = solutionType ?? throw new ArgumentNullException(nameof(solutionType));

        HandlerName = ValidateHandlerName(handlerName)
            ? handlerName
            : throw new ArgumentException("Handler name is whitespace.", nameof(handlerName));
    }

    public string? HandlerName { get; }

    public Type CaptchaType { get; }

    public Type SolutionType { get; }

    public Type? HandlerType { get; private set; }

    public Func<IServiceProvider, object>? ImplementationFactory { get; private set; }

    public Func<IServiceProvider, object, object>? SolverFunction { get; private set; }

    private bool ValidateHandlerName(string? handlerName)
    {
        if (handlerName == default)
            return true;

        return string.IsNullOrWhiteSpace(handlerName) == false;
    }

    public CaptchaHandlerDescriptor CloneWithNewName(string handlerName)
    {
        if (string.IsNullOrWhiteSpace(handlerName))
            throw new ArgumentException("Handler name is null or whitespace.", nameof(handlerName));

        return new CaptchaHandlerDescriptor(CaptchaType, SolutionType, handlerName)
        {
            HandlerType = HandlerType,
            ImplementationFactory = ImplementationFactory,
            SolverFunction = SolverFunction
        };
    }

    public static CaptchaHandlerDescriptor Create<TCaptcha, TSolution, THandler>(string? handlerName = default)
        where TCaptcha : ICaptcha
        where TSolution : ISolution
        where THandler : ICaptchaHandler<TCaptcha, TSolution>
    {
        return new CaptchaHandlerDescriptor(typeof(TCaptcha), typeof(TSolution), typeof(THandler), handlerName);
    }

    public static CaptchaHandlerDescriptor Create<TCaptcha, TSolution, THandler>(
        Func<IServiceProvider, THandler> factory, string? handlerName = default)
        where TCaptcha : ICaptcha
        where TSolution : ISolution
        where THandler : ICaptchaHandler<TCaptcha, TSolution>
    {
        return new CaptchaHandlerDescriptor(typeof(TCaptcha), typeof(TSolution), typeof(THandler),
            provider => factory.Invoke(provider), handlerName);
    }

    public static CaptchaHandlerDescriptor Create<TCaptcha, TSolution>(
        Func<IServiceProvider, TCaptcha, Task<TSolution>> func, string? handlerName = default)
        where TCaptcha : ICaptcha
        where TSolution : ISolution
    {
        return new CaptchaHandlerDescriptor(typeof(TCaptcha), typeof(TSolution),
            (provider, captcha) => func.Invoke(provider, (TCaptcha)captcha), handlerName);
    }

    public override string ToString()
    {
        string result = $"{CaptchaType}: {SolutionType}";

        if (SolverFunction != null)
        {
            return result + ". Has handler function.";
        }

        if (HandlerType != null)
        {
            result += $", Handler: {HandlerType}";
        }

        if (ImplementationFactory != null)
        {
            return result + ", has implementation factory";
        }

        return result;
    }
}