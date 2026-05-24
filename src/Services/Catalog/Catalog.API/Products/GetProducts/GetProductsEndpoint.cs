namespace Catalog.API.Products.GetProduct;

//public record GetProductsRequest(); // dont require any request object


public record GetProductsResponse(IEnumerable<Product> products);
public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        //pass value from request body to command, then pass command to mediator, get result and return response
        app.MapGet("/products", async (ISender sender) =>
        {
            var result = await sender.Send(new GetProductsQuerry());
            var response = result.Adapt<GetProductsResponse>();
            return Results.Ok(response);
        }).WithName("GetProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get products")
        .WithDescription("Get products");
    }
}
