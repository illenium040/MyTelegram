using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Application.Abstractions;
using Telegram.Domain.Abstractions;
using Telegram.Domain.Entities;
using Telegram.Domain.Shared;
using Telegram.Domain.ValueObjects;
using Telegram.Infrastructure.Abstractions;

namespace Telegram.Application.Users.Queries.Handlers
{
    internal class GetUserByIdQueryHandler : QueryHandler<GetUserByIdQuery, User>
    {
        private readonly IUserRepository _userRepository;
        public GetUserByIdQueryHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _userRepository = userRepository;
        }

        public override async Task<Result<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var res = await _userRepository.GetByIdAsync(request.Id);

            if (res is null) return Result.Failure<User>(new Error("GetUser", "User not found"));

            return Result.Success(res);
        }
    }
}
