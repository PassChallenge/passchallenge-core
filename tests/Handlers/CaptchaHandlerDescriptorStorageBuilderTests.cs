using KillDNS.CaptchaSolver.Core.Handlers;
using KillDNS.CaptchaSolver.Core.Tests.Tools;
using Moq;
using NUnit.Framework;

namespace KillDNS.CaptchaSolver.Core.Tests.Handlers;

public class CaptchaHandlerDescriptorStorageBuilderTests
{
    [Test]
    public void AddCaptchaHandler_With_Handler_And_Default_HandlerName_Is_Correct()
    {
        CaptchaHandlerDescriptorStorageBuilder builder = new();
        builder.AddCaptchaHandler<TestCaptcha, TestSolution, TestCaptchaHandler>();

        ICaptchaHandlerDescriptorAvailableStorage storage = builder.Build();

        CaptchaHandlerDescriptor descriptor = storage.GetDescriptor<TestCaptcha, TestSolution>();

        Assert.Multiple(() =>
        {
            Assert.That(descriptor.CaptchaType, Is.EqualTo(typeof(TestCaptcha)));
            Assert.That(descriptor.SolutionType, Is.EqualTo(typeof(TestSolution)));
            Assert.That(descriptor.HandlerType, Is.EqualTo(typeof(TestCaptchaHandler)));
            Assert.That(descriptor.HandlerName,
                Is.EqualTo($"{nameof(TestCaptcha)}-{nameof(TestSolution)}-0".ToLower()));
            Assert.IsNull(descriptor.ImplementationFactory);
            Assert.IsNull(descriptor.SolverFunction);
        });
    }

    [Test]
    public void AddCaptchaHandler_With_Handler_And_Concrete_HandlerName_Is_Correct()
    {
        string expectedHandlerName = "handler-name";
        CaptchaHandlerDescriptorStorageBuilder builder = new();
        builder.AddCaptchaHandler<TestCaptcha, TestSolution, TestCaptchaHandler>(expectedHandlerName);

        ICaptchaHandlerDescriptorAvailableStorage storage = builder.Build();

        CaptchaHandlerDescriptor descriptor = storage.GetDescriptor<TestCaptcha, TestSolution>();

        Assert.Multiple(() =>
        {
            Assert.That(descriptor.CaptchaType, Is.EqualTo(typeof(TestCaptcha)));
            Assert.That(descriptor.SolutionType, Is.EqualTo(typeof(TestSolution)));
            Assert.That(descriptor.HandlerType, Is.EqualTo(typeof(TestCaptchaHandler)));
            Assert.That(descriptor.HandlerName, Is.EqualTo(expectedHandlerName));
            Assert.IsNull(descriptor.ImplementationFactory);
            Assert.IsNull(descriptor.SolverFunction);
        });
    }

    [Test]
    public void AddCaptchaHandler_With_Handler_Factory_And_Default_HandlerName_Is_Correct()
    {
        CaptchaHandlerDescriptorStorageBuilder builder = new();
        builder.AddCaptchaHandler<TestCaptcha, TestSolution, TestCaptchaHandler>(_ => new TestCaptchaHandler());

        ICaptchaHandlerDescriptorAvailableStorage storage = builder.Build();

        CaptchaHandlerDescriptor descriptor = storage.GetDescriptor<TestCaptcha, TestSolution>();

        Assert.Multiple(() =>
        {
            Assert.That(descriptor.CaptchaType, Is.EqualTo(typeof(TestCaptcha)));
            Assert.That(descriptor.SolutionType, Is.EqualTo(typeof(TestSolution)));
            Assert.That(descriptor.HandlerType, Is.EqualTo(typeof(TestCaptchaHandler)));
            Assert.That(descriptor.HandlerName,
                Is.EqualTo($"{nameof(TestCaptcha)}-{nameof(TestSolution)}-0".ToLower()));
            Assert.IsNotNull(descriptor.ImplementationFactory);
            Assert.IsNull(descriptor.SolverFunction);
        });
    }

    [Test]
    public void AddCaptchaHandler_With_Handler_Factory_And_Concrete_HandlerName_Is_Correct()
    {
        string expectedHandlerName = "handler-name";
        CaptchaHandlerDescriptorStorageBuilder builder = new();
        builder.AddCaptchaHandler<TestCaptcha, TestSolution, TestCaptchaHandler>(_ => new TestCaptchaHandler(),
            expectedHandlerName);

        ICaptchaHandlerDescriptorAvailableStorage storage = builder.Build();

        CaptchaHandlerDescriptor descriptor = storage.GetDescriptor<TestCaptcha, TestSolution>();

        Assert.Multiple(() =>
        {
            Assert.That(descriptor.CaptchaType, Is.EqualTo(typeof(TestCaptcha)));
            Assert.That(descriptor.SolutionType, Is.EqualTo(typeof(TestSolution)));
            Assert.That(descriptor.HandlerType, Is.EqualTo(typeof(TestCaptchaHandler)));
            Assert.That(descriptor.HandlerName, Is.EqualTo(expectedHandlerName));
            Assert.IsNotNull(descriptor.ImplementationFactory);
            Assert.IsNull(descriptor.SolverFunction);
        });
    }

    [Test]
    public void AddCaptchaHandler_With_Solver_Function_And_Default_HandlerName_Is_Correct()
    {
        CaptchaHandlerDescriptorStorageBuilder builder = new();
        builder.AddCaptchaHandler<TestCaptcha, TestSolution>((_, _) => Task.FromResult(It.IsAny<TestSolution>()));

        ICaptchaHandlerDescriptorAvailableStorage storage = builder.Build();

        CaptchaHandlerDescriptor descriptor = storage.GetDescriptor<TestCaptcha, TestSolution>();

        Assert.Multiple(() =>
        {
            Assert.That(descriptor.CaptchaType, Is.EqualTo(typeof(TestCaptcha)));
            Assert.That(descriptor.SolutionType, Is.EqualTo(typeof(TestSolution)));
            Assert.IsNull(descriptor.HandlerType);
            Assert.That(descriptor.HandlerName,
                Is.EqualTo($"{nameof(TestCaptcha)}-{nameof(TestSolution)}-0".ToLower()));
            Assert.IsNull(descriptor.ImplementationFactory);
            Assert.IsNotNull(descriptor.SolverFunction);
        });
    }

    [Test]
    public void AddCaptchaHandler_With_Solver_Function_And_Concrete_HandlerName_Is_Correct()
    {
        string expectedHandlerName = "handler-name";
        CaptchaHandlerDescriptorStorageBuilder builder = new();
        builder.AddCaptchaHandler<TestCaptcha, TestSolution>((_, _) => Task.FromResult(It.IsAny<TestSolution>()),
            expectedHandlerName);

        ICaptchaHandlerDescriptorAvailableStorage storage = builder.Build();

        CaptchaHandlerDescriptor descriptor = storage.GetDescriptor<TestCaptcha, TestSolution>();

        Assert.Multiple(() =>
        {
            Assert.That(descriptor.CaptchaType, Is.EqualTo(typeof(TestCaptcha)));
            Assert.That(descriptor.SolutionType, Is.EqualTo(typeof(TestSolution)));
            Assert.IsNull(descriptor.HandlerType);
            Assert.That(descriptor.HandlerName, Is.EqualTo(expectedHandlerName));
            Assert.IsNull(descriptor.ImplementationFactory);
            Assert.IsNotNull(descriptor.SolverFunction);
        });
    }


    [Test]
    public void AddCaptchaHandler_With_Multiple_Handlers_And_Default_HandlerName_Is_Correct()
    {
        CaptchaHandlerDescriptorStorageBuilder builder = new();

        builder.AddCaptchaHandler<TestCaptcha, TestSolution, TestCaptchaHandler>();
        builder.AddCaptchaHandler<TestCaptcha, TestSolution, TestCaptchaHandler>();

        ICaptchaHandlerDescriptorAvailableStorage storage = builder.Build();

        IReadOnlyList<CaptchaHandlerDescriptor> descriptors =
            storage.GetDescriptors<TestCaptcha, TestSolution>().ToList();

        Assert.That(descriptors.Count, Is.EqualTo(2));

        for (int i = 0; i < descriptors.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(descriptors[i].CaptchaType, Is.EqualTo(typeof(TestCaptcha)));
                Assert.That(descriptors[i].SolutionType, Is.EqualTo(typeof(TestSolution)));
                Assert.That(descriptors[i].HandlerType, Is.EqualTo(typeof(TestCaptchaHandler)));
                Assert.That(descriptors[i].HandlerName,
                    Is.EqualTo($"{nameof(TestCaptcha)}-{nameof(TestSolution)}-{i}".ToLower()));
                Assert.IsNull(descriptors[i].ImplementationFactory);
                Assert.IsNull(descriptors[i].SolverFunction);
            });
        }
    }

    [Test]
    public void AddCaptchaHandler_With_Multiple_Handlers_And_Repeated_HandlerName_Throws_InvalidOperationException()
    {
        string expectedHandlerName = "handler-name";
        CaptchaHandlerDescriptorStorageBuilder builder = new();

        builder.AddCaptchaHandler<TestCaptcha, TestSolution, TestCaptchaHandler>(expectedHandlerName);
        Assert.Throws<InvalidOperationException>(() =>
            builder.AddCaptchaHandler<TestCaptcha, TestSolution, TestCaptchaHandler>(expectedHandlerName));
    }
}