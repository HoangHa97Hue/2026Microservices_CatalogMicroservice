using Microsoft.EntityFrameworkCore;
//using Discount.Grpc.Data;
using DiscountEntity = Discount.Grpc.Data.Discount;  // fix loi entity trung ten voi namespace
namespace Discount.Grpc.Model
{
    public class DiscountContext : DbContext
    {
        public DbSet<DiscountEntity> Discount { get; set; }
        public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DiscountEntity>().HasData(
                new DiscountEntity
                {
                    Id = Guid.Parse(
                        "6f1c0f77-8f45-4a84-9fc7-1e7a6bb7e101"),

                    ProductId = Guid.Parse(
                        "3d13c704-e34e-441b-bd29-6037bda6ae67"),
                    ProductName = "Iphone 1", Description = "IPhone1 Discount", Amount = 150 },
                new DiscountEntity {
                    Id = Guid.Parse(
                        "8a4d2c91-1b7e-4f56-a3c8-9d0e2f7b6a42"),

                    ProductId = Guid.Parse(
                        "12ff1742-8097-4f95-a2ba-9289605a09f3"),
                    ProductName = "Samung Galaxy 1", Description = "Samsung1 Discount", Amount = 78 }
            );
        }
    }
}
