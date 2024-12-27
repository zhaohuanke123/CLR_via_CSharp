using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;

public class Worker : MarshalByRefObject
{
    public void PrintDomain()
    {
        Console.WriteLine($"Object is executing in AppDomain \"{typeof(Assembly)}\"");
    }
}

class Example
{
    public static void Main()
    {
        // Create an ordinary instance in the current AppDomain
        Worker localWorker = new Worker();
        localWorker.PrintDomain();

        // Use AssemblyLoadContext to load and execute code in isolation
        var context = new AssemblyLoadContext("NewContext", true);
        var assembly = context.LoadFromAssemblyPath(Assembly.GetExecutingAssembly().Location);
        var workerType = assembly.GetType("Worker");
        // 在context里面创建对象
        var workerInstance = Activator.CreateInstance(workerType);
        workerType.GetMethod("PrintDomain").Invoke(workerInstance, null);
    }
}