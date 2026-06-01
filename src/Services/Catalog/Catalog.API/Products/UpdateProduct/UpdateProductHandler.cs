using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductsCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductsResult>;

    public record UpdateProductsResult(Guid Id);

    public class UpdateroductCommandValidator : AbstractValidator<UpdateProductsCommand>
    {
        public UpdateroductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
                .Length(2, 150).WithMessage("Name must be bettween 2 and 150 characters");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
            //RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
            //RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

    internal class UpdateProductsHandler
        (IDocumentSession session, ILogger<UpdateProductsHandler> logger)
        : ICommandHandler<UpdateProductsCommand, UpdateProductsResult>
    {

        public async Task<UpdateProductsResult> Handle(UpdateProductsCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateProductsHandler called with {@Command}", command);
            Product product = command.Adapt<Product>();
            session.Update<Product>(product);
            await session.SaveChangesAsync(cancellationToken);
            return new UpdateProductsResult(product.Id);
        }

    }
}
