using System;

namespace Messaging.Contract.Commands;

public record TellMeYourAge
{
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    public Guid Identifier { get; init; } = Guid.NewGuid();
}