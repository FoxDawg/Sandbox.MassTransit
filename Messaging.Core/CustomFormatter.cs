using System;
using System.Linq;
using MassTransit;

namespace Messaging.Infrastructure;

public class CustomFormatter : IEndpointNameFormatter, IEntityNameFormatter
{
    private readonly IEndpointNameFormatter defaultFormatter;

    public CustomFormatter(IEndpointNameFormatter defaultFormatter)
    {
        this.defaultFormatter = defaultFormatter;
    }

    public string TemporaryEndpoint(string tag)
    {
        return defaultFormatter.TemporaryEndpoint(tag);
    }

    public string Consumer<T>() where T : class, IConsumer
    {
        var defaultName = defaultFormatter.Consumer<T>();
        // Please give this a bit more thought. This is just to make a point.
        var type = typeof(T).GetInterfaces().First().GenericTypeArguments.First();

        if (type.Namespace.Contains("command", StringComparison.OrdinalIgnoreCase))
        {
            return defaultName + ".fifo";
        }

        return defaultName;
    }

    public string FormatEntityName<T>()
    {
        var type = typeof(T);

        if (type.Namespace.Contains("command", StringComparison.OrdinalIgnoreCase))
        {
            return type.Name + ".fifo";
        }
        return type.Name;
    }

    public string Message<T>() where T : class
    {
        return defaultFormatter.Message<T>();
    }

    public string Saga<T>() where T : class, ISaga
    {
        return defaultFormatter.Saga<T>();
    }

    public string ExecuteActivity<T, TArguments>() where T : class, IExecuteActivity<TArguments> where TArguments : class
    {
        return defaultFormatter.ExecuteActivity<T, TArguments>();
    }

    public string CompensateActivity<T, TLog>() where T : class, ICompensateActivity<TLog> where TLog : class
    {
        return defaultFormatter.CompensateActivity<T, TLog>();
    }

    public string SanitizeName(string name)
    {
        return name;
    }

    public string Separator { get; }
}