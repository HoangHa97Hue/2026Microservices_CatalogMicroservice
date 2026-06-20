namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketCommand(Guid UserId) : ICommand<DeleteBasketResult>;
public record DeleteBasketResult(bool IsSuccess);

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId must not be empty.");
    }
}

internal class DeleteBasketHandler
        (IBasketRepository basketRepository)
        : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{

    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        //var productFromDb = await session.Query<Product>().SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        //var basket = await session.LoadAsync<ShoppingCart>(request.userId, cancellationToken);
        //if (basket is null)
        //{
        //    throw new BasketNotFoundException(request.userId);
        //}

        //session.Delete(basket);
        //await session.SaveChangesAsync(cancellationToken);

        //todo get from cache
        //get from db
        var basket = await basketRepository.GetBasket(request.UserId);
        if (basket is null)
            throw new BasketNotFoundException(request.UserId);
        //TODO : remove from db
        //TODO: remove cache
        return new DeleteBasketResult(true);
    }
}


