using Application.Payments;
using Application.Payments.CreatePayment;
using Application.Payments.GetPayment;
using Application.Payments.PayPayment;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Web.Payments;

// HTTP only: each endpoint turns the request into a use case call and its result into a status code
public static class PaymentEndpoints
{
    private const string GetPaymentRoute = "GetPayment";

    public static IEndpointRouteBuilder MapPaymentEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder payments = app.MapGroup("/payments");

        payments.MapPost("", CreatePaymentAsync);
        payments.MapGet("/{id:guid}", GetPaymentAsync).WithName(GetPaymentRoute);
        payments.MapPost("/{id:guid}/pay", PayPaymentAsync);

        return app;
    }

    private static async Task<CreatedAtRoute<CreatePaymentResponse>> CreatePaymentAsync(
        CreatePaymentCommand command,
        CreatePaymentHandler handler,
        CancellationToken cancellationToken
    )
    {
        CreatePaymentResponse response = await handler.HandleAsync(command, cancellationToken);
        return TypedResults.CreatedAtRoute(response, GetPaymentRoute, new { id = response.Id });
    }

    private static async Task<Results<Ok<PaymentResponse>, NotFound>> GetPaymentAsync(
        Guid id,
        GetPaymentHandler handler,
        CancellationToken cancellationToken
    )
    {
        return OkOrNotFound(await handler.HandleAsync(new GetPaymentQuery(id), cancellationToken));
    }

    private static async Task<Results<Ok<PaymentResponse>, NotFound>> PayPaymentAsync(
        Guid id,
        PayPaymentHandler handler,
        CancellationToken cancellationToken
    )
    {
        return OkOrNotFound(await handler.HandleAsync(new PayPaymentCommand(id), cancellationToken));
    }

    private static Results<Ok<PaymentResponse>, NotFound> OkOrNotFound(PaymentResponse? payment)
    {
        return payment is null ? TypedResults.NotFound() : TypedResults.Ok(payment);
    }
}
