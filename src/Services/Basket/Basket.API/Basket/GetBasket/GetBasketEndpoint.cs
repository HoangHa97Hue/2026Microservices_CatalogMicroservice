namespace Basket.API.Basket.GetBasket;

//public record GetBasketByUserIdRequest(Guid userId);
public record GetBasketByUserIdResponse(ShoppingCart shoppingCart);
public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userId}", async (Guid userId, ISender sender) =>
        {
            var result = await sender.Send(new GetBasketByUserIdQuerry(userId));
            var response = result.Adapt<GetBasketByUserIdResponse>();
            return Results.Ok(response);
        }).WithName("GetBasketByUserId")
   .Produces<GetBasketByUserIdResponse>(StatusCodes.Status200OK)
   .ProducesProblem(StatusCodes.Status400BadRequest)
   .WithSummary("Get basket by user ID")
   .WithDescription("Get basket by user ID");
    }
}
