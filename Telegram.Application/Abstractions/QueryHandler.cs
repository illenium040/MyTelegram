using Telegram.Domain.Abstractions;
using Telegram.Domain.Shared;

namespace Telegram.Application.Abstractions;

internal abstract class QueryHandler
{
    protected IUnitOfWork UnitOfWork { get; }
    public QueryHandler(IUnitOfWork unitOfWork) => UnitOfWork = unitOfWork;
}

internal abstract class QueryHandler<TQuery, TResponse> : QueryHandler, IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    protected QueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public abstract Task<Result<TResponse>> Handle(TQuery request, CancellationToken cancellationToken);
}

internal abstract class QueryHandler<TQuery> : QueryHandler, IQueryHandler<TQuery>
    where TQuery : IQuery
{
    protected QueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public abstract Task<Result> Handle(TQuery request, CancellationToken cancellationToken);
}
