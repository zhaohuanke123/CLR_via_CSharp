// 文件名：PartialClassTest.cs
// 编译为可执行程序

using System;

public class PartialClassTest
{
    public static void Main()
    {
        PartialClass instance = new PartialClass();

        // 测试 Part1 的方法
        Console.WriteLine(instance.Part1Method());

        // 测试 Part2 的方法
        Console.WriteLine(instance.Part2Method());
    }
}
