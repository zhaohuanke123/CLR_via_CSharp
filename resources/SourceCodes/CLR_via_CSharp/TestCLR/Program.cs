using System;

public class Program
{
    public static int Main(string arg)
    {
        Console.WriteLine($"Hello from managed code! Argument: {arg}");
        return arg.Length; // 返回字符串长度作为执行结果
    }
}