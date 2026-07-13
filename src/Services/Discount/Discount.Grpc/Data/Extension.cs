using Discount.Grpc.Model;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    //this extension is for automigration database, because after add migration then update database, the database will be created and the data will be inserted, but if we want to do this automatically when the application starts, we can use this extension
    public static class Extension
    {
        //auto asynchronusly applies when any pending migrations for the context to the database. Will create the database if it does not already exist.
        public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DiscountContext>();
                context.Database.MigrateAsync();
            }
            return app;
        }
    }
}
