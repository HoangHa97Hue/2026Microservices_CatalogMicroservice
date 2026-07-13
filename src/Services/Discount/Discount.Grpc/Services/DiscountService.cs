using Discount.Grpc.Model;
using Discount.Grpc.Protos;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;
using DiscountEntity = Discount.Grpc.Data.Discount;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<DiscountModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var discount = await dbContext.Discount.FirstOrDefaultAsync(d => d.ProductId == Guid.Parse(request.ProductId));
        if (discount == null)
        {
            discount = new DiscountEntity
            {
                ProductName = "No discount",
                Amount = 0,
                Description = "No discount description"
            };
        }
        DiscountModel discountResponse = discount.Adapt<DiscountModel>();
        logger.LogInformation("Discount retrieved for ProductName: {ProductName}, Amount: {Amount}", discount.ProductName, discount.Amount);
        return discountResponse;
    }

    public override async Task<DiscountModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var discountEntity = request.Discount.Adapt<DiscountEntity>();
        dbContext.Discount.Add(discountEntity);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Discount created for ProductName: {ProductName}, Amount: {Amount}", discountEntity.ProductName, discountEntity.Amount);
        return discountEntity.Adapt<DiscountModel>();
    }

    public override async Task<DiscountModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var discountEntity = request.Discount.Adapt<DiscountEntity>();
        if(discountEntity is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request Object"));
        }
        dbContext.Discount.Update(discountEntity);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Discount updated for ProductI: {ProductId}, ProductName: {ProductName}, Amount: {Amount}", discountEntity.ProductId, discountEntity.ProductName, discountEntity.Amount);
        return discountEntity.Adapt<DiscountModel>();
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var discountEntity = await dbContext.Discount.FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.Id));
        if (discountEntity is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Product with productID : {request.Id} is not found"));
        }
        dbContext.Discount.Remove(discountEntity);
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Discount deleted successfully for ProductId: {ProductId}, ProductName: {ProductName}", discountEntity.ProductId, discountEntity.ProductName);
        return new DeleteDiscountResponse { Success = true} ;
    }
}