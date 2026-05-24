
using MediatR;

namespace BuildingBlocks.CQRS;

public interface IQuerryHandler<in TQuerry, TResponse> : IRequestHandler<TQuerry, TResponse>
    where TQuerry : IQuerry<TResponse>
    where TResponse : notnull
{
}
