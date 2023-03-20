using PassChallenge.Core.Handlers;
using PassChallenge.Core.Solutions;
using PassChallenge.Core.Tests.Tools;
using Moq;
using NUnit.Framework;
using PassChallenge.Core.Challenges;

namespace PassChallenge.Core.Tests.Handlers;

public class ChallengeHandlerDescriptorStorageBuilderTests
{
    [Test]
    public void AddChallengeHandler_With_Handler_And_Default_HandlerName_Is_Correct()
    {
        ChallengeHandlerDescriptorStorageBuilder builder = new();
        builder.AddChallengeHandler<TestChallenge, TestSolution, TestChallengeHandler<TestChallenge, TestSolution>>();

        IChallengeHandlerDescriptorAvailableStorage storage = builder.Build();

        ChallengeHandlerDescriptor descriptor = storage.GetDescriptor<TestChallenge, TestSolution>();

        Assert.Multiple(() =>
        {
            Assert.That(descriptor.ChallengeType, Is.EqualTo(typeof(TestChallenge)));
            Assert.That(descriptor.SolutionType, Is.EqualTo(typeof(TestSolution)));
            Assert.That(descriptor.HandlerType, Is.EqualTo(typeof(TestChallengeHandler<TestChallenge, TestSolution>)));
            Assert.That(descriptor.HandlerName,
                Is.EqualTo($"{nameof(TestChallenge)}-{nameof(TestSolution)}-0".ToLower()));
            Assert.IsNull(descriptor.ImplementationFactory);
            Assert.IsNull(descriptor.SolverFunction);
        });
    }

    [Test]
    public void AddChallengeHandler_With_Handler_And_Concrete_HandlerName_Is_Correct()
    {
        string expectedHandlerName = "handler-name";
        ChallengeHandlerDescriptorStorageBuilder builder = new();
        builder.AddChallengeHandler<TestChallenge, TestSolution, TestChallengeHandler<TestChallenge, TestSolution>>(
            expectedHandlerName);

        IChallengeHandlerDescriptorAvailableStorage storage = builder.Build();

        ChallengeHandlerDescriptor descriptor = storage.GetDescriptor<TestChallenge, TestSolution>();

        Assert.Multiple(() =>
        {
            Assert.That(descriptor.ChallengeType, Is.EqualTo(typeof(TestChallenge)));
            Assert.That(descriptor.SolutionType, Is.EqualTo(typeof(TestSolution)));
            Assert.That(descriptor.HandlerType, Is.EqualTo(typeof(TestChallengeHandler<TestChallenge, TestSolution>)));
            Assert.That(descriptor.HandlerName, Is.EqualTo(expectedHandlerName));
            Assert.IsNull(descriptor.ImplementationFactory);
            Assert.IsNull(descriptor.SolverFunction);
        });
    }

    [Test]
    public void AddChallengeHandler_With_Handler_Factory_And_Default_HandlerName_Is_Correct()
    {
        ChallengeHandlerDescriptorStorageBuilder builder = new();
        builder.AddChallengeHandler<TestChallenge, TestSolution, TestChallengeHandler<TestChallenge, TestSolution>>(_ =>
            new TestChallengeHandler<TestChallenge, TestSolution>());

        IChallengeHandlerDescriptorAvailableStorage storage = builder.Build();

        ChallengeHandlerDescriptor descriptor = storage.GetDescriptor<TestChallenge, TestSolution>();

        Assert.Multiple(() =>
        {
            Assert.That(descriptor.ChallengeType, Is.EqualTo(typeof(TestChallenge)));
            Assert.That(descriptor.SolutionType, Is.EqualTo(typeof(TestSolution)));
            Assert.That(descriptor.HandlerType, Is.EqualTo(typeof(TestChallengeHandler<TestChallenge, TestSolution>)));
            Assert.That(descriptor.HandlerName,
                Is.EqualTo($"{nameof(TestChallenge)}-{nameof(TestSolution)}-0".ToLower()));
            Assert.IsNotNull(descriptor.ImplementationFactory);
            Assert.IsNull(descriptor.SolverFunction);
        });
    }

    [Test]
    public void AddChallengeHandler_With_Handler_Factory_And_Concrete_HandlerName_Is_Correct()
    {
        string expectedHandlerName = "handler-name";
        ChallengeHandlerDescriptorStorageBuilder builder = new();
        builder.AddChallengeHandler<TestChallenge, TestSolution, TestChallengeHandler<TestChallenge, TestSolution>>(
            _ => new TestChallengeHandler<TestChallenge, TestSolution>(),
            expectedHandlerName);

        IChallengeHandlerDescriptorAvailableStorage storage = builder.Build();

        ChallengeHandlerDescriptor descriptor = storage.GetDescriptor<TestChallenge, TestSolution>();

        Assert.Multiple(() =>
        {
            Assert.That(descriptor.ChallengeType, Is.EqualTo(typeof(TestChallenge)));
            Assert.That(descriptor.SolutionType, Is.EqualTo(typeof(TestSolution)));
            Assert.That(descriptor.HandlerType, Is.EqualTo(typeof(TestChallengeHandler<TestChallenge, TestSolution>)));
            Assert.That(descriptor.HandlerName, Is.EqualTo(expectedHandlerName));
            Assert.IsNotNull(descriptor.ImplementationFactory);
            Assert.IsNull(descriptor.SolverFunction);
        });
    }

    [Test]
    public void AddChallengeHandler_With_Solver_Function_And_Default_HandlerName_Is_Correct()
    {
        ChallengeHandlerDescriptorStorageBuilder builder = new();
        builder.AddChallengeHandler<TestChallenge, TestSolution>((_, _) => Task.FromResult(It.IsAny<TestSolution>()));

        IChallengeHandlerDescriptorAvailableStorage storage = builder.Build();

        ChallengeHandlerDescriptor descriptor = storage.GetDescriptor<TestChallenge, TestSolution>();

        Assert.Multiple(() =>
        {
            Assert.That(descriptor.ChallengeType, Is.EqualTo(typeof(TestChallenge)));
            Assert.That(descriptor.SolutionType, Is.EqualTo(typeof(TestSolution)));
            Assert.IsNull(descriptor.HandlerType);
            Assert.That(descriptor.HandlerName,
                Is.EqualTo($"{nameof(TestChallenge)}-{nameof(TestSolution)}-0".ToLower()));
            Assert.IsNull(descriptor.ImplementationFactory);
            Assert.IsNotNull(descriptor.SolverFunction);
        });
    }

    [Test]
    public void AddChallengeHandler_With_Solver_Function_And_Concrete_HandlerName_Is_Correct()
    {
        string expectedHandlerName = "handler-name";
        ChallengeHandlerDescriptorStorageBuilder builder = new();
        builder.AddChallengeHandler<TestChallenge, TestSolution>((_, _) => Task.FromResult(It.IsAny<TestSolution>()),
            expectedHandlerName);

        IChallengeHandlerDescriptorAvailableStorage storage = builder.Build();

        ChallengeHandlerDescriptor descriptor = storage.GetDescriptor<TestChallenge, TestSolution>();

        Assert.Multiple(() =>
        {
            Assert.That(descriptor.ChallengeType, Is.EqualTo(typeof(TestChallenge)));
            Assert.That(descriptor.SolutionType, Is.EqualTo(typeof(TestSolution)));
            Assert.IsNull(descriptor.HandlerType);
            Assert.That(descriptor.HandlerName, Is.EqualTo(expectedHandlerName));
            Assert.IsNull(descriptor.ImplementationFactory);
            Assert.IsNotNull(descriptor.SolverFunction);
        });
    }


    [Test]
    public void AddChallengeHandler_With_Multiple_Handlers_And_Default_HandlerName_Is_Correct()
    {
        ChallengeHandlerDescriptorStorageBuilder builder = new();

        builder.AddChallengeHandler<TestChallenge, TestSolution, TestChallengeHandler<TestChallenge, TestSolution>>();
        builder.AddChallengeHandler<TestChallenge, TestSolution, TestChallengeHandler<TestChallenge, TestSolution>>();

        IChallengeHandlerDescriptorAvailableStorage storage = builder.Build();

        IReadOnlyList<ChallengeHandlerDescriptor> descriptors =
            storage.GetDescriptors<TestChallenge, TestSolution>().ToList();

        Assert.That(descriptors.Count, Is.EqualTo(2));

        for (int i = 0; i < descriptors.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(descriptors[i].ChallengeType, Is.EqualTo(typeof(TestChallenge)));
                Assert.That(descriptors[i].SolutionType, Is.EqualTo(typeof(TestSolution)));
                Assert.That(descriptors[i].HandlerType,
                    Is.EqualTo(typeof(TestChallengeHandler<TestChallenge, TestSolution>)));
                Assert.That(descriptors[i].HandlerName,
                    Is.EqualTo($"{nameof(TestChallenge)}-{nameof(TestSolution)}-{i}".ToLower()));
                Assert.IsNull(descriptors[i].ImplementationFactory);
                Assert.IsNull(descriptors[i].SolverFunction);
            });
        }
    }

    [Test]
    public void AddChallengeHandler_With_Multiple_Handlers_And_Repeated_HandlerName_Throws_InvalidOperationException()
    {
        string expectedHandlerName = "handler-name";
        ChallengeHandlerDescriptorStorageBuilder builder = new();

        builder.AddChallengeHandler<TestChallenge, TestSolution, TestChallengeHandler<TestChallenge, TestSolution>>(
            expectedHandlerName);
        Assert.Throws<InvalidOperationException>(() =>
            builder.AddChallengeHandler<TestChallenge, TestSolution, TestChallengeHandler<TestChallenge, TestSolution>>(
                expectedHandlerName));
    }

    [Test]
    public void AddChallengeHandler_With_Handler_Is_Interface_Throws_ArgumentException()
    {
        string expectedHandlerName = "handler-name";
        ChallengeHandlerDescriptorStorageBuilder builder = new();
        Assert.Throws<ArgumentException>(() =>
            builder.AddChallengeHandler<IChallenge, ISolution, IChallengeHandler<IChallenge, ISolution>>(
                expectedHandlerName));
    }

    [Test]
    public void AddChallengeHandler_With_HandlerFactory_And_Handler_Is_Correct()
    {
        string expectedHandlerName = "handler-name";
        ChallengeHandlerDescriptorStorageBuilder builder = new();
        Assert.DoesNotThrow(() =>
            builder.AddChallengeHandler<IChallenge, ISolution, IChallengeHandler<IChallenge, ISolution>>(_ =>
                It.IsAny<IChallengeHandler<IChallenge, ISolution>>(), expectedHandlerName));
    }
}