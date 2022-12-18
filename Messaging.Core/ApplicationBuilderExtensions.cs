using System;
using System.Reflection;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureMassTransit(this IServiceCollection services, Assembly consumerAssembly)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumers(consumerAssembly);
            busConfigurator.UsingAmazonSqs((context, config) =>
            {
                config.Host(new Uri("amazonsqs://localhost:4566"), h =>
                {
                    h.AccessKey("test");
                    h.SecretKey("test");
                    h.Config(new AmazonSimpleNotificationServiceConfig { ServiceURL = "http://localhost:4566" });
                    h.Config(new AmazonSQSConfig { ServiceURL = "http://localhost:4566" });
                });

                var formatter = new CustomFormatter(new DefaultEndpointNameFormatter(false));
                config.MessageTopology.SetEntityNameFormatter(formatter);
                config.ConfigureEndpoints(context, formatter);
            });
        });
        
        return services;
    }
}