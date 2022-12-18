using Messaging.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();

builder.Services.ConfigureMassTransit(typeof(Program).Assembly);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

