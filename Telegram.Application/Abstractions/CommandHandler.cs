using Telegram.Domain.Abstractions;
using Telegram.Domain.Shared;

namespace Telegram.Application.Abstractions
{
    internal abstract class CommandHandler
    {
        protected IUnitOfWork UnitOfWork { get; }
        public CommandHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }

    internal abstract class CommandHandler<TCommand, TResponse> : CommandHandler, ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        protected CommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public abstract Task<Result<TResponse>> Handle(TCommand request, CancellationToken cancellationToken);
    }

    internal abstract class CommandHandler<TCommand> : CommandHandler, ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        protected CommandHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public abstract Task<Result> Handle(TCommand request, CancellationToken cancellationToken);
    }
}
