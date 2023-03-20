using PassChallenge.Core.Challenges;
using PassChallenge.Core.Handlers;
using PassChallenge.Core.Solutions;

namespace PassChallenge.Core.Tests.Tools;

public class TestChallengeHandlerDescriptorStorage : IChallengeHandlerDescriptorStorage
{
    public IReadOnlyCollection<ChallengeHandlerDescriptor> Descriptors { get; }

    public TestChallengeHandlerDescriptorStorage()
    {
        Descriptors = null!;
    }

    public bool ContainsDescriptor<TChallenge, TSolution>(string? descriptorName = default)
        where TChallenge : IChallenge where TSolution : ISolution
    {
        throw new NotImplementedException();
    }

    public string GetDefaultDescriptorName<TChallenge, TSolution>() where TChallenge : IChallenge where TSolution : ISolution
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<ChallengeHandlerDescriptor> GetDescriptors<TChallenge, TSolution>()
        where TChallenge : IChallenge where TSolution : ISolution
    {
        throw new NotImplementedException();
    }

    public ChallengeHandlerDescriptor GetDescriptor<TChallenge, TSolution>(string? descriptorName = default)
        where TChallenge : IChallenge where TSolution : ISolution
    {
        throw new NotImplementedException();
    }
}