using System.Runtime.CompilerServices;

internal sealed class Type1
{
}

internal sealed class Type2
{
}

internal class Program
{
    static void Main()
    {
        MyMethodAsync(123).Wait();
    }

    private static async Task<String> MyMethodAsync(Int32 argument)
    {
        Int32 local = argument;
        try
        {
            Type1 result1 = await Method1Async();
            for (Int32 x = 0; x < 3; x++)
            {
                Type2 result2 = await Method2Async();
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Catch");
        }
        finally
        {
            Console.WriteLine("Finally");
        }

        return "Done";
    }

    private static async Task<Type1> Method1Async()
    {
        /* 以异步方式执行一些操作，最后返回一个 Type1 对象 */
        return new Type1();
    }

    private static async Task<Type2> Method2Async()
    {
        /* 以异步方式执行一些操作，最后返回一个 Type2 对象 */
        return new Type2();
    }
}