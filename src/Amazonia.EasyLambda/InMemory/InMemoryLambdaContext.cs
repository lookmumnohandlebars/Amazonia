using System;
using Amazon.Lambda.Core;

namespace Amazonia.EasyLambda;

public class TestLambdaContext : ILambdaContext
{

    /// <summary>The AWS request ID associated with the request.</summary>
    public string AwsRequestId { get; set; } = null!;

    /// <summary>
    /// Information about the client application and device when invoked
    /// through the AWS Mobile SDK.
    /// </summary>
    public IClientContext ClientContext { get; set; } = null!;

    /// <summary>Name of the Lambda function that is running.</summary>
    public string FunctionName { get; set; } = null!;

    /// <summary>
    /// The Lambda function version that is executing.
    /// If an alias is used to invoke the function, then this will be
    /// the version the alias points to.
    /// </summary>
    public string FunctionVersion { get; set; } = null!;

    /// <summary>
    /// Information about the Amazon Cognito identity provider when
    /// invoked through the AWS Mobile SDK.
    /// </summary>
    public ICognitoIdentity Identity { get; set; } = null!;

    /// <summary>The ARN used to invoke this function.</summary>
    public string InvokedFunctionArn { get; set; } = null!;

    /// <summary>
    /// Lambda logger associated with the Context object. For the TestLambdaContext this is default to the TestLambdaLogger.
    /// </summary>
    public ILambdaLogger Logger { get; set; } = new InMemoryLambdaLogger();

    /// <summary>
    /// The CloudWatch log group name associated with the invoked function.
    /// </summary>
    public string LogGroupName { get; set; } = string.Empty;

    /// <summary>
    /// The CloudWatch log stream name for this function execution.
    /// </summary>
    public string LogStreamName { get; set; } = string.Empty;

    /// <summary>
    /// Memory limit, in MB, you configured for the Lambda function.
    /// </summary>
    public int MemoryLimitInMB { get; set; } = 2048;

    /// <summary>
    /// Remaining execution time till the function will be terminated.
    /// </summary>
    public TimeSpan RemainingTime { get; set; } = new TimeSpan();
}