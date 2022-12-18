using Messaging.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApi.Producer;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();

builder.Services.ConfigureMassTransit(typeof(Program).Assembly);
builder.Services.AddHostedService<HeartBeatSender>();
builder.Services.AddHostedService<AgeRequestHandler>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
