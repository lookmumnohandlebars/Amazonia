using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Amazonia.EasyLambda.UnitTests;

public class TestLambda : BaseEasyLambda<TestRequest, TestResponse, TestLambdaHandler>
{
    public TestLambda() : this(new ConfigurationBuilder())
    {
    }

    public TestLambda(IConfigurationBuilder configurationBuilder) : 
        base(configurationBuilder, Configure, OnError)
    {
    }

    public static ServiceProvider Configure(IConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Build();
        var services = new ServiceCollection();
        services.AddSingleton<TestLambdaHandler>();
        return services.BuildServiceProvider();
    }
    
    public static void OnError(ServiceProvider serviceProvider, Exception exception)
    {
        
    }

    public override Task<TestResponse> InnerHandleAsync(TestRequest input)
    {
        return Handler.HandleAsync(input);
    }
}

public class TestRequest
{
    public string Input { get; set; }
}

public class TestResponse
{
    public string Output { get; set; }
}

public class TestLambdaHandler
{
    public Task<TestResponse> HandleAsync(TestRequest request)
    {
        return Task.FromResult(new TestResponse { Output = $"Processed: {request.Input}" });
    }
}