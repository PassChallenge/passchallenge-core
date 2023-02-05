using System;
using KillDNS.CaptchaSolver.Core.Producer;
using KillDNS.CaptchaSolver.Core.Solver;
using Microsoft.Extensions.DependencyInjection;

namespace KillDNS.CaptchaSolver.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCaptchaSolver<TProducer>(this IServiceCollection serviceCollection,
        Action<CaptchaSolverBuilder<TProducer>> configure, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TProducer : IProducer
    {
        CaptchaSolverBuilder<TProducer> builder = new();
        configure.Invoke(builder);

        serviceCollection.Add(new ServiceDescriptor(typeof(ICaptchaSolverFactory), provider =>
            new CaptchaSolverFactory(builder.Build(provider)), lifetime));

        return serviceCollection;
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