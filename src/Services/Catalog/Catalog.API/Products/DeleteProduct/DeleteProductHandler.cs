using Catalog.API.Products.UpdateProduct;

namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductsCommand(Guid Id) : ICommand<DeleteProductsResult>;

    public record DeleteProductsResult(Guid Id);

    public class DeletePoductCommandValidator : AbstractValidator<DeleteProductsCommand>
    {
        public DeletePoductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required");
        }
    }
    internal class DeleteProductsHandler
        (IDocumentSession session)
        : ICommandHandler<DeleteProductsCommand, DeleteProductsResult>
    {

        public async Task<DeleteProductsResult> Handle(DeleteProductsCommand command, CancellationToken cancellationToken)
        {
            //existing 
            var existing = await session.LoadAsync<Product>(command.Id, cancellationToken);
            if(existing is null)
            {
                throw new Exception($"Product {command.Id} not found");
            }
            //session.Delete<Product>(command.id);
            session.Delete(existing);
            await session.SaveChangesAsync(cancellationToken);
            return new DeleteProductsResult(command.Id);
        }
    }
}
