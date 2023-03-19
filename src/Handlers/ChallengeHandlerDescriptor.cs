using System;
using System.Threading.Tasks;
using PassChallenge.Core.Challenges;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Handlers;

public class ChallengeHandlerDescriptor
{
    private ChallengeHandlerDescriptor(Type challengeType, Type solutionType, Type handlerType,
        Func<IServiceProvider, object> factory, string? handlerName = default) : this(challengeType, solutionType,
        handlerName)
    {
        ImplementationFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        HandlerType = handlerType ?? throw new ArgumentNullException(nameof(handlerType));
    }

    private ChallengeHandlerDescriptor(Type challengeType, Type solutionType, Type handlerType,
        string? handlerName = default) : this(challengeType, solutionType, handlerName)
    {
        if (handlerType == null)
            throw new ArgumentNullException(nameof(handlerType));
        
        if (handlerType.IsInterface)
            throw new ArgumentException("The handler must be a class.");

        HandlerType = handlerType;
    }

    private ChallengeHandlerDescriptor(Type challengeType, Type solutionType,
        Func<IServiceProvider, object, object> solverFunction, string? handlerName = default) : this(challengeType,
        solutionType, handlerName)
    {
        SolverFunction = solverFunction ?? throw new ArgumentNullException(nameof(solverFunction));
    }

    private ChallengeHandlerDescriptor(Type challengeType, Type solutionType, string? handlerName = default)
    {
        ChallengeType = challengeType ?? throw new ArgumentNullException(nameof(challengeType));
        SolutionType = solutionType ?? throw new ArgumentNullException(nameof(solutionType));

        HandlerName = ValidateHandlerName(handlerName)
            ? handlerName
            : throw new ArgumentException("Handler name is whitespace.", nameof(handlerName));
    }

    public string? HandlerName { get; }

    public Type ChallengeType { get; }

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

    public ChallengeHandlerDescriptor CloneWithNewName(string handlerName)
    {
        if (string.IsNullOrWhiteSpace(handlerName))
            throw new ArgumentException("Handler name is null or whitespace.", nameof(handlerName));

        return new ChallengeHandlerDescriptor(ChallengeType, SolutionType, handlerName)
        {
            HandlerType = HandlerType,
            ImplementationFactory = ImplementationFactory,
            SolverFunction = SolverFunction
        };
    }

    public static ChallengeHandlerDescriptor Create<TChallenge, TSolution, THandler>(string? handlerName = default)
        where TChallenge : IChallenge
        where TSolution : ISolution
        where THandler : IChallengeHandler<TChallenge, TSolution>
    {
        return new ChallengeHandlerDescriptor(typeof(TChallenge), typeof(TSolution), typeof(THandler), handlerName);
    }

    public static ChallengeHandlerDescriptor Create<TChallenge, TSolution, THandler>(
        Func<IServiceProvider, THandler> factory, string? handlerName = default)
        where TChallenge : IChallenge
        where TSolution : ISolution
        where THandler : IChallengeHandler<TChallenge, TSolution>
    {
        return new ChallengeHandlerDescriptor(typeof(TChallenge), typeof(TSolution), typeof(THandler),
            provider => factory.Invoke(provider), handlerName);
    }

    public static ChallengeHandlerDescriptor Create<TChallenge, TSolution>(
        Func<IServiceProvider, TChallenge, Task<TSolution>> func, string? handlerName = default)
        where TChallenge : IChallenge
        where TSolution : ISolution
    {
        return new ChallengeHandlerDescriptor(typeof(TChallenge), typeof(TSolution),
            (provider, challenge) => func.Invoke(provider, (TChallenge)challenge), handlerName);
    }

    public override string ToString()
    {
        string result = $"{ChallengeType}: {SolutionType}";

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