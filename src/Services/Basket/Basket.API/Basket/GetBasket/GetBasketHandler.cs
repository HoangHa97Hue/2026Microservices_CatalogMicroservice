namespace Basket.API.Basket.GetBasket;

public record GetBasketByUserIdQuerry(Guid userId) : IQuerry<GetBasketByUserIdResult>;
public record GetBasketByUserIdResult(ShoppingCart shoppingCart);

internal class GetBasketHandler
        (IBasketRepository basketRepository)
        : IQuerryHandler<GetBasketByUserIdQuerry, GetBasketByUserIdResult>
{

    public async Task<GetBasketByUserIdResult> Handle(GetBasketByUserIdQuerry request, CancellationToken cancellationToken)
    {
        //var productFromDb = await session.Query<Product>().SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        //var basket = await session.LoadAsync<ShoppingCart>(request.userId, cancellationToken);
        var basket = await basketRepository.GetBasket(request.userId);
        if (basket is null)
        {
            throw new BasketNotFoundException(request.userId);
        }
        return new GetBasketByUserIdResult(basket);
    }
}
