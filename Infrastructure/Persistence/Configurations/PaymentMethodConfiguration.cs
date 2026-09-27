using Domain.Payments.PaymentMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("PaymentMethods");

        builder.Property<Guid>("Id"); // shadow property: column exists, class doesn't have it
        builder.HasKey("Id");

        builder.HasDiscriminator<string>("MethodType")
            .HasValue<CashPayment>("Cash");
    }
}
