using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Handlers;
using KillDNS.CaptchaSolver.Core.Solutions;
using KillDNS.CaptchaSolver.Core.Tests.Tools;
using Moq;
using NUnit.Framework;

namespace KillDNS.CaptchaSolver.Core.Tests.Handlers;

public class CaptchaHandlerDescriptorStorageTests
{
    private readonly List<CaptchaHandlerDescriptor> _descriptors;

    public CaptchaHandlerDescriptorStorageTests()
    {
        _descriptors = new List<CaptchaHandlerDescriptor>
        {
            CaptchaHandlerDescriptor.Create<ICaptcha, ISolution>((_, _) => Task.FromResult(It.IsAny<ISolution>()),
                "handler-1"),
            CaptchaHandlerDescriptor.Create<TestCaptcha, TestSolution, TestCaptchaHandler>("handler-2")
        };
    }

    [Test]
    public void Constructor_Is_Correct()
    {
        Dictionary<(Type captchaType, Type solutionType), Dictionary<string, CaptchaHandlerDescriptor>> descriptors =
            new()
            {
                {
                    (_descriptors[0].CaptchaType, _descriptors[0].SolutionType),
                    new Dictionary<string, CaptchaHandlerDescriptor>
                    {
                        { _descriptors[0].HandlerName!, _descriptors[0] }
                    }
                },
                {
                    (_descriptors[1].CaptchaType, _descriptors[1].SolutionType),
                    new Dictionary<string, CaptchaHandlerDescriptor>
                    {
                        { _descriptors[1].HandlerName!, _descriptors[1] }
                    }
                }
            };

        CaptchaHandlerDescriptorStorage storage = new(descriptors);

        Assert.That(storage.Descriptors, Is.EquivalentTo(_descriptors));
    }

    [Test]
    public void Constructor_When_Descriptors_Is_Null_Throws_ArgumentNullException()
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => new CaptchaHandlerDescriptorStorage(null!));
    }

    [Test]
    public void ContainsDescriptor_When_Default_HandlerName_Returns_True()
    {
        Dictionary<(Type captchaType, Type solutionType), Dictionary<string, CaptchaHandlerDescriptor>> descriptors =
            new()
            {
                {
                    (_descriptors[0].CaptchaType, _descriptors[0].SolutionType),
                    new Dictionary<string, CaptchaHandlerDescriptor>
                    {
                        { _descriptors[0].HandlerName!, _descriptors[0] }
                    }
                }
            };

        CaptchaHandlerDescriptorStorage storage = new(descriptors);

        Assert.That(storage.ContainsDescriptor<ICaptcha, ISolution>(), Is.True);
    }

    [Test]
    public void ContainsDescriptor_When_Default_HandlerName_Returns_False()
    {
        Dictionary<(Type captchaType, Type solutionType), Dictionary<string, CaptchaHandlerDescriptor>> descriptors =
            new()
            {
                {
                    (_descriptors[0].CaptchaType, _descriptors[0].SolutionType),
                    new Dictionary<string, CaptchaHandlerDescriptor>
                    {
                        { _descriptors[0].HandlerName!, _descriptors[0] }
                    }
                }
            };

        CaptchaHandlerDescriptorStorage storage = new(descriptors);

        Assert.That(storage.ContainsDescriptor<TestCaptcha, TestSolution>(), Is.False);
    }

    [Test]
    public void ContainsDescriptor_When_Concrete_HandlerName_Returns_True()
    {
        Dictionary<(Type captchaType, Type solutionType), Dictionary<string, CaptchaHandlerDescriptor>> descriptors =
            new()
            {
                {
                    (_descriptors[0].CaptchaType, _descriptors[0].SolutionType),
                    new Dictionary<string, CaptchaHandlerDescriptor>
                    {
                        { _descriptors[0].HandlerName!, _descriptors[0] }
                    }
                }
            };

        CaptchaHandlerDescriptorStorage storage = new(descriptors);

        Assert.That(storage.ContainsDescriptor<ICaptcha, ISolution>(_descriptors[0].HandlerName), Is.True);
    }

    [Test]
    public void ContainsDescriptor_When_Concrete_HandlerName_Returns_False()
    {
        Dictionary<(Type captchaType, Type solutionType), Dictionary<string, CaptchaHandlerDescriptor>> descriptors =
            new()
            {
                {
                    (_descriptors[0].CaptchaType, _descriptors[0].SolutionType),
                    new Dictionary<string, CaptchaHandlerDescriptor>
                    {
                        { _descriptors[0].HandlerName!, _descriptors[0] }
                    }
                }
            };

        CaptchaHandlerDescriptorStorage storage = new(descriptors);

        Assert.That(storage.ContainsDescriptor<ICaptcha, ISolution>(_descriptors[1].HandlerName), Is.False);
    }

    [Test]
    public void ContainsDescriptor_When_Descriptors_Is_Empty_Returns_False()
    {
        Dictionary<(Type captchaType, Type solutionType), Dictionary<string, CaptchaHandlerDescriptor>> descriptors =
            new();

        CaptchaHandlerDescriptorStorage storage = new(descriptors);

        Assert.That(storage.ContainsDescriptor<ICaptcha, ISolution>(), Is.False);
    }

    [Test]
    public void GetDefaultDescriptorName_Is_Correct()
    {
        Dictionary<(Type captchaType, Type solutionType), Dictionary<string, CaptchaHandlerDescriptor>> descriptors =
            new()
            {
                {
                    (_descriptors[0].CaptchaType, _descriptors[0].SolutionType),
                    new Dictionary<string, CaptchaHandlerDescriptor>
                    {
                        { _descriptors[0].HandlerName!, _descriptors[0] }
                    }
                }
            };

        CaptchaHandlerDescriptorStorage storage = new(descriptors);

        Assert.That(storage.GetDefaultDescriptorName<ICaptcha, ISolution>(), Is.EqualTo(_descriptors[0].HandlerName));
    }

    [Test]
    public void GetDefaultDescriptorName_When_Descriptors_Not_Contain_That_Descriptor_Throws_InvalidOperationException()
    {
        Dictionary<(Type captchaType, Type solutionType), Dictionary<string, CaptchaHandlerDescriptor>> descriptors =
            new();

        CaptchaHandlerDescriptorStorage storage = new(descriptors);

        Assert.Throws<InvalidOperationException>(() => storage.GetDefaultDescriptorName<ICaptcha, ISolution>());
    }

    [Test]
    public void GetDescriptors_Is_Correct()
    {
        List<CaptchaHandlerDescriptor> expectedDescriptors = new()
        {
            _descriptors[0].CloneWithNewName("handler-1"),
            _descriptors[0].CloneWithNewName("handler-2")
        };

        Dictionary<(Type captchaType, Type solutionType), Dictionary<string, CaptchaHandlerDescriptor>> descriptors =
            new()
            {
                {
                    (expectedDescriptors[0].CaptchaType, expectedDescriptors[0].SolutionType),
                    new Dictionary<string, CaptchaHandlerDescriptor>
                    {
                        { expectedDescriptors[0].HandlerName!, expectedDescriptors[0] },
                        { expectedDescriptors[1].HandlerName!, expectedDescriptors[1] },
                    }
                }
            };

        CaptchaHandlerDescriptorStorage storage = new(descriptors);

        Assert.That(storage.GetDescriptors<ICaptcha, ISolution>(), Is.EquivalentTo(expectedDescriptors));
    }

    [Test]
    public void GetDescriptors_When_Descriptors_Not_Contain_That_Descriptor_Types_Throws_InvalidOperationException()
    {

        Dictionary<(Type captchaType, Type solutionType), Dictionary<string, CaptchaHandlerDescriptor>> descriptors =
            new();

        CaptchaHandlerDescriptorStorage storage = new(descriptors);

        Assert.Throws<InvalidOperationException>(() => storage.GetDescriptors<ICaptcha, ISolution>());
    }

    [Test]
    public void GetDescriptor_Is_Correct()
    {
        Dictionary<(Type captchaType, Type solutionType), Dictionary<string, CaptchaHandlerDescriptor>> descriptors =
            new()
            {
                {
                    (_descriptors[0].CaptchaType, _descriptors[0].SolutionType),
                    new Dictionary<string, CaptchaHandlerDescriptor>
                    {
                        { _descriptors[0].HandlerName!, _descriptors[0] },
                    }
                }
            };

        CaptchaHandlerDescriptorStorage storage = new(descriptors);

        Assert.That(storage.GetDescriptor<ICaptcha, ISolution>(), Is.EqualTo(_descriptors[0]));
        Assert.That(storage.GetDescriptor<ICaptcha, ISolution>(_descriptors[0].HandlerName),
            Is.EqualTo(_descriptors[0]));
    }

    [Test]
    public void GetDescriptor_When_Descriptors_Not_Contain_That_Descriptor_Throws_InvalidOperationException()
    {

        Dictionary<(Type captchaType, Type solutionType), Dictionary<string, CaptchaHandlerDescriptor>> descriptors =
            new()
            {
                {
                    (_descriptors[0].CaptchaType, _descriptors[0].SolutionType),
                    new Dictionary<string, CaptchaHandlerDescriptor>
                    {
                        { _descriptors[0].HandlerName!, _descriptors[0] },
                    }
                }
            };

        CaptchaHandlerDescriptorStorage storage = new(descriptors);

        Assert.Throws<InvalidOperationException>(() =>
            storage.GetDescriptor<ICaptcha, ISolution>(_descriptors[1].HandlerName));
        Assert.Throws<InvalidOperationException>(() => storage.GetDescriptor<TestCaptcha, TestSolution>());
    }

    [Test]
    public void IsAvailable_Is_Correct()
    {
        Dictionary<(Type captchaType, Type solutionType), Dictionary<string, CaptchaHandlerDescriptor>> descriptors =
            new()
            {
                {
                    (_descriptors[0].CaptchaType, _descriptors[0].SolutionType),
                    new Dictionary<string, CaptchaHandlerDescriptor>
                    {
                        { _descriptors[0].HandlerName!, _descriptors[0] },
                    }
                }
            };

        CaptchaHandlerDescriptorStorage storage = new(descriptors);

        Assert.That(storage.IsAvailable<ICaptcha, ISolution>(), Is.True);
        Assert.That(storage.IsAvailable<ICaptcha, ISolution>(_descriptors[0].HandlerName),
            Is.True);
    }
}