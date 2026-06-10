using Catalog.API.Products.UpdateProduct;
using Marten.Pagination;

namespace Catalog.API.Products.GetProduct
{
    public record GetProductsQuerry(int PageNumber = 1, int PageSize = 10) : IQuerry<GetProductsResult>;

    public record GetProductsResult(IEnumerable<Product> products);

    public class  GetProductsValidator : AbstractValidator<GetProductsQuerry>
    {
        public GetProductsValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("PageNumber is required");
            RuleFor(x => x.PageSize).InclusiveBetween(1,100).WithMessage("PageSize must be between 1 and 100");
        }
    }

    internal class GetProductsHandler
        (IDocumentSession session)
        : IQuerryHandler<GetProductsQuerry, GetProductsResult>
    {

        public async Task<GetProductsResult> Handle(GetProductsQuerry request, CancellationToken cancellationToken)
        {
            var productsFromDb = await session.Query<Product>().ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);
            return new GetProductsResult(productsFromDb);
        }
    }
}
