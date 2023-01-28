using Telegram.Domain.Abstractions;
using Telegram.Domain.ValueObjects;

namespace Telegram.Domain.DomainEvents;

public record UserCreatedDomainEvent(Guid Id, Login Login) : IDomainEvent;
