namespace Basket.API.Model;

public class ShoppingCart
{
    public Guid UserId { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = new();
    public decimal TotalAmounts => Items.Sum(x => x.Price * x.Quantity);   // its a readonly property, no setter , its mean getter

    public ShoppingCart(Guid userId)
    {
        this.UserId = userId;
    }
}
