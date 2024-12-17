#if TARGET_32BITS
using NativeInt = System.Int32;
#else
using NativeInt = System.Int16;
#endif
using System.Globalization;
using System.Runtime.InteropServices;
using Console = System.Console;

internal class Program
{
    static void Main()
    {
        // UnmanagedTypes.Go();
        TestValueTuple.Go();
    }
}

/// <summary>
/// 提供nint 和 nuint 用来表达底层平台的int位数不确定问题。
/// C#中的int是不变的，四字节，如果要本机互操作，需要
/// </summary>
internal class Native_integers
{
    /// <summary>
    /// Function <c>memset</c> in C.
    /// </summary>
    /// <param name="src">The source pointer.</param>
    /// <param name="c">The init value.</param>
    /// <param name="size">The size of the unit.</param>
    /// <returns>The pointer.</returns>
    [DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
    private static unsafe extern void* Memset(
        [Out] void* src,
#if TARGET_32BITS
    [MarshalAs(UnmanagedType.I4)]
#else
        [MarshalAs(UnmanagedType.I2)]
#endif
        [In]
        NativeInt c,
#if TARGET_32BITS
    [MarshalAs(UnmanagedType.I4)]
#else
        [MarshalAs(UnmanagedType.I2)]
#endif
        [In]
        NativeInt size);

    /// 现在可以这样写
    /// <summary>
    /// Function <c>memset</c> in C.
    /// </summary>
    /// <param name="src">The source pointer.</param>
    /// <param name="c">The init value.</param>
    /// <param name="size">The size of the unit.</param>
    /// <returns>The pointer.</returns>
    [DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
    private static unsafe extern void* Memset(
        [Out] void* src,
        [In] nint c,
        [In] nint size);
}

public readonly struct Coords
{
    public Coords(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double X { get; init; }
    public double Y { get; init; }

    public override string ToString() =>
        $"({X.ToString(CultureInfo.InvariantCulture)}, {Y.ToString(CultureInfo.CurrentCulture)})";

    public void Modify()
    {
        // 报错
        // X = 1;
        // Y = 2;
    }
}

// 可以在值类型的声明中使用ref修饰符。ref struct的实例是在堆栈上分配，不能转移到托管堆上，也就是说不允许被装箱或作为堆上对象的成员。
public ref struct CustomRef
{
    public bool IsValid;
    public Span<int> Inputs;
    public Span<int> Outputs;
}

/// <summary>
/// Readonly
/// </summary>
ref struct TestStruct
{
    ref readonly int aConstant; // 值无法修改
    readonly ref int Storage; // 引用无法修改
    readonly ref readonly int CantChange; // 引用和值都无法修改
}

// 规范指导
// 有限推荐使用
//
// 只有在struct内存较大，且生命周期仅限当前callstack，才有意义

public struct Coords<T>
{
    public T X;
    public T Y;
}

public class UnmanagedTypes
{
    public static void Go()
    {
        DisplaySize<Coords<int>>();
        DisplaySize<Coords<double>>();
    }

    private unsafe static void DisplaySize<T>() where T : unmanaged
    {
        // 由于T是非托管类型所以可以直接分配堆栈数组
        var arr = stackalloc T[100];
        Console.WriteLine($"{typeof(T)} is unmanaged and its size is {sizeof(T)} bytes");
    }
}
// Output:
// Coords`1[System.Int32] is unmanaged and its size is 8 bytes
// Coords`1[System.Double] is unmanaged and its size is 16 bytes

class TestValueTuple
{
    public static void Go()
    {
        (double, int) t1 = (4.5, 3); // valueTupe<double,int> t1
        Console.WriteLine($"Tuple with elements {t1.Item1} and {t1.Item2}.");
        // Output:
        // Tuple with elements 4.5 and 3.

        (double Sum, int Count) t2 = (4.5, 3);
        Console.WriteLine($"Sum of {t2.Count} elements is {t2.Sum}.");
        // Output:
        // Sum of 3 elements is 4.5.

        Console.WriteLine(t1.GetType());
        var vt = new ValueTuple<int, int, int, int, int, int, int, int>(); // 最多了

        // public struct ValueTuple : IEquatable<ValueTuple>, IStructuralEquatable, IStructuralComparable, IComparable, IComparable<ValueTuple>, IValueTupleInternal, ITuple
    }

    /// <summary>
    /// 避免out
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    static void Test3()
    {
        var xs = new[] { 4, 7, 9 };
        var limits = FindMinMax(xs);
        Console.WriteLine($"Limits of [{string.Join(" ", xs)}] are {limits.min} and {limits.max}");
        // Output:
        // Limits of [4 7 9] are 4 and 9

        var ys = new[] { -9, 0, 67, 100 };
        var (minimum, maximum) = FindMinMax(ys);
        Console.WriteLine($"Limits of [{string.Join(" ", ys)}] are {minimum} and {maximum}");
        // Output:
        // Limits of [-9 0 67 100] are -9 and 100

        (int min, int max) FindMinMax(int[] input)
        {
            if (input is null || input.Length == 0)
            {
                throw new ArgumentException("Cannot find minimum and maximum of a null or empty array.");
            }

            var min = int.MaxValue;
            var max = int.MinValue;
            foreach (var i in input)
            {
                if (i < min)
                {
                    min = i;
                }

                if (i > max)
                {
                    max = i;
                }
            }

            return (min, max);
        }
    }

    /// <summary>
    /// 令人迷惑的名称
    /// </summary>
    static void Test4()
    {
        var t = (Sum: 4.5, Count: 3);
        Console.WriteLine($"Sum of {t.Count} elements is {t.Sum}.");

        (double Sum, int Count) d = (4.5, 3);
        Console.WriteLine($"Sum of {d.Count} elements is {d.Sum}.");

        // 元组在使用时可以指定任意名称，编译器只要能够翻译得懂，就可以把名称转移为item1 item2 .... 实际名称不会出现在IL中。
        // 另外如果方法返回元组，那名称会写入元数据中，这样调用方法的地方获取返回值可以使用该方法定义的参数名。（避免out代码示例中有演示） 
    }

    /// <summary>
    /// 赋值操作
    /// </summary>
    static void Test5()
    {
        (int, double) t1 = (17, 3.14);
        (double First, double Second) t2 = (0.0, 1.0);
        t2 = t1; // newobj       instance void valuetype [System.Runtime]System.ValueTuple`2<float64, float64>::.ctor(!0/*float64*/, !1/*float64*/)
        Console.WriteLine($"{nameof(t2)}: {t2.First} and {t2.Second}");
        // Output:
        // t2: 17 and 3.14

        (double A, double B) t3 = (2.0, 3.0);
        t3 = t2;
        Console.WriteLine($"{nameof(t3)}: {t3.A} and {t3.B}");
        // Output:
        // t3: 17 and 3.14

        // 在括号内指定类型
        var t = ("post office", 3.6);
        (string destination, double distance) = t;
        Console.WriteLine($"Distance to {destination} is {distance} kilometers.");
        // Output:
        // Distance to post office is 3.6 kilometers.
    }
}