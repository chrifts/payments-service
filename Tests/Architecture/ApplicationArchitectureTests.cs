using Application;
using NetArchTest.Rules;

namespace Tests.Architecture;

public class ApplicationArchitectureTests
{
    // Handlers talk to IPaymentRepository and IUnitOfWork, never to the DbContext or HTTP types
    [Fact]
    public void Application_ShouldNotDependOnOuterLayersEfCoreOrAspNetCore()
    {
        // Given
        var applicationTypes = Types.InAssembly(AssemblyReference.Assembly);

        // When
        var result = applicationTypes
            .ShouldNot()
            .HaveDependencyOnAny("Infrastructure", "Web", "Microsoft.EntityFrameworkCore", "Microsoft.AspNetCore")
            .GetResult();

        // Then
        Assert.True(result.IsSuccessful);
    }
}
