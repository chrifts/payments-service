using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Application.Payments.CreatePayment;

namespace Web.Payments;

// The JSON shape of PaymentMethodData belongs to the API, not to Application. A new payment method
// is registered in DerivedTypes instead of adding attributes to the base record
public static class PaymentMethodJson
{
    public static void AddPolymorphism(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Type != typeof(PaymentMethodData))
        {
            return;
        }

        typeInfo.PolymorphismOptions = new JsonPolymorphismOptions
        {
            TypeDiscriminatorPropertyName = "type",
            DerivedTypes =
            {
                new JsonDerivedType(typeof(CashPaymentData), "cash"),
            },
        };

        // Without "type", System.Text.Json tries to create the abstract base and throws NotSupportedException (a 500).
        // A JsonException makes ASP.NET Core answer 400, like any other malformed body
        typeInfo.CreateObject = () => throw new JsonException("A payment method needs a \"type\", for example \"cash\".");
    }
}
