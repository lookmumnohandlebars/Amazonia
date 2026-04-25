using System;
using Amazon.Lambda.Core;

namespace Amazonia.EasyLambda;

public class InMemoryLambdaLogger : ILambdaLogger
{
    public void Log(string message) => Console.WriteLine(message);


    public void LogLine(string message) => Log(message);
}