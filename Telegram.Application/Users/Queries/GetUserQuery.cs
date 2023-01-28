using Telegram.Application.Abstractions;
using Telegram.Domain.Entities;

namespace Telegram.Application.Users.Queries;

public sealed record GetUserQuery(
    string? Login = null,
    string? DisplayName = null
    ) : IQuery<User>;
