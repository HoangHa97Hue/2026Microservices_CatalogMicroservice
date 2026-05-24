var builder = WebApplication.CreateBuilder(args);

// add serviceres to the container
builder.Services.AddCarter();
builder.Services.AddMediatR(config => 

{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddMarten(options =>
{
    // Establish the connection string to your Marten database
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

var app = builder.Build();

//configure the http request pipeline
app.MapCarter();
app.Run();
