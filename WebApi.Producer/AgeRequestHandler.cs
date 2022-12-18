using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.AmazonSqsTransport;
using Messaging.Contract.Commands;
using Messaging.Contract.Responses;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApi.Producer;

public class AgeRequestHandler : IHostedService
{
    private readonly ILogger<AgeRequestHandler> logger;
    private readonly IBus messageBus;
    private Task task;

    public AgeRequestHandler(IBus messageBus, ILogger<AgeRequestHandler> logger)
    {
        this.messageBus = messageBus;
        this.logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.task = Task.Run(async () =>
        {
            while (cancellationToken.IsCancellationRequested == false)
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                    logger.LogInformation("Sending age request");
                    var response = await messageBus.Request<TellMeYourAge, AgeResponse>(new TellMeYourAge(),
                        cancellationToken, default, ctx =>
                        {
                            if (ctx is AmazonSqsMessageSendContext<TellMeYourAge> amazonCtx)
                            {
                                amazonCtx.GroupId = nameof(TellMeYourAge);
                                amazonCtx.DeduplicationId = ctx.Message.Identifier.ToString();
                            }
                        });
                    logger.LogInformation("Received age response {Timestamp}", response.Message.Age);
                    
                }
                catch (TaskCanceledException)
                {
                    logger.LogWarning("Task cancelled");
                }
                catch (RequestTimeoutException)
                {
                    logger.LogWarning("No one responded");
                }
        }, cancellationToken);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await this.task;
    }
}