namespace Catalog.API.Products.GetProductsByCategory
{
    public record GetProductsByCategoryQuerry(string Category) : IQuerry<GetProductsByCategoryResult>;

    public record GetProductsByCategoryResult(IEnumerable<Product> products);
    internal class GetProductsByCategoryHandler
        (IDocumentSession session, ILogger<GetProductsByCategoryHandler> logger)
        : IQuerryHandler<GetProductsByCategoryQuerry, GetProductsByCategoryResult>
    {

        public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuerry request, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductsByCategoryQueryHandler called with {@Query}", request);
            var productsFromDb = await session.Query<Product>().Where(p => p.Category.Contains(request.Category)).ToListAsync(cancellationToken);
            return new GetProductsByCategoryResult(productsFromDb);
        }
    }
}
