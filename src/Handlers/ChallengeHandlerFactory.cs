using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PassChallenge.Core.Challenges;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Handlers;

public class ChallengeHandlerFactory : IChallengeHandlerFactory
{
    private readonly IChallengeHandlerDescriptorStorage _challengeHandlerDescriptorStorage;

    public ChallengeHandlerFactory(IChallengeHandlerDescriptorStorage challengeHandlerDescriptorStorage)
    {
        _challengeHandlerDescriptorStorage = challengeHandlerDescriptorStorage ??
                                           throw new ArgumentNullException(nameof(challengeHandlerDescriptorStorage));
    }

    public string GetDefaultHandlerName<TChallenge, TSolution>() where TChallenge : IChallenge where TSolution : ISolution
    {
        return _challengeHandlerDescriptorStorage.GetDefaultDescriptorName<TChallenge, TSolution>();
    }

    public IReadOnlyCollection<string> GetHandlerNames<TChallenge, TSolution>()
        where TChallenge : IChallenge where TSolution : ISolution
    {
        return _challengeHandlerDescriptorStorage.GetDescriptors<TChallenge, TSolution>().Select(x => x.HandlerName!)
            .ToList();
    }

    public bool CanProduce<TChallenge, TSolution>(string? handlerName = default)
        where TChallenge : IChallenge where TSolution : ISolution
    {
        return _challengeHandlerDescriptorStorage.ContainsDescriptor<TChallenge, TSolution>(handlerName);
    }

    public IChallengeHandler<TChallenge, TSolution> CreateHandler<TChallenge, TSolution>(IServiceProvider serviceProvider,
        string? handlerName = default)
        where TChallenge : IChallenge
        where TSolution : ISolution
    {
        if (serviceProvider == null)
            throw new ArgumentNullException(nameof(serviceProvider));

        ChallengeHandlerDescriptor handlerDescriptor =
            _challengeHandlerDescriptorStorage.GetDescriptor<TChallenge, TSolution>(handlerName);

        //if using handler function
        if (handlerDescriptor.SolverFunction != null)
        {
            return new FixedChallengeHandler<TChallenge, TSolution>(serviceProvider,
                (provider, challenge) => (Task<TSolution>)handlerDescriptor.SolverFunction.Invoke(provider, challenge));
        }

        //if using handler creator function
        if (handlerDescriptor.ImplementationFactory != null)
        {
            return (IChallengeHandler<TChallenge, TSolution>)handlerDescriptor.ImplementationFactory
                .Invoke(serviceProvider);
        }

        //if handler class
        var parameters = handlerDescriptor.HandlerType!.GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0]
            .GetParameters()
            .Select(x => serviceProvider.GetRequiredService(x.ParameterType)).ToArray();

        return (IChallengeHandler<TChallenge, TSolution>)Activator.CreateInstance(handlerDescriptor.HandlerType,
            parameters);
    }
}