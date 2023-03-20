using System.Collections.Generic;
using PassChallenge.Core.Challenges;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Handlers;

public interface IChallengeHandlerDescriptorStorage
{
    IReadOnlyCollection<ChallengeHandlerDescriptor> Descriptors { get; }

    bool ContainsDescriptor<TChallenge, TSolution>(string? descriptorName = default)
        where TChallenge : IChallenge where TSolution : ISolution;

    string GetDefaultDescriptorName<TChallenge, TSolution>() where TChallenge : IChallenge where TSolution : ISolution;

    IReadOnlyCollection<ChallengeHandlerDescriptor> GetDescriptors<TChallenge, TSolution>()
        where TChallenge : IChallenge where TSolution : ISolution;

    ChallengeHandlerDescriptor GetDescriptor<TChallenge, TSolution>(string? descriptorName = default)
        where TChallenge : IChallenge where TSolution : ISolution;
}