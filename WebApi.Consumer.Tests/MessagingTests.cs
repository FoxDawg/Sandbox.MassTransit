using System;
using System.Threading.Tasks;
using FluentAssertions;
using MassTransit;
using MassTransit.Testing;
using Messaging.Contract.Events;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace WebApi.Consumer.Tests;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureServices(services => services.AddMassTransitTestHarness());
    }
}

public class MessagingTests : IAsyncDisposable
{
    private readonly CustomWebApplicationFactory applicationFactory;

    public MessagingTests()
    {
        this.applicationFactory = new CustomWebApplicationFactory();
    }
    [Fact]
    public async Task Receives_Heartbeat()
    {
        // Arrange
        var testHarness = this.applicationFactory.Services.GetService<ITestHarness>()!;
        var messageBus = this.applicationFactory.Services.GetService<IBus>()!;
        
        // Act
        await messageBus.Publish(new HeartBeat());
        
        // Assert
        var publishedHeartbeat = await testHarness.Published.Any<HeartBeat>();
        publishedHeartbeat.Should().BeTrue();
        await Task.Delay(TimeSpan.FromSeconds(0.5));
        var consumedHeartBeat = await testHarness.Consumed.Any<HeartBeat>();
        consumedHeartBeat.Should().BeTrue();
    }

    public async ValueTask DisposeAsync()
    {
        await this.applicationFactory.DisposeAsync();
    }
}