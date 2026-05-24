namespace Catalog.API.Products.GetProductsByCategory;

//public record GetProductsByCategoryRequest(string Category);


public record GetProductsByCategoryResponse(IEnumerable<Product> products);
public class GetProductsByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        //pass value from request body to command, then pass command to mediator, get result and return response
        //app.MapGet("/products/{category}", async (string category, ISender sender) => // filtering should use Querry Parram, not path parram => it conflict with getproductbyid
        app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
        {
            var result = await sender.Send(new GetProductsByCategoryQuerry(category));
            var response = result.Adapt<GetProductsByCategoryResponse>();
            return Results.Ok(response);
        }).WithName("GetProductsByCategory")
        .Produces<GetProductsByCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get products by category ")
        .WithDescription("Get products by category");
    }
}
