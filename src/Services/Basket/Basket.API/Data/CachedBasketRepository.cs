using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
    {
        public async Task<ShoppingCart> GetBasket(Guid userId, CancellationToken cancellationToken = default)
        {
            var cachedBasket = await cache.GetStringAsync(userId.ToString(), cancellationToken);
            if (!string.IsNullOrEmpty(cachedBasket))
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
            }


            var basket = await repository.GetBasket(userId, cancellationToken);
            await cache.SetStringAsync(userId.ToString(), JsonSerializer.Serialize(basket), cancellationToken);
            return basket;
        }

        public async Task<Guid> StoreBasket(ShoppingCart cart, CancellationToken cancellationToken = default)
        {
            var basket = await repository.StoreBasket(cart, cancellationToken);
            //var cacheOptions = new DistributedCacheEntryOptions
            //{
            //    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
            //    SlidingExpiration = TimeSpan.FromMinutes(30)
            //};
            await cache.SetStringAsync(cart.UserId.ToString(), JsonSerializer.Serialize(cart), cancellationToken);
            return cart.UserId;
        }

        public async Task<bool> DeleteBasket(Guid userId, CancellationToken cancellationToken = default)
        {
            await repository.DeleteBasket(userId, cancellationToken);
            await cache.RemoveAsync(userId.ToString(), cancellationToken);
            return true;
        }
    }
}
