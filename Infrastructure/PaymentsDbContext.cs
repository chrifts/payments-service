using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : DbContext(options)
{

}