namespace Catalog.API.Products.GetProductsByCategory
{
    public record GetProductsByCategoryQuerry(string Category) : IQuerry<GetProductsByCategoryResult>;

    public record GetProductsByCategoryResult(IEnumerable<Product> products);
    internal class GetProductsByCategoryHandler
        (IDocumentSession session)
        : IQuerryHandler<GetProductsByCategoryQuerry, GetProductsByCategoryResult>
    {

        public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuerry request, CancellationToken cancellationToken)
        {
            var productsFromDb = await session.Query<Product>().Where(p => p.Category.Contains(request.Category)).ToListAsync(cancellationToken);
            return new GetProductsByCategoryResult(productsFromDb);
        }
    }
}
