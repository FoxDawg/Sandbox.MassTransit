using System;

namespace Messaging.Contract.Events;

public sealed record HeartBeat
{
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    public Guid Identifier { get; init; } = Guid.NewGuid();
}