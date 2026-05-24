namespace Catalog.API.Products.GetProduct
{
    public record GetProductsQuerry() : IQuerry<GetProductsResult>;

    public record GetProductsResult(IEnumerable<Product> products);

    internal class GetProductsHandler
        (IDocumentSession session, ILogger<GetProductsHandler> logger)
        : IQuerryHandler<GetProductsQuerry, GetProductsResult>
    {

        public async Task<GetProductsResult> Handle(GetProductsQuerry request, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductsQueryHandler called with {@Query}", request);
            var productsFromDb = await session.Query<Product>().ToListAsync(cancellationToken);
            return new GetProductsResult(productsFromDb);
        }
    }
}
