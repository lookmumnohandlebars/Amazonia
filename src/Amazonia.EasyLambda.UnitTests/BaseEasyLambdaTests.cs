using Microsoft.Extensions.Configuration;

namespace Amazonia.EasyLambda.UnitTests;

public class BaseEasyLambdaTests
{
    [Fact]
    public async Task Lambda_should_process_request_and_return_response()
    {
        // Arrange
        var configBuilder = new ConfigurationBuilder();
        var lambda = new TestLambda(configBuilder);
        var request = new TestRequest { Input = "Test Input" };

        // Act
        var response = await lambda.HandleAsync(request, new TestLambdaContext());

        // Assert
        Assert.Equal("Processed: Test Input", response.Output);
    }
}