using MediatR;
using Telegram.Domain.Shared;

namespace Telegram.Application.Abstractions;

public interface IQuery : IRequest<Result>
{
}

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
