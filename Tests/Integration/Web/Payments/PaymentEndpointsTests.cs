using System.Net;
using System.Net.Http.Json;
using Application.Payments;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Tests.Integration.Infrastructure;

namespace Tests.Integration.Web.Payments;

public class PaymentEndpointsTests(MySqlFixture fixture) : IClassFixture<MySqlFixture>
{
    [Fact]
    public async Task CreateThenPayThenGet_ReturnsPaidPayment()
    {
        // Given
        await using WebApplicationFactory<Program> factory = CreateFactory();
        HttpClient client = factory.CreateClient();
        HttpResponseMessage created = await client.PostAsJsonAsync("/payments", CashPaymentRequest(100.50m, "EUR", 120));

        // When
        HttpResponseMessage paid = await client.PostAsync($"{created.Headers.Location}/pay", content: null);
        PaymentResponse? payment = await client.GetFromJsonAsync<PaymentResponse>(created.Headers.Location);

        // Then
        Assert.Equal(HttpStatusCode.Created, created.StatusCode);
        Assert.Equal(HttpStatusCode.OK, paid.StatusCode);
        Assert.NotNull(payment);
        Assert.Equal("Paid", payment.Status);
        Assert.NotNull(payment.PaidAt);
    }

    [Fact]
    public async Task Create_WithUnsupportedCurrency_ReturnsBadRequestProblemDetails()
    {
        // Given
        await using WebApplicationFactory<Program> factory = CreateFactory();
        HttpClient client = factory.CreateClient();

        // When
        HttpResponseMessage response = await client.PostAsJsonAsync("/payments", CashPaymentRequest(10, "XYZ", 10));

        // Then
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Create_WithoutPaymentMethodType_ReturnsBadRequest()
    {
        // Given
        await using WebApplicationFactory<Program> factory = CreateFactory();
        HttpClient client = factory.CreateClient();
        var request = new { amountDue = 10m, currencyCode = "EUR", paymentMethod = new { tenderedAmount = 10m } };

        // When
        HttpResponseMessage response = await client.PostAsJsonAsync("/payments", request);

        // Then
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private WebApplicationFactory<Program> CreateFactory()
    {
        return new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            // Not "Development": user secrets aren't loaded, so the app can only reach the test container
            builder.UseEnvironment("Testing");
            builder.UseSetting("ConnectionStrings:PaymentsDb", fixture.ConnectionString);
        });
    }

    private static object CashPaymentRequest(decimal amountDue, string currencyCode, decimal tenderedAmount)
    {
        return new { amountDue, currencyCode, paymentMethod = new { type = "cash", tenderedAmount } };
    }
}
