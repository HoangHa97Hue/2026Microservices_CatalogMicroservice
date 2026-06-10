namespace Catalog.API.Products.GetProduct;

public record GetProductsRequest(int PageNumber = 1, int PageSize = 10); // dont take int? because we can send request with null values and default values will be used


public record GetProductsResponse(IEnumerable<Product> products);
public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        //pass value from request body to command, then pass command to mediator, get result and return response
        app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
        {
            var querry = request.Adapt<GetProductsQuerry>();
            var result = await sender.Send(querry);
            var response = result.Adapt<GetProductsResponse>();
            return Results.Ok(response);
        }).WithName("GetProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get products")
        .WithDescription("Get products");
    }
}
