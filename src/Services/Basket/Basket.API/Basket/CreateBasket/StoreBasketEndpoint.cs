 namespace Basket.API.Basket.StoreBasket;

//public record CreateBasketRequest(Guid UserId, List<ShoppingCartItem> ProductItems);  // dang o day de sau nay co them thuoc tinh thi them vao sau, tam thoi de empty
public record StoreBasketRequest(ShoppingCart Cart);  // dang o day de sau nay co them thuoc tinh thi them vao sau, tam thoi de empty
public record StoreBasketResponse(Guid UserId);
public class StoreBasketEndpoint() : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
        {

            var requestDto = request.Adapt<StoreBasketCommand>();
            var result = await sender.Send(requestDto);
            var response = result.Adapt<StoreBasketResponse>();
            //return Results.Ok(response);
            return Results.Created($"/basket/{response.UserId}", response);
        }).WithName("StoreBasket")
   .Produces<StoreBasketResponse>(StatusCodes.Status201Created) 
   .ProducesProblem(StatusCodes.Status400BadRequest)
   .WithSummary("Store basket by user ID")
   .WithDescription("Store basket by user ID");
    }
}
