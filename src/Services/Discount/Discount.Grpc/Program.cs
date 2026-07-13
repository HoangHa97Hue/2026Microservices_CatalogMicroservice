using Discount.Grpc.Data;
using Discount.Grpc.Model;
using Discount.Grpc.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DiscountContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddGrpc();
var app = builder.Build();
//configure the HTTP request pipeline
app.UseMigration(); // apply migration automatically when the application starts, should add in the beginning of the configuring HTTP request pipeline, ensure migration occurs before app start up processing and before accepting any request
app.MapGrpcService<DiscountService>();
app.Run();
