namespace Catalog.API.Products.DeleteProduct;

//public record DeleteProductsRequest(Guid id);


public record DeleteProductsResponse(Guid id);
public class DeleteProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        //pass value from request body to command, then pass command to mediator, get result and return response
        app.MapDelete("/products/{productId}", async (Guid productId, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductsCommand(productId));
            var response = result.Adapt<DeleteProductsResponse>();
            return Results.Ok(response);
        }).WithName("DeleteProduct")
        .Produces<DeleteProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete product")
        .WithDescription("Delete product");
    }
}
