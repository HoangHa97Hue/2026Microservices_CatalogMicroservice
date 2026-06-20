namespace Basket.API.Data
{
    public class BasketRepository(IDocumentSession session) : IBasketRepository
    {

        public async Task<Guid> DeleteBasket(Guid userId, CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();
            session.Delete<ShoppingCart>(userId);
            await session.SaveChangesAsync(cancellationToken);
            return userId;
        }


        public async Task<ShoppingCart> GetBasket(Guid userId, CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();
            var productFromDb = await session.LoadAsync<ShoppingCart>(userId, cancellationToken);
            return productFromDb is null ? throw new BasketNotFoundException(userId) : productFromDb;
        }

        public async Task<Guid> StoreBasket(ShoppingCart cart, CancellationToken cancellationToken = default)
        {
            session.Store(cart);
            await session.SaveChangesAsync(cancellationToken);
            return cart.UserId;
        }
    }
}
