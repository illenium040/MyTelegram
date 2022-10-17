using MediatR;
using Telegram.Domain.Shared;

namespace Telegram.Application.Abstractions
{
    public interface ICommand : IRequest<Result>
    {
    }
    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {

    }
}
