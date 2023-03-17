using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Solutions;
using KillDNS.CaptchaSolver.Core.Solver;
using KillDNS.CaptchaSolver.Core.Tests.Tools;
using NUnit.Framework;

namespace KillDNS.CaptchaSolver.Core.Tests.Solver;

public class AvailableCaptchaAndSolutionStorageTests
{
    [Test]
    public void Constructor_Is_Correct()
    {
        var availableCaptchaAndSolutions = new Dictionary<Type, Dictionary<Type, HashSet<string>>>();

        // ReSharper disable once ObjectCreationAsStatement
        Assert.DoesNotThrow(() => new AvailableCaptchaAndSolutionStorage(availableCaptchaAndSolutions));
    }

    [Test]
    public void Constructor_When_AvailableCaptchaAndSolutions_Is_Null_Throws_ArgumentNullException()
    {
        // ReSharper disable once ObjectCreationAsStatement
        Assert.Throws<ArgumentNullException>(() => new AvailableCaptchaAndSolutionStorage(null!));
    }

    [Test]
    public void IsAvailable_When_AvailableCaptchaAndSolutions_Is_Empty_Returns_False()
    {
        var availableCaptchaAndSolutions = new Dictionary<Type, Dictionary<Type, HashSet<string>>>();
        AvailableCaptchaAndSolutionStorage storage = new(availableCaptchaAndSolutions);
        Assert.That(storage.IsAvailable<ICaptcha, ISolution>(), Is.False);
    }

    [Test]
    public void IsAvailable_When_Pair_Is_Found_In_AvailableCaptchaAndSolutions_Returns_True()
    {
        var availableCaptchaAndSolutions = new Dictionary<Type, Dictionary<Type, HashSet<string>>>();
        availableCaptchaAndSolutions.Add(typeof(ICaptcha), new Dictionary<Type, HashSet<string>>
        {
            { typeof(ISolution), new HashSet<string>() }
        });

        AvailableCaptchaAndSolutionStorage storage = new(availableCaptchaAndSolutions);
        Assert.That(storage.IsAvailable<ICaptcha, ISolution>(), Is.True);
    }

    [Test]
    public void IsAvailable_When_Pair_Not_Found_In_AvailableCaptchaAndSolutions_Returns_False()
    {
        var availableCaptchaAndSolutions = new Dictionary<Type, Dictionary<Type, HashSet<string>>>();
        availableCaptchaAndSolutions.Add(typeof(ICaptcha), new Dictionary<Type, HashSet<string>>
        {
            { typeof(ISolution), new HashSet<string>() }
        });

        AvailableCaptchaAndSolutionStorage storage = new(availableCaptchaAndSolutions);
        Assert.That(storage.IsAvailable<TestCaptcha, TestSolution>(), Is.False);
    }

    [Test]
    public void
        IsAvailable_When_HandlerName_Not_Set_And_Default_Pair_Is_Found_In_AvailableCaptchaAndSolutions_Returns_True()
    {
        string expectedHandlerName = "handler-name";

        var availableCaptchaAndSolutions = new Dictionary<Type, Dictionary<Type, HashSet<string>>>();
        availableCaptchaAndSolutions.Add(typeof(ICaptcha), new Dictionary<Type, HashSet<string>>
        {
            {
                typeof(ISolution), new HashSet<string>
                {
                    expectedHandlerName
                }
            }
        });

        AvailableCaptchaAndSolutionStorage storage = new(availableCaptchaAndSolutions);
        Assert.That(storage.IsAvailable<ICaptcha, ISolution>(), Is.True);
    }

    [Test]
    public void IsAvailable_When_HandlerName_Found_In_AvailableCaptchaAndSolutions_Returns_True()
    {
        string expectedHandlerName = "handler-name";

        var availableCaptchaAndSolutions = new Dictionary<Type, Dictionary<Type, HashSet<string>>>();
        availableCaptchaAndSolutions.Add(typeof(ICaptcha), new Dictionary<Type, HashSet<string>>
        {
            {
                typeof(ISolution), new HashSet<string>
                {
                    expectedHandlerName
                }
            }
        });

        AvailableCaptchaAndSolutionStorage storage = new(availableCaptchaAndSolutions);
        Assert.That(storage.IsAvailable<ICaptcha, ISolution>(expectedHandlerName), Is.True);
    }

    [Test]
    public void IsAvailable_When_HandlerName_Not_Found_In_AvailableCaptchaAndSolutions_Returns_False()
    {
        string expectedHandlerName = "handler-name";
        string wrongHandlerName = "handler-name-not-found";

        var availableCaptchaAndSolutions = new Dictionary<Type, Dictionary<Type, HashSet<string>>>();
        availableCaptchaAndSolutions.Add(typeof(ICaptcha), new Dictionary<Type, HashSet<string>>
        {
            {
                typeof(ISolution), new HashSet<string>
                {
                    expectedHandlerName
                }
            }
        });

        AvailableCaptchaAndSolutionStorage storage = new(availableCaptchaAndSolutions);
        Assert.That(storage.IsAvailable<ICaptcha, ISolution>(wrongHandlerName), Is.False);
    }
}