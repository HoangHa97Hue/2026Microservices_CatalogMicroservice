namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductsCommand(Guid id) : ICommand<DeleteProductsResult>;

    public record DeleteProductsResult(Guid id);
    internal class DeleteProductsHandler
        (IDocumentSession session, ILogger<DeleteProductsHandler> logger)
        : ICommandHandler<DeleteProductsCommand, DeleteProductsResult>
    {

        public async Task<DeleteProductsResult> Handle(DeleteProductsCommand command, CancellationToken cancellationToken)
        {
            //existing 
            var existing = await session.LoadAsync<Product>(command.id, cancellationToken);
            if(existing is null)
            {
                throw new Exception($"Product {command.id} not found");
            }
            logger.LogInformation("DeleteProductsHandler called with {@Command}", command);
            //session.Delete<Product>(command.id);
            session.Delete(existing);
            await session.SaveChangesAsync(cancellationToken);
            return new DeleteProductsResult(command.id);
        }
    }
}
