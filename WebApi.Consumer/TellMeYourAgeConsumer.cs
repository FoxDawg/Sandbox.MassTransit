using System;
using System.Threading.Tasks;
using MassTransit;
using Messaging.Contract.Commands;
using Messaging.Contract.Responses;
using Microsoft.Extensions.Logging;

namespace WebApi.Consumer;

public class TellMeYourAgeConsumer : IConsumer<TellMeYourAge>
{
    private readonly ILogger<TellMeYourAgeConsumer> logger;

    public TellMeYourAgeConsumer(ILogger<TellMeYourAgeConsumer> logger)
    {
        this.logger = logger;
    }
    public async Task Consume(ConsumeContext<TellMeYourAge> context)
    {
        this.logger.LogInformation("Received age request");
        var age = TimeSpan.FromMilliseconds(Environment.TickCount);
        await context.RespondAsync(new AgeResponse(age));
    }
}

