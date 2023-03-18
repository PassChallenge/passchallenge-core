using KillDNS.CaptchaSolver.Core.Captcha;
using KillDNS.CaptchaSolver.Core.Extensions;
using KillDNS.CaptchaSolver.Core.Handlers;
using KillDNS.CaptchaSolver.Core.Producer;
using KillDNS.CaptchaSolver.Core.Solutions;
using KillDNS.CaptchaSolver.Core.Solver;
using KillDNS.CaptchaSolver.Core.Tests.Tools;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace KillDNS.CaptchaSolver.Core.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Test]
    [TestCase(ServiceLifetime.Transient)]
    [TestCase(ServiceLifetime.Scoped)]
    [TestCase(ServiceLifetime.Singleton)]
    public void AddCaptchaSolver_DefaultSolver_ServiceCollection_AddMethod_Is_Called(ServiceLifetime lifetime)
    {
        Mock<IServiceCollection> mock = new();
        TestHelper.MakeEnumerableDescriptors(mock);

        mock.Object.AddCaptchaSolver<IProducer>(It.IsAny<string>(), lifetime);

        mock.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Once());
        mock.Verify(
            x => x.Add(It.Is<ServiceDescriptor>(descriptor =>
                descriptor.ServiceType == typeof(ICaptchaSolverFactory) &&
                descriptor.Lifetime == lifetime)), Times.Once());
    }

    [Test]
    public void AddCaptchaSolver_DefaultSolver_When_ServiceCollection_Is_Null_Throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddCaptchaSolver<IProducer>(null!));
    }

    [Test]
    [TestCase(ServiceLifetime.Transient)]
    [TestCase(ServiceLifetime.Scoped)]
    [TestCase(ServiceLifetime.Singleton)]
    public void AddSpecifiedCaptchaSolver_SpecifiedSolver_ServiceCollection_AddMethod_Is_Called(
        ServiceLifetime lifetime)
    {
        Mock<IServiceCollection> mock = new();
        TestHelper.MakeEnumerableDescriptors(mock);

        mock.Object.AddSpecifiedCaptchaSolver<IProducerWithSpecifiedCaptchaAndSolutions>(builder =>
        {
            builder.AvailableCaptchaAndSolutionStorageBuilder.AddSupportCaptchaAndSolution(
                CaptchaHandlerDescriptor.Create<ICaptcha, ISolution>((_, _) =>
                    It.IsAny<Task<ISolution>>()));
        }, It.IsAny<string>(), lifetime);

        mock.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Once());
        mock.Verify(
            x => x.Add(It.Is<ServiceDescriptor>(descriptor =>
                descriptor.ServiceType == typeof(ICaptchaSolverFactory) &&
                descriptor.Lifetime == lifetime)), Times.Once());
    }

    [Test]
    public void AddSpecifiedCaptchaSolver_DefaultSolver_When_ServiceCollection_Is_Null_Throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            ServiceCollectionExtensions.AddSpecifiedCaptchaSolver<IProducerWithSpecifiedCaptchaAndSolutions>(
                null!, _ => { }));
    }

    [Test]
    public void AddSpecifiedCaptchaSolver_With_Builder_When_ServiceCollection_Is_Null_Throws_ArgumentNullException()
    {
        Mock<IServiceCollection> mock = new();
        Assert.Throws<ArgumentNullException>(() =>
            mock.Object.AddSpecifiedCaptchaSolver<IProducerWithSpecifiedCaptchaAndSolutions>(
                null!));
    }

    [Test]
    public void AddCaptchaSolver_With_Empty_Builder_Is_Correct()
    {
        Mock<IServiceCollection> mock = new();
        TestHelper.MakeEnumerableDescriptors(mock);

        mock.Object.AddCaptchaSolver<IProducer>(It.IsAny<string>(), It.IsAny<ServiceLifetime>());
        mock.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Once());
    }

    [Test]
    public void AddCaptchaSolver_With_Empty_Builder_Default_ServiceLifetime_Is_Correct()
    {
        Mock<IServiceCollection> mock = new();
        TestHelper.MakeEnumerableDescriptors(mock);

        mock.Object.AddCaptchaSolver<IProducer>();
        mock.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Once());
    }

    [Test]
    public void AddCaptchaSolver_With_Builder_Is_Correct()
    {
        Mock<IServiceCollection> mock = new();
        TestHelper.MakeEnumerableDescriptors(mock);

        mock.Object.AddCaptchaSolver<IProducer>(_ => { }, It.IsAny<string>(), It.IsAny<ServiceLifetime>());
        mock.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Once());
    }

    [Test]
    public void AddCaptchaSolver_With_Builder_When_ServiceCollection_Is_Null_Throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddCaptchaSolver<IProducer>(null!,
            _ => { }));
    }

    [Test]
    public void AddCaptchaSolver_With_Builder_When_Builder_Is_Null_Throws_ArgumentNullException()
    {
        Mock<IServiceCollection> mock = new();
        Assert.Throws<ArgumentNullException>(() => mock.Object.AddCaptchaSolver<IProducer>(null!, It.IsAny<string>()));
    }

    [Test]
    public void AddCaptchaSolvers_With_Default_SolverNames_Is_Correct()
    {
        Mock<IServiceCollection> mock = new();
        Mock<IServiceProvider> mockServiceProvider = new();
        IList<ServiceDescriptor> descriptors = TestHelper.MakeEnumerableDescriptors(mock);

        mock.Object.AddCaptchaSolver<TestProducer>();
        mock.Object.AddCaptchaSolver<TestProducer>();

        List<ICaptchaSolverFactory> factories = descriptors.Select(x =>
            (ICaptchaSolverFactory)x.ImplementationFactory!.Invoke(mockServiceProvider.Object)).ToList();

        for (int i = 0; i < factories.Count; i++)
        {
            Assert.That(factories[i].SolverName, Is.EqualTo($"{nameof(TestProducer)}-{i}".ToLower()));
        }
    }

    [Test]
    public void AddCaptchaSolvers_With_Different_SolverNames_Is_Correct()
    {
        Mock<IServiceCollection> mock = new();
        Mock<IServiceProvider> mockServiceProvider = new();
        IList<ServiceDescriptor> descriptors = TestHelper.MakeEnumerableDescriptors(mock);

        mock.Object.AddCaptchaSolver<TestProducer>("solver-0");
        mock.Object.AddCaptchaSolver<TestProducer>("solver-1");

        List<ICaptchaSolverFactory> factories = descriptors.Select(x =>
            (ICaptchaSolverFactory)x.ImplementationFactory!.Invoke(mockServiceProvider.Object)).ToList();

        Assert.That(factories[0].SolverName, Is.EqualTo("solver-0"));
        Assert.That(factories[1].SolverName, Is.EqualTo("solver-1"));
    }

    [Test]
    public void AddCaptchaSolvers_With_Duplicate_SolverNames_Throws_InvalidOperationException()
    {
        Mock<IServiceCollection> mock = new();
        TestHelper.MakeEnumerableDescriptors(mock);

        mock.Object.AddCaptchaSolver<IProducer>("solver-0");
        Assert.Throws<InvalidOperationException>(() => mock.Object.AddCaptchaSolver<IProducer>("solver-0"));
    }

    [Test]
    public void AddFactoryToServiceCollection_Is_Correct()
    {
        Mock<IServiceCollection> mock = new();
        TestHelper.MakeEnumerableDescriptors(mock);

        Mock<IServiceProvider> providerMock = new();

        AvailableCaptchaAndSolutionStorage availableCaptchaAndSolutionStorage = new(
            new Dictionary<Type, Dictionary<Type, HashSet<string>>>
            {
                {
                    typeof(TestCaptcha), new Dictionary<Type, HashSet<string>>
                    {
                        { typeof(TestSolution), new HashSet<string>() }
                    }
                }
            });

        string expectedSolverName = "solver-0";

        mock.Object.AddSpecifiedCaptchaSolver<TestSpecifiedProducerBase>(
            builder =>
            {
                builder.AvailableCaptchaAndSolutionStorageBuilder.SetStorage(availableCaptchaAndSolutionStorage);
            }, expectedSolverName, It.IsAny<ServiceLifetime>());

        mock.Verify(x => x.Add(It.Is<ServiceDescriptor>((o, type) =>
            o is ServiceDescriptor &&
            ((ServiceDescriptor)o).ImplementationFactory!.Invoke(providerMock.Object) is CaptchaSolverFactory &&
            ((CaptchaSolverFactory)((ServiceDescriptor)o).ImplementationFactory!.Invoke(providerMock.Object))
            .SolverName == expectedSolverName &&
            ((CaptchaSolverFactory)((ServiceDescriptor)o).ImplementationFactory!.Invoke(providerMock.Object))
            .CanCreateSolver<ICaptcha, ISolution>(It.IsAny<string>()) == false &&
            ((CaptchaSolverFactory)((ServiceDescriptor)o).ImplementationFactory!.Invoke(providerMock.Object))
            .CanCreateSolver<TestCaptcha, TestSolution>(It.IsAny<string>()) == true
        )), Times.Once());
    }
}