using PassChallenge.Core.Solutions;
using PassChallenge.Core.Solver;
using PassChallenge.Core.Tests.Tools;
using NUnit.Framework;
using PassChallenge.Core.Challenges;

namespace PassChallenge.Core.Tests.Solver;

public class AvailableChallengeAndSolutionStorageTests
{
    [Test]
    public void Constructor_Is_Correct()
    {
        var availableChallengeAndSolutions = new Dictionary<Type, Dictionary<Type, HashSet<string>>>();

        // ReSharper disable once ObjectCreationAsStatement
        Assert.DoesNotThrow(() => new AvailableChallengeAndSolutionStorage(availableChallengeAndSolutions));
    }

    [Test]
    public void Constructor_When_AvailableChallengeAndSolutions_Is_Null_Throws_ArgumentNullException()
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => new AvailableChallengeAndSolutionStorage(null!));
    }

    [Test]
    public void IsAvailable_When_AvailableChallengeAndSolutions_Is_Empty_Returns_False()
    {
        var availableChallengeAndSolutions = new Dictionary<Type, Dictionary<Type, HashSet<string>>>();
        AvailableChallengeAndSolutionStorage storage = new(availableChallengeAndSolutions);
        Assert.That(storage.IsAvailable<IChallenge, ISolution>(), Is.False);
    }

    [Test]
    public void IsAvailable_When_Pair_Is_Found_In_AvailableChallengeAndSolutions_Returns_True()
    {
        var availableChallengeAndSolutions = new Dictionary<Type, Dictionary<Type, HashSet<string>>>();
        availableChallengeAndSolutions.Add(typeof(IChallenge), new Dictionary<Type, HashSet<string>>
        {
            { typeof(ISolution), new HashSet<string>() }
        });

        AvailableChallengeAndSolutionStorage storage = new(availableChallengeAndSolutions);
        Assert.That(storage.IsAvailable<IChallenge, ISolution>(), Is.True);
    }

    [Test]
    public void IsAvailable_When_Pair_Not_Found_In_AvailableChallengeAndSolutions_Returns_False()
    {
        var availableChallengeAndSolutions = new Dictionary<Type, Dictionary<Type, HashSet<string>>>();
        availableChallengeAndSolutions.Add(typeof(IChallenge), new Dictionary<Type, HashSet<string>>
        {
            { typeof(ISolution), new HashSet<string>() }
        });

        AvailableChallengeAndSolutionStorage storage = new(availableChallengeAndSolutions);
        Assert.That(storage.IsAvailable<TestChallenge, TestSolution>(), Is.False);
    }

    [Test]
    public void
        IsAvailable_When_HandlerName_Not_Set_And_Default_Pair_Is_Found_In_AvailableChallengeAndSolutions_Returns_True()
    {
        string expectedHandlerName = "handler-name";

        var availableChallengeAndSolutions = new Dictionary<Type, Dictionary<Type, HashSet<string>>>();
        availableChallengeAndSolutions.Add(typeof(IChallenge), new Dictionary<Type, HashSet<string>>
        {
            {
                typeof(ISolution), new HashSet<string>
                {
                    expectedHandlerName
                }
            }
        });

        AvailableChallengeAndSolutionStorage storage = new(availableChallengeAndSolutions);
        Assert.That(storage.IsAvailable<IChallenge, ISolution>(), Is.True);
    }

    [Test]
    public void IsAvailable_When_HandlerName_Found_In_AvailableChallengeAndSolutions_Returns_True()
    {
        string expectedHandlerName = "handler-name";

        var availableChallengeAndSolutions = new Dictionary<Type, Dictionary<Type, HashSet<string>>>();
        availableChallengeAndSolutions.Add(typeof(IChallenge), new Dictionary<Type, HashSet<string>>
        {
            {
                typeof(ISolution), new HashSet<string>
                {
                    expectedHandlerName
                }
            }
        });

        AvailableChallengeAndSolutionStorage storage = new(availableChallengeAndSolutions);
        Assert.That(storage.IsAvailable<IChallenge, ISolution>(expectedHandlerName), Is.True);
    }

    [Test]
    public void IsAvailable_When_HandlerName_Not_Found_In_AvailableChallengeAndSolutions_Returns_False()
    {
        string expectedHandlerName = "handler-name";
        string wrongHandlerName = "handler-name-not-found";

        var availableChallengeAndSolutions = new Dictionary<Type, Dictionary<Type, HashSet<string>>>();
        availableChallengeAndSolutions.Add(typeof(IChallenge), new Dictionary<Type, HashSet<string>>
        {
            {
                typeof(ISolution), new HashSet<string>
                {
                    expectedHandlerName
                }
            }
        });

        AvailableChallengeAndSolutionStorage storage = new(availableChallengeAndSolutions);
        Assert.That(storage.IsAvailable<IChallenge, ISolution>(wrongHandlerName), Is.False);
    }
}