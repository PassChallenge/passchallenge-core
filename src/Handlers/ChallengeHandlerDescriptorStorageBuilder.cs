using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PassChallenge.Core.Challenges;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Handlers;

public class ChallengeHandlerDescriptorStorageBuilder
{
    private readonly Dictionary<(Type ChallengeType, Type solutionType), Dictionary<string, ChallengeHandlerDescriptor>>
        _descriptors = new();

    public ChallengeHandlerDescriptorStorageBuilder AddChallengeHandler<TChallenge, TSolution, THandler>(
        string? handlerName = default)
        where TChallenge : IChallenge
        where TSolution : ISolution
        where THandler : IChallengeHandler<TChallenge, TSolution>
    {
        if (typeof(THandler).IsInterface)
            throw new ArgumentException("The handler must be a class.");

        AddHandlerDescriptor(ChallengeHandlerDescriptor.Create<TChallenge, TSolution, THandler>(handlerName));
        return this;
    }

    public ChallengeHandlerDescriptorStorageBuilder AddChallengeHandler<TChallenge, TSolution, THandler>(
        Func<IServiceProvider, THandler> factory,
        string? handlerName = default) where TChallenge : IChallenge
        where TSolution : ISolution
        where THandler : IChallengeHandler<TChallenge, TSolution>
    {
        AddHandlerDescriptor(ChallengeHandlerDescriptor.Create<TChallenge, TSolution, THandler>(factory, handlerName));
        return this;
    }

    public ChallengeHandlerDescriptorStorageBuilder AddChallengeHandler<TChallenge, TSolution>(
        Func<IServiceProvider, TChallenge, Task<TSolution>> func,
        string? handlerName = default)
        where TChallenge : IChallenge where TSolution : ISolution
    {
        AddHandlerDescriptor(ChallengeHandlerDescriptor.Create(func, handlerName));
        return this;
    }

    public IChallengeHandlerDescriptorAvailableStorage Build()
    {
        return new ChallengeHandlerDescriptorStorage(_descriptors);
    }

    private void AddHandlerDescriptor(ChallengeHandlerDescriptor descriptor)
    {
        if (_descriptors.ContainsKey((descriptor.ChallengeType, descriptor.SolutionType)) == false)
            _descriptors.Add((descriptor.ChallengeType, descriptor.SolutionType),
                new Dictionary<string, ChallengeHandlerDescriptor>());

        Dictionary<string, ChallengeHandlerDescriptor> namedDescriptors =
            _descriptors[(descriptor.ChallengeType, descriptor.SolutionType)];

        ChallengeHandlerDescriptor newDescriptor = descriptor.HandlerName != default
            ? descriptor
            : descriptor.CloneWithNewName(GetNewHandlerIdentifier(namedDescriptors.Keys, descriptor));

        if (namedDescriptors.ContainsKey(newDescriptor.HandlerName!))
            throw new InvalidOperationException(
                $"Descriptor with challenge '{newDescriptor.ChallengeType}' and solution '{newDescriptor.SolutionType}' and handler name '{newDescriptor.HandlerName}' already added.");

        namedDescriptors.Add(newDescriptor.HandlerName!, newDescriptor);
    }

    private string GetNewHandlerIdentifier(IReadOnlyCollection<string> allHandlerNames,
        ChallengeHandlerDescriptor descriptor)
    {
        string baseHandlerName = $"{descriptor.ChallengeType.Name}-{descriptor.SolutionType.Name}-".ToLower();
        for (int i = 0;; i++)
        {
            string finalHandlerName = baseHandlerName + i;

            if (allHandlerNames.Contains(finalHandlerName) == false)
                return finalHandlerName;
        }
    }
}