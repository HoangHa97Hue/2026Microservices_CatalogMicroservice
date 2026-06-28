namespace Basket.API.Data;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasket(Guid userId, CancellationToken cancellationToken = default);
    Task<Guid> StoreBasket(ShoppingCart cart, CancellationToken cancellationToken = default);
    Task<bool> DeleteBasket(Guid userId, CancellationToken cancellationToken = default);
}
