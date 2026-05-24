namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductsRequest(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price);


public record UpdateProductsResponse(Guid Id);
public class UpdateProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        //pass value from request body to command, then pass command to mediator, get result and return response
        app.MapPut("/products", async (UpdateProductsRequest request, ISender sender) =>
        {
            //if(request.id != productId)
            var command = request.Adapt<UpdateProductsCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<UpdateProductsResponse>();
            return Results.Ok(response);
        }).WithName("UpdateProduct")
        .Produces<UpdateProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update product")
        .WithDescription("Update product");
    }
}
