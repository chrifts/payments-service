using System.Text.Json.Serialization.Metadata;
using Application;
using Infrastructure;
using Web.ExceptionHandling;
using Web.Payments;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

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

app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapPaymentEndpoints();

app.Run();
