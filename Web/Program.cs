using System.Text.Json.Serialization.Metadata;
using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Scalar.AspNetCore;
using Serilog;
using Web.ExceptionHandling;
using Web.Payments;

var builder = WebApplication.CreateBuilder(args);

// Sinks and levels come from appsettings: sending logs somewhere else doesn't need a code change
builder.Services.AddSerilog((_, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(builder.Configuration));
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<DomainExceptionHandler>();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver
    {
        Modifiers = { PaymentMethodJson.AddPolymorphism },
    };
    // Clients don't have to send the "type" discriminator as the first property
    options.SerializerOptions.AllowOutOfOrderMetadataProperties = true;
});
// A malformed body is a 400 in every environment (Development would throw and end as a 500)
builder.Services.Configure<RouteHandlerOptions>(options => options.ThrowOnBadRequest = false);

var app = builder.Build();

// Only the Docker demo turns this on; in development migrations run with `dotnet ef database update`
if (app.Configuration.GetValue<bool>("Database:MigrateOnStartup"))
{
    await app.Services.MigrateDatabaseAsync();
}

app.UseExceptionHandler();
app.UseStatusCodePages();

// API description (/openapi/v1.json) and its interactive UI (/scalar), only in Development (local runs and the Docker demo)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapPaymentEndpoints();

app.Run();
