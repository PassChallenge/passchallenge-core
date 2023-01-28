using System;
using System.Linq;
using System.Reflection;
using KillDNS.CaptchaSolver.Core.Producer;
using Microsoft.Extensions.DependencyInjection;

namespace KillDNS.CaptchaSolver.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCaptchaSolver<TProducer>(this IServiceCollection serviceCollection,
        Action<CaptchaSolverBuilder<TProducer>> configure, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TProducer : IProducer
    {
        CaptchaSolverBuilder<TProducer> builder = new(serviceCollection);
        configure.Invoke(builder);

        serviceCollection.Add(new ServiceDescriptor(typeof(ICaptchaSolverFactory), provider =>
            new CaptchaSolverFactory(CreateProducer(provider)), lifetime));

        return serviceCollection;

        IProducer CreateProducer(IServiceProvider provider)
        {
            var parameters = typeof(TProducer).GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0]
                .GetParameters()
                .Select(x => provider.GetRequiredService(x.ParameterType)).ToArray();

            IProducer producer = (IProducer)Activator.CreateInstance(typeof(TProducer), parameters);

            if (producer is IProducerWithSpecifiedCaptchaAndSolutions specifiedProducer)
            {
                specifiedProducer.SetAvailableCaptchaAndSolutionTypes(builder.AvailableCaptchaAndSolutionTypes);
            }

            foreach (var configureProducerAction in builder.ConfigureProducerActions)
            {
                configureProducerAction?.Invoke((TProducer)producer);
            }

            return producer;
        }
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