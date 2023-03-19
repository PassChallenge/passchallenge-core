using System;
using System.Collections.Generic;
using PassChallenge.Core.Challenges;
using PassChallenge.Core.Handlers;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Solver;

public class AvailableChallengeAndSolutionStorageBuilder
{
    private readonly Dictionary<Type, Dictionary<Type, HashSet<string>>>
        _availableChallengeAndSolutionTypes = new();

    private IAvailableChallengeAndSolutionStorage? _availableChallengeAndSolutionStorage;

    public AvailableChallengeAndSolutionStorageBuilder AddSupportChallengeAndSolution<TChallenge, TSolution>(
        string? handlerName = default)
        where TChallenge : IChallenge where TSolution : ISolution
    {
        AddSupportChallengeAndSolution(typeof(TChallenge), typeof(TSolution), handlerName);
        return this;
    }

    public AvailableChallengeAndSolutionStorageBuilder AddSupportChallengeAndSolution(ChallengeHandlerDescriptor descriptor)
    {
        if (descriptor == null)
            throw new ArgumentNullException(nameof(descriptor));

        AddSupportChallengeAndSolution(descriptor.ChallengeType, descriptor.SolutionType, descriptor.HandlerName);
        return this;
    }

    public void SetStorage(IAvailableChallengeAndSolutionStorage storage)
    {
        _availableChallengeAndSolutionStorage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    public IAvailableChallengeAndSolutionStorage Build()
    {
        return _availableChallengeAndSolutionStorage ??
               new AvailableChallengeAndSolutionStorage(_availableChallengeAndSolutionTypes);
    }

    private void AddSupportChallengeAndSolution(Type challengeType, Type solutionType,
        string? handlerName = default)
    {
        if (!_availableChallengeAndSolutionTypes.ContainsKey(challengeType))
        {
            _availableChallengeAndSolutionTypes.Add(challengeType, new Dictionary<Type, HashSet<string>>());
            _availableChallengeAndSolutionTypes[challengeType].Add(solutionType, new HashSet<string>());
        }

        if (handlerName == default)
            return;

        if (_availableChallengeAndSolutionTypes[challengeType][solutionType].Contains(handlerName))
            throw new InvalidOperationException(
                $"Challenge '{challengeType}' and solution '{solutionType}' with handler name '{handlerName}' already added.");

        _availableChallengeAndSolutionTypes[challengeType][solutionType].Add(handlerName);
    }
}