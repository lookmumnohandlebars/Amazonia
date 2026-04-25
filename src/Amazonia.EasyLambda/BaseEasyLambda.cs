using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Amazonia.EasyLambda;

public abstract class BaseEasyLambda<TInput, TOutput, THandler> : IEasyLambda<TInput, TOutput>
    where THandler : notnull
    where TOutput : notnull
{
    protected THandler Handler { get; }
    private Action<Exception> OnError { get; }
    
    /// <summary>
    ///     Initialises with the standard Logging, configuration & application dependencies
    /// </summary>
    protected BaseEasyLambda(
        Func<IConfigurationBuilder, ServiceProvider> serviceProviderFactory, 
        Action<ServiceProvider, Exception> onError
    )
        : this(new ConfigurationBuilder(), serviceProviderFactory, onError) { }

    protected BaseEasyLambda(
        IConfigurationBuilder configurationBuilder, 
        Func<IConfigurationBuilder, ServiceProvider> serviceProviderFactory,
        Action<ServiceProvider, Exception> onError
    )
    {
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        var serviceProvider = serviceProviderFactory(configurationBuilder);
        Handler = serviceProvider.GetService<THandler>() ?? throw new InvalidOperationException($"Failed to resolve handler of type {typeof(THandler).FullName} from the service provider. Please check the configuration of the service provider in the; lambda constructor.");
        OnError = e => onError(serviceProvider, e);
    }

    public async Task<TOutput> HandleAsync(TInput input, ILambdaContext lambdaContext)
    {
        try
        {
            return await InnerHandleAsync(input);
        }
        catch (Exception e)
        {
            OnError.Invoke(e);   
            throw;
        }
    }

    /// <summary>
    ///     The inner handler to be implemented by the specific event listener function
    /// </summary>
    /// <param name="input">The event message (unwrapped from the Eventbridge wrapper)</param>
    /// <returns></returns>
    public abstract Task<TOutput> InnerHandleAsync(TInput input);
}