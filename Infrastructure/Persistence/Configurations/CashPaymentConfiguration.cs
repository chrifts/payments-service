using Domain.Payments.PaymentMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CashPaymentConfiguration : IEntityTypeConfiguration<CashPayment>
{
    public void Configure(EntityTypeBuilder<CashPayment> builder)
    {
        builder.ComplexProperty(cashPayment => cashPayment.Tendered, moneyBuilder =>
        {
            moneyBuilder.Property(money => money.Amount)
                .HasColumnName("TenderedAmount")
                .HasPrecision(18, 8);

            moneyBuilder.ComplexProperty(money => money.Currency, currencyBuilder =>
            {
                currencyBuilder.Property(currency => currency.Code).HasColumnName("TenderedCurrencyCode").HasMaxLength(10);
                currencyBuilder.Property(currency => currency.Precision).HasColumnName("TenderedCurrencyPrecision");
            });
        });
    }
}
