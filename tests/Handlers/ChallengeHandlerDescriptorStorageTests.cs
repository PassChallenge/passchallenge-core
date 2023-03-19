using PassChallenge.Core.Handlers;
using PassChallenge.Core.Solutions;
using PassChallenge.Core.Tests.Tools;
using Moq;
using NUnit.Framework;
using PassChallenge.Core.Challenges;

namespace PassChallenge.Core.Tests.Handlers;

public class ChallengeHandlerDescriptorStorageTests
{
    private readonly List<ChallengeHandlerDescriptor> _descriptors;

    public ChallengeHandlerDescriptorStorageTests()
    {
        _descriptors = new List<ChallengeHandlerDescriptor>
        {
            ChallengeHandlerDescriptor.Create<IChallenge, ISolution>((_, _) => Task.FromResult(It.IsAny<ISolution>()),
                "handler-1"),
            ChallengeHandlerDescriptor.Create<TestChallenge, TestSolution, TestChallengeHandler<TestChallenge, TestSolution>>(
                "handler-2")
        };
    }

    [Test]
    public void Constructor_Is_Correct()
    {
        Dictionary<(Type ChallengeType, Type solutionType), Dictionary<string, ChallengeHandlerDescriptor>> descriptors =
            new()
            {
                {
                    (_descriptors[0].ChallengeType, _descriptors[0].SolutionType),
                    new Dictionary<string, ChallengeHandlerDescriptor>
                    {
                        { _descriptors[0].HandlerName!, _descriptors[0] }
                    }
                },
                {
                    (_descriptors[1].ChallengeType, _descriptors[1].SolutionType),
                    new Dictionary<string, ChallengeHandlerDescriptor>
                    {
                        { _descriptors[1].HandlerName!, _descriptors[1] }
                    }
                }
            };

        ChallengeHandlerDescriptorStorage storage = new(descriptors);

        Assert.That(storage.Descriptors, Is.EquivalentTo(_descriptors));
    }

    [Test]
    public void Constructor_When_Descriptors_Is_Null_Throws_ArgumentNullException()
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => new ChallengeHandlerDescriptorStorage(null!));
    }

    [Test]
    public void ContainsDescriptor_When_Default_HandlerName_Returns_True()
    {
        Dictionary<(Type ChallengeType, Type solutionType), Dictionary<string, ChallengeHandlerDescriptor>> descriptors =
            new()
            {
                {
                    (_descriptors[0].ChallengeType, _descriptors[0].SolutionType),
                    new Dictionary<string, ChallengeHandlerDescriptor>
                    {
                        { _descriptors[0].HandlerName!, _descriptors[0] }
                    }
                }
            };

        ChallengeHandlerDescriptorStorage storage = new(descriptors);

        Assert.That(storage.ContainsDescriptor<IChallenge, ISolution>(), Is.True);
    }

    [Test]
    public void ContainsDescriptor_When_Default_HandlerName_Returns_False()
    {
        Dictionary<(Type ChallengeType, Type solutionType), Dictionary<string, ChallengeHandlerDescriptor>> descriptors =
            new()
            {
                {
                    (_descriptors[0].ChallengeType, _descriptors[0].SolutionType),
                    new Dictionary<string, ChallengeHandlerDescriptor>
                    {
                        { _descriptors[0].HandlerName!, _descriptors[0] }
                    }
                }
            };

        ChallengeHandlerDescriptorStorage storage = new(descriptors);

        Assert.That(storage.ContainsDescriptor<TestChallenge, TestSolution>(), Is.False);
    }

    [Test]
    public void ContainsDescriptor_When_Concrete_HandlerName_Returns_True()
    {
        Dictionary<(Type ChallengeType, Type solutionType), Dictionary<string, ChallengeHandlerDescriptor>> descriptors =
            new()
            {
                {
                    (_descriptors[0].ChallengeType, _descriptors[0].SolutionType),
                    new Dictionary<string, ChallengeHandlerDescriptor>
                    {
                        { _descriptors[0].HandlerName!, _descriptors[0] }
                    }
                }
            };

        ChallengeHandlerDescriptorStorage storage = new(descriptors);

        Assert.That(storage.ContainsDescriptor<IChallenge, ISolution>(_descriptors[0].HandlerName), Is.True);
    }

    [Test]
    public void ContainsDescriptor_When_Concrete_HandlerName_Returns_False()
    {
        Dictionary<(Type ChallengeType, Type solutionType), Dictionary<string, ChallengeHandlerDescriptor>> descriptors =
            new()
            {
                {
                    (_descriptors[0].ChallengeType, _descriptors[0].SolutionType),
                    new Dictionary<string, ChallengeHandlerDescriptor>
                    {
                        { _descriptors[0].HandlerName!, _descriptors[0] }
                    }
                }
            };

        ChallengeHandlerDescriptorStorage storage = new(descriptors);

        Assert.That(storage.ContainsDescriptor<IChallenge, ISolution>(_descriptors[1].HandlerName), Is.False);
    }

    [Test]
    public void ContainsDescriptor_When_Descriptors_Is_Empty_Returns_False()
    {
        Dictionary<(Type ChallengeType, Type solutionType), Dictionary<string, ChallengeHandlerDescriptor>> descriptors =
            new();

        ChallengeHandlerDescriptorStorage storage = new(descriptors);

        Assert.That(storage.ContainsDescriptor<IChallenge, ISolution>(), Is.False);
    }

    [Test]
    public void GetDefaultDescriptorName_Is_Correct()
    {
        Dictionary<(Type ChallengeType, Type solutionType), Dictionary<string, ChallengeHandlerDescriptor>> descriptors =
            new()
            {
                {
                    (_descriptors[0].ChallengeType, _descriptors[0].SolutionType),
                    new Dictionary<string, ChallengeHandlerDescriptor>
                    {
                        { _descriptors[0].HandlerName!, _descriptors[0] }
                    }
                }
            };

        ChallengeHandlerDescriptorStorage storage = new(descriptors);

        Assert.That(storage.GetDefaultDescriptorName<IChallenge, ISolution>(), Is.EqualTo(_descriptors[0].HandlerName));
    }

    [Test]
    public void GetDefaultDescriptorName_When_Descriptors_Not_Contain_That_Descriptor_Throws_InvalidOperationException()
    {
        Dictionary<(Type ChallengeType, Type solutionType), Dictionary<string, ChallengeHandlerDescriptor>> descriptors =
            new();

        ChallengeHandlerDescriptorStorage storage = new(descriptors);

        Assert.Throws<InvalidOperationException>(() => storage.GetDefaultDescriptorName<IChallenge, ISolution>());
    }

    [Test]
    public void GetDescriptors_Is_Correct()
    {
        List<ChallengeHandlerDescriptor> expectedDescriptors = new()
        {
            _descriptors[0].CloneWithNewName("handler-1"),
            _descriptors[0].CloneWithNewName("handler-2")
        };

        Dictionary<(Type ChallengeType, Type solutionType), Dictionary<string, ChallengeHandlerDescriptor>> descriptors =
            new()
            {
                {
                    (expectedDescriptors[0].ChallengeType, expectedDescriptors[0].SolutionType),
                    new Dictionary<string, ChallengeHandlerDescriptor>
                    {
                        { expectedDescriptors[0].HandlerName!, expectedDescriptors[0] },
                        { expectedDescriptors[1].HandlerName!, expectedDescriptors[1] },
                    }
                }
            };

        ChallengeHandlerDescriptorStorage storage = new(descriptors);

        Assert.That(storage.GetDescriptors<IChallenge, ISolution>(), Is.EquivalentTo(expectedDescriptors));
    }

    [Test]
    public void GetDescriptors_When_Descriptors_Not_Contain_That_Descriptor_Types_Throws_InvalidOperationException()
    {

        Dictionary<(Type ChallengeType, Type solutionType), Dictionary<string, ChallengeHandlerDescriptor>> descriptors =
            new();

        ChallengeHandlerDescriptorStorage storage = new(descriptors);

        Assert.Throws<InvalidOperationException>(() => storage.GetDescriptors<IChallenge, ISolution>());
    }

    [Test]
    public void GetDescriptor_Is_Correct()
    {
        Dictionary<(Type ChallengeType, Type solutionType), Dictionary<string, ChallengeHandlerDescriptor>> descriptors =
            new()
            {
                {
                    (_descriptors[0].ChallengeType, _descriptors[0].SolutionType),
                    new Dictionary<string, ChallengeHandlerDescriptor>
                    {
                        { _descriptors[0].HandlerName!, _descriptors[0] },
                    }
                }
            };

        ChallengeHandlerDescriptorStorage storage = new(descriptors);

        Assert.That(storage.GetDescriptor<IChallenge, ISolution>(), Is.EqualTo(_descriptors[0]));
        Assert.That(storage.GetDescriptor<IChallenge, ISolution>(_descriptors[0].HandlerName),
            Is.EqualTo(_descriptors[0]));
    }

    [Test]
    public void GetDescriptor_When_Descriptors_Not_Contain_That_Descriptor_Throws_InvalidOperationException()
    {

        Dictionary<(Type ChallengeType, Type solutionType), Dictionary<string, ChallengeHandlerDescriptor>> descriptors =
            new()
            {
                {
                    (_descriptors[0].ChallengeType, _descriptors[0].SolutionType),
                    new Dictionary<string, ChallengeHandlerDescriptor>
                    {
                        { _descriptors[0].HandlerName!, _descriptors[0] },
                    }
                }
            };

        ChallengeHandlerDescriptorStorage storage = new(descriptors);

        Assert.Throws<InvalidOperationException>(() =>
            storage.GetDescriptor<IChallenge, ISolution>(_descriptors[1].HandlerName));
        Assert.Throws<InvalidOperationException>(() => storage.GetDescriptor<TestChallenge, TestSolution>());
    }

    [Test]
    public void IsAvailable_Is_Correct()
    {
        Dictionary<(Type ChallengeType, Type solutionType), Dictionary<string, ChallengeHandlerDescriptor>> descriptors =
            new()
            {
                {
                    (_descriptors[0].ChallengeType, _descriptors[0].SolutionType),
                    new Dictionary<string, ChallengeHandlerDescriptor>
                    {
                        { _descriptors[0].HandlerName!, _descriptors[0] },
                    }
                }
            };

        ChallengeHandlerDescriptorStorage storage = new(descriptors);

        Assert.That(storage.IsAvailable<IChallenge, ISolution>(), Is.True);
        Assert.That(storage.IsAvailable<IChallenge, ISolution>(_descriptors[0].HandlerName),
            Is.True);
    }
}