// Assembly1.cs

class Program
{
    static void Main()
    {
        var derivedObject = new DerivedClass1();
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