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
        mock.Setup(x => x.Add(It.IsAny<ServiceDescriptor>()));

        mock.Object.AddCaptchaSolver<IProducer>(lifetime);

        mock.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Once());
        mock.Verify(
            x => x.Add(It.Is<ServiceDescriptor>(descriptor =>
                descriptor.ServiceType == typeof(ICaptchaSolverFactory) &&
                descriptor.Lifetime == lifetime)), Times.Once());
    }

    [Test]
    [TestCase(ServiceLifetime.Transient)]
    [TestCase(ServiceLifetime.Scoped)]
    [TestCase(ServiceLifetime.Singleton)]
    public void AddSpecifiedCaptchaSolver_SpecifiedSolver_ServiceCollection_AddMethod_Is_Called(
        ServiceLifetime lifetime)
    {
        Mock<IServiceCollection> mock = new();
        mock.Setup(x => x.Add(It.IsAny<ServiceDescriptor>()));

        mock.Object.AddSpecifiedCaptchaSolver<IProducerWithSpecifiedCaptchaAndSolutions>(builder =>
        {
            builder.AvailableCaptchaAndSolutionStorageBuilder.AddSupportCaptchaAndSolution(
                CaptchaHandlerDescriptor.Create<ICaptcha, ISolution>((_, _) =>
                    It.IsAny<Task<ISolution>>()));
        }, lifetime);

        mock.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Once());
        mock.Verify(
            x => x.Add(It.Is<ServiceDescriptor>(descriptor =>
                descriptor.ServiceType == typeof(ICaptchaSolverFactory) &&
                descriptor.Lifetime == lifetime)), Times.Once());
    }

    [Test]
    public void AddCaptchaSolver_With_Empty_Builder_Is_Correct()
    {
        Mock<IServiceCollection> mock = new();
        mock.Object.AddCaptchaSolver<IProducer>(It.IsAny<ServiceLifetime>());
        mock.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Once());
    }

    [Test]
    public void AddCaptchaSolver_With_Empty_Builder_Default_ServiceLifetime_Is_Correct()
    {
        Mock<IServiceCollection> mock = new();
        mock.Object.AddCaptchaSolver<IProducer>();
        mock.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Once());
    }

    [Test]
    public void AddCaptchaSolver_With_Builder_Is_Correct()
    {
        Mock<IServiceCollection> mock = new();
        mock.Object.AddCaptchaSolver<IProducer>(_ => { }, It.IsAny<ServiceLifetime>());
        mock.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Once());
    }

    [Test]
    public void AddFactoryToServiceCollection_Is_Correct()
    {
        Mock<IServiceCollection> mock = new();
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

        mock.Object.AddSpecifiedCaptchaSolver<TestSpecifiedProducerBase>(
            builder =>
            {
                builder.AvailableCaptchaAndSolutionStorageBuilder.SetStorage(availableCaptchaAndSolutionStorage);
            }, It.IsAny<ServiceLifetime>());

        mock.Verify(x => x.Add(It.Is<ServiceDescriptor>((o, type) =>
            o is ServiceDescriptor &&
            ((ServiceDescriptor)o).ImplementationFactory!.Invoke(providerMock.Object) is CaptchaSolverFactory &&
            ((CaptchaSolverFactory)((ServiceDescriptor)o).ImplementationFactory!.Invoke(providerMock.Object))
            .CanCreateSolver<ICaptcha, ISolution>(It.IsAny<string>()) == false &&
            ((CaptchaSolverFactory)((ServiceDescriptor)o).ImplementationFactory!.Invoke(providerMock.Object))
            .CanCreateSolver<TestCaptcha, TestSolution>(It.IsAny<string>()) == true
        )), Times.Once());
    }
}