using Telegram.Application.Abstractions;
using Telegram.Domain.Entities;

namespace Telegram.Application.Users.Queries;

public sealed record GetAllUsersQuery() : IQuery<IEnumerable<User?>>;
