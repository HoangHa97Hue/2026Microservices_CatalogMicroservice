namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketRequest(Guid UserId);
public record DeleteBasketResponse(bool IsSuccess);
public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userId}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteBasketCommand(id));
            var response = result.Adapt<DeleteBasketResponse>();
            return Results.Ok(response);
        }).WithName("DeleteBasketByUserId")
   .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
   .ProducesProblem(StatusCodes.Status400BadRequest)
   .WithSummary("Delete basket by user ID")
   .WithDescription("Delete basket by user ID");
    }
}
