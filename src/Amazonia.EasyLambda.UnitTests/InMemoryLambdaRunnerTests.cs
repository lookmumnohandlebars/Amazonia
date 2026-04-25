using System.Text;
using Amazon.Lambda.Serialization.Json;
using Microsoft.Extensions.Configuration;

namespace Amazonia.EasyLambda.UnitTests;

public class InMemoryLambdaRunnerTests
{
    [Fact] 
    public async Task Runner_should_invoke_lambda_and_return_result()
    {
        // Arrange
        var configBuilder = new ConfigurationBuilder();
        var runner = new InMemoryLambdaRunner<TestLambda, TestRequest, TestResponse>(
            () => new TestLambda(configBuilder)
        );

        // Act
        var result = await runner.InvokeLambda("{ \"Input\": \"Test Input\" } }");

        // Assert
        Assert.Equal("Processed: Test Input", result.Output);
    }
}