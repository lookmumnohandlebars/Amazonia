using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;

namespace Amazonia.EasyLambda;

public interface IEasyLambda<TInput, TOutput>
     where TOutput : notnull
{
    Task<TOutput> HandleAsync(TInput input, ILambdaContext lambdaContext);
}
