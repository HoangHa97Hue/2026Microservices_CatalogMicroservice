namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductsCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductsResult>;

    public record UpdateProductsResult(Guid Id);

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
