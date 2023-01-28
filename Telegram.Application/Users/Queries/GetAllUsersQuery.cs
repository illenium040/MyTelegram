using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Application.Abstractions;
using Telegram.Domain.Entities;

namespace Telegram.Application.Users.Queries
{
    public sealed record GetAllUsersQuery() : IQuery<IEnumerable<User?>>;
}
