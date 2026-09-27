using Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Payments;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder
            .Ignore(payment => payment.Method)
            .Ignore(payment => payment.Events)
            .HasKey(payment => payment.Id);

        builder.Property(payment => payment.Id)
            .HasConversion(paymentId => paymentId.Value, guid => new PaymentId(guid));

        builder.ComplexProperty(payment => payment.Amount, moneyBuilder =>
        {
            moneyBuilder.Property(money => money.Amount)
                .HasColumnName("Amount")
                .HasPrecision(18, 8); // decimal(18,8): up to 8 decimals (BTC)

            moneyBuilder.ComplexProperty(money => money.Currency, currencyBuilder =>
            {
                currencyBuilder.Property(currency => currency.Code).HasColumnName("CurrencyCode").HasMaxLength(10);
                currencyBuilder.Property(currency => currency.Precision).HasColumnName("CurrencyPrecision");
            });
        });

        builder.Property(payment => payment.CreatedAt);
    }
}
