namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdQuerry(Guid Id) : IQuerry<GetProductByIdResult>;

    public record GetProductByIdResult(Product product);

    internal class GetProductByIdHandler
        (IDocumentSession session)
        : IQuerryHandler<GetProductByIdQuerry, GetProductByIdResult>
    {

        public async Task<GetProductByIdResult> Handle(GetProductByIdQuerry request, CancellationToken cancellationToken)
        {
            //var productFromDb = await session.Query<Product>().SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            var product = await session.LoadAsync<Product>(request.Id, cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException(request.Id);
            }
            return new GetProductByIdResult(product);
        }
    }
}
