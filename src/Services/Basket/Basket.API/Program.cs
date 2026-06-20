using Basket.API.Data;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();
// add serviceres to the container
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    options.Schema.For<ShoppingCart>()
        .Identity(x => x.UserId);
}).UseLightweightSessions();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidatorBehavior<,>)); // add the validation pipeline to the MediatR configuration
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
var app = builder.Build();

app.MapCarter();
app.UseExceptionHandler(opt => { });  // configure the app to use our Customhandler , {} => relying on custom configure handler
app.Run();
