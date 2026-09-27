using Domain;
using NetArchTest.Rules;

namespace Tests.Architecture;

public class DomainArchitectureTests
{
    [Fact]
    public void Domain_ShouldNotDependOnOtherLayersOrEfCore()
    {
        // Given
        var domainTypes = Types.InAssembly(AssemblyReference.Assembly);

        // When
        var result = domainTypes
            .ShouldNot()
            .HaveDependencyOnAny("Application", "Infrastructure", "Microsoft.EntityFrameworkCore")
            .GetResult();

        // Then
        Assert.True(result.IsSuccessful);
    }
}
