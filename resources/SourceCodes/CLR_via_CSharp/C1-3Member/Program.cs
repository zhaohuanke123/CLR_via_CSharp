// Assembly1.cs

using System.Runtime.CompilerServices;

class Program
{
    static void Main()
    {
        // TestMethod.Go();
        TestLocalFunc.Go();
        // derivedObject.myValue = 5;
        // derivedObject.myValue2 = 5;
    }
}

public class BaseClass
{
    // 只有在派生类与基类在同一程序集时，派生类才能够访问基类的private protected 成员
    private protected int myValue = 0;
}

/// <summary>
/// </summary>
public class DerivedClass1 : BaseClass
{
    void Access()
    {
        var baseObject = new BaseClass();

        // Error , because myValue can only be accessed by
        // classes derived from BaseClass.
        // baseObject.myValue = 5;

        // OK, accessed through the current derived class instance
        myValue = 5;
    }

    protected int myValue2 = 0;
}

/// <summary>
/// 定义一个值类型，拥有readonly成员
/// </summary>
public struct Vector2
{
    public float x;
    public float y;

    public readonly float GetLengthReadonly()
    {
        return MathF.Sqrt(LengthSquared);
    }

    public float GetLength()
    {
        return MathF.Sqrt(LengthSquared);
    }

    /// <summary>
    /// 只读实例成员 不能修改实例本身
    /// </summary>
    /// <returns></returns>
    public readonly float GetLengthIllegal()
    {
        var tmp = MathF.Sqrt(LengthSquared);

        // x = tmp;    // Compiler error, cannot write x
        // y = tmp;    // Compiler error, cannot write y

        return tmp;
    }

    public readonly float LengthSquared
    {
        get
        {
            return (x * x) +
                   (y * y);
        }
        // set { x = 1; y = 1 }
    }

    public readonly void GenericMethod<T>(T value) where T : struct
    {
    }

    // Allowed
    public readonly event Action<EventArgs> Event1
    {
        add { }
        remove { }
    }

    // Not allowed 
    // public readonly event Action<EventArgs> Event2;

    // public event Action<EventArgs> Event3
    // {
    //     readonly add { }
    //     readonly remove { }
    // }

    // 静态成员不能标记为readonly
    //public static readonly event Action<EventArgs> Event4
    //{
    //    add { }
    //    remove { }
    //}

    /// <summary>
    /// todo 自动实现属性中，get默认都是readonly的
    /// .custom instance void [System.Runtime]System.Runtime.CompilerServices.IsReadOnlyAttribute::.ctor()
    /// </summary>
    public int Prop5 { get; set; }
}

class TestMethod
{
    public static void Go()
    {
        Function(1, default);
        // Compiled to:
        // Function(1, default, "1", "default");

        int x = 1;
        TimeSpan y = TimeSpan.Zero;
        Function(x, y);
        // Compiled to:
        // Function(x, y, "x", "y");

        Function(int.Parse("2") + 1 + Math.Max(2, 3), TimeSpan.Zero - TimeSpan.MaxValue);
        // Compiled to:
        // Function(int.Parse("2") + 1 + Math.Max(2, 3), TimeSpan.Zero - TimeSpan.MaxValue,
        //    "int.Parse(\"2\") + 1 + Math.Max(2, 3)", "TimeSpan.Zero - TimeSpan.MaxValue");
    }

    private static void Function(int a, TimeSpan b, [CallerArgumentExpression("a")] string c = "",
        [CallerArgumentExpression("b")] string d = "")
    {
        Console.WriteLine($"Called with value {a} from expression '{c}'");
        Console.WriteLine($"Called with value {b} from expression '{d}'");
    }
}

public static class Verify
{
    public static void Argument(bool condition, string message,
        [CallerArgumentExpression("condition")]
        string conditionExpression = null)
    {
        if (!condition) throw new ArgumentException(message: message, paramName: conditionExpression);
    }

    public static void InRange(int argument, int low, int high,
        [CallerArgumentExpression("argument")] string argumentExpression = null,
        [CallerArgumentExpression("low")] string lowExpression = null,
        [CallerArgumentExpression("high")] string highExpression = null)
    {
        if (argument < low)
        {
            throw new ArgumentOutOfRangeException(paramName: argumentExpression,
                message: $"{argumentExpression} ({argument}) cannot be less than {lowExpression} ({low}).");
        }

        if (argument > high)
        {
            throw new ArgumentOutOfRangeException(paramName: argumentExpression,
                message: $"{argumentExpression} ({argument}) cannot be greater than {highExpression} ({high}).");
        }
    }

    public static void NotNull<T>(T argument,
        [CallerArgumentExpression("argument")] string argumentExpression = null)
        where T : class
    {
        if (argument == null) throw new ArgumentNullException(paramName: argumentExpression);
    }

    static T Single<T>(this T[] array)
    {
        Verify.NotNull(array); // paramName: "array"
        Verify.Argument(array.Length == 1, "Array must contain a single element."); // paramName: "array.Length == 1"

        return array[0];
    }

    static T ElementAt<T>(this T[] array, int index)
    {
        Verify.NotNull(array); // paramName: "array"
        // paramName: "index"
        // message: "index (-1) cannot be less than 0 (0).", or
        //          "index (6) cannot be greater than array.Length - 1 (5)."
        Verify.InRange(index, 0, array.Length - 1);

        return array[index];
    }
}

class TestLocalFunc
{
    public static void Go()
    {
        int c = 1;
        Console.WriteLine(Func(1, 2));

        int Func(int a, int b)
        {
            return a + b + c;
        }
    }
}

struct Point
{
}

/// <summary>
/// 允许重写派生程度更深的返回值类型
/// </summary>
internal class Covariant_returns
{
}

class Compilation
{
    public virtual Compilation WithOptions(Point options)
    {
        return null;
    }
}

class CSharpCompilation : Compilation
{
    public override CSharpCompilation WithOptions(Point options)
    {
        return null;
    }
}

namespace NS1
{
    public class Person1
    {
        public string FirstName { get; }

        // Omitted for brevity.
    }

    public class Person2
    {
        public string FirstName { get; init; }

        // Omitted for brevity.
    }

    public class TestType
    {
        static void Test()
        {
            Person1 p1 = new Person1()
            {
                // FirstName = "444444" // 无法在初始化项中对其赋值
            };
            Person2 p2 = new Person2()
            {
                FirstName = "444444" // 可以在初始化项中对其赋值
            };
        }
    }
}

namespace NS2
{
    public class Person2
    {
        public Person2()
        {
            FirstName4 = "fddd";
        }

        public string FirstName { get; init; }
        public string FirstName3 { get; set; }
        public string FirstName4;

        // Omitted for brevity.
    }

    public class TestType
    {
        static void Test()
        {
            Person2 p2 = new Person2()
            {
                FirstName = "444444", // 可以在初始化项中对其赋值
                FirstName3 = "ddd", // 一定要初始化
                FirstName4 = "ddd"
            };
        }
    }
}
// In 参数
// 如果将值类型定义为Readonly 值类型 或 访问的成员定义为 Readonly 成员 则可以避免防御性拷贝。
//
// 功能评价
// 在以in传递struct时必须要求struct是readonly的，不允许某些成员是readonly某些不是，因为这种情况下如果发生了拷贝就是潜在的逻辑错误
//
// 规范指导
// 推荐使用
//
// 在以in传递struct时必须要求struct是readonly
//
// 不允许将部分成员声明成readonly