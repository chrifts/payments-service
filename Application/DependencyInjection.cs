using Application.Payments.CreatePayment;
using Application.Payments.GetPayment;
using Application.Payments.PayPayment;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreatePaymentHandler>();
        services.AddScoped<GetPaymentHandler>();
        services.AddScoped<PayPaymentHandler>();
        return services;
    }
}
