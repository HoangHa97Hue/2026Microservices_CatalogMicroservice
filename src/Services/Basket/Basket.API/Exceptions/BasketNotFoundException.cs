namespace Basket.API.Exceptions;

public class BasketNotFoundException : NotFoundException
{
    public BasketNotFoundException(Guid id) : base("Basket", id)
    {
    }
}
