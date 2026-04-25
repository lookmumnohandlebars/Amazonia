using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Serialization.Json;

namespace Amazonia.EasyLambda;

public class InMemoryLambdaRunner<TLambda, TReq, TOutput>(Func<TLambda> lambdaFactory, Func<string, TOutput>? responseConverter = null)
    where TLambda : IEasyLambda<TReq, TOutput>
    where TOutput : notnull
{
    private readonly JsonSerializer _serializer = new();
    private readonly Func<TestLambdaContext> _lambdaContext = () => new();

    public async Task<TOutput> InvokeLambda(string payload)
    {
        var request = DeserializeRequest(payload);
        var lambda = lambdaFactory();
        var result = await lambda.HandleAsync(request, _lambdaContext());
        var rawResponse = await SerializeResponseAsync(result);
        return responseConverter != null ? responseConverter(rawResponse) : DeserializeResponse(rawResponse);
    }
    
    private TReq DeserializeRequest(string payload)
    {
        using var payloadStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(payload));
        return _serializer.Deserialize<TReq>(payloadStream) ??
               throw new InvalidOperationException("Failed to deserialize payload. Either the test setup is wrong or there is a code change which will cause a break of contract");
    }
    
    private TOutput DeserializeResponse(string response)
    {
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(response));
        var deserializer = new JsonSerializer();
        var deserialized = deserializer.Deserialize<TOutput>(ms);
        return deserialized;
    }
    
    private async Task<string> SerializeResponseAsync(TOutput responsePayload)
    {
        var textStream = new MemoryStream();
        _serializer.Serialize(responsePayload, textStream);
        textStream.Position = 0;
        using var reader = new StreamReader(textStream);
        return await reader.ReadToEndAsync();
    }
}