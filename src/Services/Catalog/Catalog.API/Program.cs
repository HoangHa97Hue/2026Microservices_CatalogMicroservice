
var builder = WebApplication.CreateBuilder(args);

// add serviceres to the container
builder.Services.AddMediatR(config => 
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidatorBehavior<,>)); // add the validation pipeline to the MediatR configuration
    config.AddOpenBehavior(typeof(LoggingBehavior<,>)); 
});

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddCarter();

builder.Services.AddMarten(options =>
{
    // Establish the connection string to your Marten database
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();  //nen co logic retry de kiem tra db ready chua roi moi insert data

builder.Services.AddExceptionHandler<CustomExceptionHandler>(); // add a customerexceptionhandler as a service into DI container 

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();

//configure the http request pipeline
app.MapCarter();
app.UseExceptionHandler(opt => { });  // configure the app to use our Customhandler , {} => relying on custom configure handler

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();
