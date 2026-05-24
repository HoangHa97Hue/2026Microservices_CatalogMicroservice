namespace Catalog.API.Products.GetProductById;

//public record GetProductsByCategoryRequest(Guid Id);
public record GetProductByIdResponse(Product product);
public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        //pass value from request body to command, then pass command to mediator, get result and return response
        app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByIdQuerry(id));
            var response = result.Adapt<GetProductByIdResponse>();
            return Results.Ok(response);
        }).WithName("GetProductById")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get product by ID")
        .WithDescription("Get product by ID");
    }
}
