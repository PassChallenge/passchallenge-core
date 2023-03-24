using PassChallenge.Core.Extensions;
using PassChallenge.Core.Handlers;
using PassChallenge.Core.Producer;
using PassChallenge.Core.Solutions;
using PassChallenge.Core.Solver;
using PassChallenge.Core.Tests.Tools;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using PassChallenge.Core.Challenges;

namespace PassChallenge.Core.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Test]
    [TestCase(ServiceLifetime.Transient)]
    [TestCase(ServiceLifetime.Scoped)]
    [TestCase(ServiceLifetime.Singleton)]
    public void AddChallengeSolver_DefaultSolver_ServiceCollection_AddMethod_Is_Called(ServiceLifetime lifetime)
    {
        Mock<IServiceCollection> mock = new();
        TestHelper.MakeEnumerableDescriptors(mock);

        mock.Object.AddChallengeSolver<TestProducer>(It.IsAny<string>(), lifetime);

        mock.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Once());
        mock.Verify(
            x => x.Add(It.Is<ServiceDescriptor>(descriptor =>
                descriptor.ServiceType == typeof(IChallengeSolverFactory) &&
                descriptor.Lifetime == lifetime)), Times.Once());
    }

    [Test]
    public void AddChallengeSolver_DefaultSolver_When_Solver_Is_Interface_Throws_ArgumentException()
    {
        Mock<IServiceCollection> mock = new();
        Assert.Throws<ArgumentException>(() => mock.Object.AddChallengeSolver<IProducer>());
    }

    [Test]
    public void AddChallengeSolver_DefaultSolver_When_ServiceCollection_Is_Null_Throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ChallengeSolverExtensions.AddChallengeSolver<TestProducer>(null!));
    }

    [Test]
    [TestCase(ServiceLifetime.Transient)]
    [TestCase(ServiceLifetime.Scoped)]
    [TestCase(ServiceLifetime.Singleton)]
    public void AddChallengeSolver_ServiceCollection_AddMethod_Is_Called(
        ServiceLifetime lifetime)
    {
        Mock<IServiceCollection> mock = new();
        TestHelper.MakeEnumerableDescriptors(mock);

        mock.Object.AddChallengeSolver<TestProducer>(builder =>
        {
            builder.AvailableChallengeAndSolutionStorageBuilder.AddSupportChallengeAndSolution(
                ChallengeHandlerDescriptor.Create<IChallenge, ISolution>((_, _) =>
                    It.IsAny<Task<ISolution>>()));
        }, It.IsAny<string>(), lifetime);

        mock.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Once());
        mock.Verify(
            x => x.Add(It.Is<ServiceDescriptor>(descriptor =>
                descriptor.ServiceType == typeof(IChallengeSolverFactory) &&
                descriptor.Lifetime == lifetime)), Times.Once());
    }

    [Test]
    public void AddSolver_DefaultSolver_When_ServiceCollection_Is_Null_Throws_ArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            ChallengeSolverExtensions.AddChallengeSolver<TestProducer>(
                null!, _ => { }));
    }

    [Test]
    public void AddChallengeSolver_With_Builder_When_ServiceCollection_Is_Null_Throws_ArgumentNullException()
    {
        Mock<IServiceCollection> mock = new();
        Action<ChallengeSolverBuilder<TestProducer>> configure = null!;
        Assert.Throws<ArgumentNullException>(() =>
            mock.Object.AddChallengeSolver(configure));
    }

    [Test]
    public void AddChallengeSolver_With_Builder_When_Solver_Is_Interface_Throws_ArgumentException()
    {
        Mock<IServiceCollection> mock = new();
        Action<ChallengeSolverBuilder<IProducer>> configure = null!;
        Assert.Throws<ArgumentException>(() => mock.Object.AddChallengeSolver(configure));
    }

    [Test]
    public void AddChallengeSolver_With_Empty_Builder_Is_Correct()
    {
        Mock<IServiceCollection> mock = new();
        TestHelper.MakeEnumerableDescriptors(mock);

        mock.Object.AddChallengeSolver<TestProducer>(It.IsAny<string>(), It.IsAny<ServiceLifetime>());
        mock.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Once());
    }

    [Test]
    public void AddChallengeSolver_With_Empty_Builder_Default_ServiceLifetime_Is_Correct()
    {
        Mock<IServiceCollection> mock = new();
        TestHelper.MakeEnumerableDescriptors(mock);

        mock.Object.AddChallengeSolver<TestProducer>();
        mock.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Once());
    }

    [Test]
    public void AddChallengeSolver_With_Builder_Is_Correct()
    {
        Mock<IServiceCollection> mock = new();
        TestHelper.MakeEnumerableDescriptors(mock);

        mock.Object.AddChallengeSolver<TestProducer>(_ => { }, It.IsAny<string>(), It.IsAny<ServiceLifetime>());
        mock.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Once());
    }

    [Test]
    public void AddChallengeSolver_With_Builder_When_Builder_Is_Null_Throws_ArgumentNullException()
    {
        Mock<IServiceCollection> mock = new();
        Assert.Throws<ArgumentNullException>(
            () => mock.Object.AddChallengeSolver<TestProducer>(null!, It.IsAny<string>()));
    }

    [Test]
    public void AddChallengeSolvers_With_Default_SolverNames_Is_Correct()
    {
        Mock<IServiceCollection> mock = new();
        Mock<IServiceProvider> mockServiceProvider = new();
        IList<ServiceDescriptor> descriptors = TestHelper.MakeEnumerableDescriptors(mock);

        mock.Object.AddChallengeSolver<TestProducer>();
        mock.Object.AddChallengeSolver<TestProducer>();

        List<IChallengeSolverFactory> factories = descriptors.Select(x =>
            (IChallengeSolverFactory)x.ImplementationFactory!.Invoke(mockServiceProvider.Object)).ToList();

        for (int i = 0; i < factories.Count; i++)
        {
            Assert.That(factories[i].SolverName, Is.EqualTo($"{nameof(TestProducer)}-{i}".ToLower()));
        }
    }

    [Test]
    public void AddChallengeSolvers_With_Different_SolverNames_Is_Correct()
    {
        Mock<IServiceCollection> mock = new();
        Mock<IServiceProvider> mockServiceProvider = new();
        IList<ServiceDescriptor> descriptors = TestHelper.MakeEnumerableDescriptors(mock);

        mock.Object.AddChallengeSolver<TestProducer>("solver-0");
        mock.Object.AddChallengeSolver<TestProducer>("solver-1");

        List<IChallengeSolverFactory> factories = descriptors.Select(x =>
            (IChallengeSolverFactory)x.ImplementationFactory!.Invoke(mockServiceProvider.Object)).ToList();

        Assert.That(factories[0].SolverName, Is.EqualTo("solver-0"));
        Assert.That(factories[1].SolverName, Is.EqualTo("solver-1"));
    }

    [Test]
    public void AddChallengeSolvers_With_Duplicate_SolverNames_Throws_InvalidOperationException()
    {
        Mock<IServiceCollection> mock = new();
        TestHelper.MakeEnumerableDescriptors(mock);

        mock.Object.AddChallengeSolver<TestProducer>("solver-0");
        Assert.Throws<InvalidOperationException>(() => mock.Object.AddChallengeSolver<TestProducer>("solver-0"));
    }

    [Test]
    public void AddFactoryToServiceCollection_Is_Correct()
    {
        Mock<IServiceCollection> mock = new();
        TestHelper.MakeEnumerableDescriptors(mock);

        Mock<IServiceProvider> providerMock = new();

        AvailableChallengeAndSolutionStorage availableChallengeAndSolutionStorage = new(
            new Dictionary<Type, Dictionary<Type, HashSet<string>>>
            {
                {
                    typeof(TestChallenge), new Dictionary<Type, HashSet<string>>
                    {
                        { typeof(TestSolution), new HashSet<string>() }
                    }
                }
            });

        string expectedSolverName = "solver-0";

        mock.Object.AddChallengeSolver<TestProducer>(
            builder =>
            {
                builder.AvailableChallengeAndSolutionStorageBuilder.SetStorage(availableChallengeAndSolutionStorage);
                builder.AvailableChallengeAndSolutionStorageBuilder
                    .AddSupportChallengeAndSolution<TestChallenge, TestSolution>();
            }, expectedSolverName, It.IsAny<ServiceLifetime>());

        mock.Verify(x => x.Add(It.Is<ServiceDescriptor>((o, type) =>
            o is ServiceDescriptor &&
            ((ServiceDescriptor)o).ImplementationFactory!.Invoke(providerMock.Object) is ChallengeSolverFactory &&
            ((ChallengeSolverFactory)((ServiceDescriptor)o).ImplementationFactory!.Invoke(providerMock.Object))
            .SolverName == expectedSolverName &&
            ((ChallengeSolverFactory)((ServiceDescriptor)o).ImplementationFactory!.Invoke(providerMock.Object))
            .CanCreateSolver<IChallenge, ISolution>(It.IsAny<string>()) == false &&
            ((ChallengeSolverFactory)((ServiceDescriptor)o).ImplementationFactory!.Invoke(providerMock.Object))
            .CanCreateSolver<TestChallenge, TestSolution>(It.IsAny<string>()) == true
        )), Times.Once());
    }
}