using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();
        if (await session.Query<Product>().AnyAsync(cancellation))
            return;

        session.Store<Product>(GetPreconfiguredProducts());
        await session.SaveChangesAsync();

    }
    private IEnumerable<Product> GetPreconfiguredProducts()
    {
        var products = new List<Product>
        {
            new Product
            {
                Id = new Guid("3d13c704-e34e-441b-bd29-6037bda6ae67"),
                Name = "Iphone 1",
                Description = "Description for Iphone 1",
                ImageFile = "product-1.png",
                Price = 199.9m,
                Category = new List<string>{"Smart Phone"}
            },
            new Product
            {
               Id = new Guid("12ff1742-8097-4f95-a2ba-9289605a09f3"),
                Name = "Samung Galaxy 1",
                Description = "Description for Samsung 1",
                ImageFile = "product-2.png",
                Price = 209.9m,
                Category = new List<string>{"Smart Phone"}
            }
        };
        return products;
    }
}