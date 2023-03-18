using System;
using KillDNS.CaptchaSolver.Core.Producer;
using KillDNS.CaptchaSolver.Core.Solver;
using Microsoft.Extensions.DependencyInjection;

namespace KillDNS.CaptchaSolver.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCaptchaSolver<TProducer>(this IServiceCollection serviceCollection,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TProducer : class, IProducer
    {
        if (serviceCollection == null) 
            throw new ArgumentNullException(nameof(serviceCollection));
        
        return AddCaptchaSolver<TProducer>(serviceCollection, _ => { }, lifetime);
    }

    public static IServiceCollection AddCaptchaSolver<TProducer>(this IServiceCollection serviceCollection,
        Action<CaptchaSolverBuilder<TProducer>> configure, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TProducer : class, IProducer
    {
        if (serviceCollection == null) 
            throw new ArgumentNullException(nameof(serviceCollection));
        
        if (configure == null) 
            throw new ArgumentNullException(nameof(configure));
        
        CaptchaSolverBuilder<TProducer> builder = new();
        configure.Invoke(builder);

        AddFactoryToServiceCollection(serviceCollection, builder, lifetime);

        return serviceCollection;
    }

    public static IServiceCollection AddSpecifiedCaptchaSolver<TProducer>(this IServiceCollection serviceCollection,
        Action<CaptchaSolverSpecifiedBuilder<TProducer>> configure, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TProducer : class, IProducerWithSpecifiedCaptchaAndSolutions
    {
        if (serviceCollection == null) 
            throw new ArgumentNullException(nameof(serviceCollection));
        
        if (configure == null) 
            throw new ArgumentNullException(nameof(configure));
        
        CaptchaSolverSpecifiedBuilder<TProducer> builder = new();
        configure.Invoke(builder);

        AddFactoryToServiceCollection(serviceCollection, builder, lifetime);

        return serviceCollection;
    }

    private static void AddFactoryToServiceCollection<TProducer>(IServiceCollection serviceCollection,
        CaptchaSolverBuilder<TProducer> builder, ServiceLifetime lifetime)
        where TProducer : class, IProducer
    {
        serviceCollection.Add(new ServiceDescriptor(typeof(ICaptchaSolverFactory), provider =>
            new CaptchaSolverFactory(builder.Build(provider)), lifetime));
    }


    /*public static IServiceCollection AddCaptchaResolver(this IServiceCollection serviceCollection,
        Action<CaptchaResolverBuilder> configure)
    {
        CaptchaResolverBuilder builder = new(serviceCollection);
        configure.Invoke(builder);

        foreach (Type handlerType in builder.MessageHandlers)
        {
            var f=handlerType.GetInterfaces();
            var handlerInterfaces = handlerType.GetInterfaces().Where(x => x.GetGenericTypeDefinition() == typeof(ICaptchaHandler<,>));

            foreach (var handlerInterface in handlerInterfaces)
            {
                serviceCollection.AddScoped(handlerInterface, handlerType);
            }
        }

        serviceCollection.AddHostedService<ResolverHostedService>();
        return serviceCollection;
    }*/
}