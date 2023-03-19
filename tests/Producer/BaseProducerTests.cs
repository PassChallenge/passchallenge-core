using PassChallenge.Core.Producer;
using PassChallenge.Core.Solutions;
using PassChallenge.Core.Solver;
using PassChallenge.Core.Tests.Tools;
using Moq;
using NUnit.Framework;
using PassChallenge.Core.Challenges;

namespace PassChallenge.Core.Tests.Producer;

public class BaseProducerTests
{
    [Test]
    public void SetAvailableChallengeAndSolutionStorage_CanProduce_Is_Correct()
    {
        TestBaseProducer producer = new();
        producer.SetAvailableChallengeAndSolutionStorage(new AvailableChallengeAndSolutionStorage(
            new Dictionary<Type, Dictionary<Type, HashSet<string>>>()
            {
                {
                    typeof(TestChallenge), new Dictionary<Type, HashSet<string>>()
                    {
                        { typeof(TestSolution), new HashSet<string>() }
                    }
                }
            })
        );

        Assert.Multiple(() =>
        {
            Assert.That(producer.CanProduce<TestChallenge, TestSolution>(), Is.True);
            Assert.That(producer.CanProduce<IChallenge, ISolution>(), Is.False);
        });
    }

    [Test]
    public void
        SetAvailableChallengeAndSolutionStorage_When_AvailableChallengeAndSolutionStorage_Is_Null_Throws_ArgumentNullException()
    {
        TestBaseProducer producer = new();
        Assert.Throws<ArgumentNullException>(() => producer.SetAvailableChallengeAndSolutionStorage(null!));
    }

    [Test]
    public void ProduceAndWaitSolution_Is_Correct()
    {
        Mock<BaseProducer> mock = new();

        IChallenge expectedChallenge = It.IsAny<IChallenge>();
        CancellationToken expectedCancellationToken = new CancellationTokenSource(TimeSpan.FromHours(1)).Token;

        mock.Object.ProduceAndWaitSolution<IChallenge, ISolution>(expectedChallenge, It.IsAny<string>(),
            expectedCancellationToken);
        mock.Verify(x =>
            x.ProduceAndWaitSolution<IChallenge, ISolution>(It.Is<IChallenge>(mo => mo == expectedChallenge),
                It.Is<string>(mo => mo == default),
                It.Is<CancellationToken>(mo => mo == expectedCancellationToken)));
    }
}