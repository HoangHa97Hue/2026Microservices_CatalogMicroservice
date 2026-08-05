using Discount.Grpc.Protos;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(Guid UserId);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart must not be null.");
        RuleFor(x => x.Cart.UserId).NotNull().NotEmpty().WithMessage("UserId must not be empty and not null.");
        //RuleFor(x => x.Cart.ProductItems).NotNull().WithMessage("ProductItems must not be null.");
    }
}

internal class StoreBasketCommandHandler
        (IBasketRepository basketRepository, DiscountProtoService.DiscountProtoServiceClient discountClient)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{

    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        ShoppingCart cart = command.Cart;
        //loop all product to get discount
        foreach(var item in cart.Items) {
            var discount = await discountClient.GetDiscountAsync(new GetDiscountRequest { ProductId = item.ProductId.ToString() });
            if(discount is not null)
                item.Price -= (decimal)discount.Amount;
        }

        //TODO: Store shopping cart in DB, if exists update it, otherwise create new one
        await basketRepository.StoreBasket(cart, cancellationToken);
        //update cache
        //session.Store(cart);
        //await session.SaveChangesAsync(cancellationToken);
        return new StoreBasketResult(command.Cart.UserId);
    }
}


