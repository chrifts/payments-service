using Infrastructure.Department.Persistence;
using Infrastructure.Employee.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : DbContext(options)
{
    public DbSet<EmployeeModel> Employees => Set<EmployeeModel>();
    public DbSet<DepartmentModel> Departments => Set<DepartmentModel>();
}