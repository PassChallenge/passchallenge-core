using System;
using System.Collections.Generic;
using System.Linq;
using PassChallenge.Core.Challenges;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Handlers;

internal class ChallengeHandlerDescriptorStorage : IChallengeHandlerDescriptorAvailableStorage
{
    private readonly Dictionary<(Type ChallengeType, Type solutionType), Dictionary<string, ChallengeHandlerDescriptor>>
        _descriptors;

    public ChallengeHandlerDescriptorStorage(
        Dictionary<(Type ChallengeType, Type solutionType), Dictionary<string, ChallengeHandlerDescriptor>> descriptors)
    {
        _descriptors = descriptors ?? throw new ArgumentNullException(nameof(descriptors));
        Descriptors = _descriptors.SelectMany(x => x.Value).Select(x => x.Value).ToList();
    }

    public IReadOnlyCollection<ChallengeHandlerDescriptor> Descriptors { get; }


    public bool ContainsDescriptor<TChallenge, TSolution>(string? descriptorName = default)
        where TChallenge : IChallenge where TSolution : ISolution
    {
        if (_descriptors.TryGetValue((typeof(TChallenge), typeof(TSolution)), out var namedHandlers) == false)
            return false;

        return descriptorName == default ? namedHandlers.Count > 0 : namedHandlers.ContainsKey(descriptorName);
    }

    public string GetDefaultDescriptorName<TChallenge, TSolution>() where TChallenge : IChallenge where TSolution : ISolution
    {
        if (ContainsDescriptor<TChallenge, TSolution>() == false)
            throw new InvalidOperationException(
                $"Can't find descriptor for challenge type '{typeof(TChallenge)}' and solution type '{typeof(TSolution)}'.");

        return _descriptors[(typeof(TChallenge), typeof(TSolution))].First().Key;
    }

    public IReadOnlyCollection<ChallengeHandlerDescriptor> GetDescriptors<TChallenge, TSolution>()
        where TChallenge : IChallenge where TSolution : ISolution
    {
        if (ContainsDescriptor<TChallenge, TSolution>() == false)
            throw new InvalidOperationException(
                $"Can't find descriptor for challenge type '{typeof(TChallenge)}' and solution type '{typeof(TSolution)}'.");

        return _descriptors[(typeof(TChallenge), typeof(TSolution))].Values;
    }

    public ChallengeHandlerDescriptor GetDescriptor<TChallenge, TSolution>(string? descriptorName = default)
        where TChallenge : IChallenge where TSolution : ISolution
    {
        if (ContainsDescriptor<TChallenge, TSolution>(descriptorName))
            return descriptorName == default
                ? _descriptors[(typeof(TChallenge), typeof(TSolution))].Values.First()
                : _descriptors[(typeof(TChallenge), typeof(TSolution))][descriptorName];

        throw new InvalidOperationException(
            $"Can't find descriptor for challenge type '{typeof(TChallenge)}' and solution type '{typeof(TSolution)}'" +
            (descriptorName == default ? "." : $" with name '{descriptorName}'."));
    }

    public bool IsAvailable<TChallenge, TSolution>(string? handlerName = default)
        where TChallenge : IChallenge where TSolution : ISolution
    {
        return ContainsDescriptor<TChallenge, TSolution>(handlerName);
    }
}