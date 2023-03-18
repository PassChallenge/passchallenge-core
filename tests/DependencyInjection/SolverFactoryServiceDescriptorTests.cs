using KillDNS.CaptchaSolver.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace KillDNS.CaptchaSolver.Core.Tests.DependencyInjection;

public class SolverFactoryServiceDescriptorTests
{
    [Test]
    public void Constructor_ServiceType_Instance_SolverName_Is_Correct()
    {
        Type expectedServiceType = typeof(string);
        object expectedInstance = "instance";
        string expectedSolverName = "solver-0";

        SolverFactoryServiceDescriptor solverFactoryServiceDescriptor =
            new(expectedServiceType, expectedInstance, expectedSolverName);

        ServiceDescriptor serviceDescriptor = solverFactoryServiceDescriptor;

        Assert.That(serviceDescriptor.ServiceType, Is.EqualTo(expectedServiceType));
        Assert.That(serviceDescriptor.ImplementationInstance, Is.EqualTo(expectedInstance));

        Assert.That(solverFactoryServiceDescriptor.SolverName, Is.EqualTo(expectedSolverName));
    }

    [Test]
    public void Constructor_ServiceType_ImplementationType_Lifetime_SolverName_Is_Correct()
    {
        Type expectedServiceType = typeof(string);
        Type expectedImplementationType = typeof(int);
        ServiceLifetime expectedServiceLifetime = ServiceLifetime.Transient;
        string expectedSolverName = "solver-0";

        SolverFactoryServiceDescriptor solverFactoryServiceDescriptor =
            new(expectedServiceType, expectedImplementationType, expectedServiceLifetime, expectedSolverName);

        ServiceDescriptor serviceDescriptor = solverFactoryServiceDescriptor;

        Assert.That(serviceDescriptor.ServiceType, Is.EqualTo(expectedServiceType));
        Assert.That(serviceDescriptor.ImplementationType, Is.EqualTo(expectedImplementationType));
        Assert.That(serviceDescriptor.Lifetime, Is.EqualTo(expectedServiceLifetime));

        Assert.That(solverFactoryServiceDescriptor.SolverName, Is.EqualTo(expectedSolverName));
    }

    [Test]
    public void Constructor_ServiceType_Factory_Lifetime_SolverName_Is_Correct()
    {
        Type expectedServiceType = typeof(string);
        // ReSharper disable once ConvertToLocalFunction
        Func<IServiceProvider, object> expectedFactory = _ => "test";
        ServiceLifetime expectedServiceLifetime = ServiceLifetime.Transient;
        string expectedSolverName = "solver-0";

        SolverFactoryServiceDescriptor solverFactoryServiceDescriptor =
            new(expectedServiceType, expectedFactory, expectedServiceLifetime, expectedSolverName);

        ServiceDescriptor serviceDescriptor = solverFactoryServiceDescriptor;

        Assert.That(serviceDescriptor.ServiceType, Is.EqualTo(expectedServiceType));
        Assert.That(serviceDescriptor.ImplementationFactory, Is.EqualTo(expectedFactory));
        Assert.That(serviceDescriptor.Lifetime, Is.EqualTo(expectedServiceLifetime));

        Assert.That(solverFactoryServiceDescriptor.SolverName, Is.EqualTo(expectedSolverName));
    }
}