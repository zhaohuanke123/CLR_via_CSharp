internal class Program
{
    static void Main(string[] args)
    {
    }
}

/// <summary>
///  接口可以包含实例方法、属性、事件、索引器
///  接口可以包含 静态构造器、字段、常量或运算符
///  C# 11 开始 非字段接口成员可以是static abstract
///  接口成员可以是任意访问修饰符，如果是private 则必须有默认的实现
///  默认接口方法是virtual。没有方法体的接口成员默认是abstract
/// </summary>
public interface IInterface
{
    /// <summary>
    /// 静态构造器
    /// </summary>
    static IInterface()
    {
    }

    /// <summary>
    ///   .method public hidebysig virtual newslot abstract instance void
    /// </summary>
    void Test4Public(); // public void Test4Public

    /// <summary>
    ///  .method public hidebysig virtual newslot instance void
    /// </summary>
    void Test4PublicDefault()
    {
    }

    /// <summary>
    /// 受保护的接口成员
    ///  .method family hidebysig virtual newslot abstract instance void
    /// </summary>
    protected void Test4Protected();

    /// <summary>
    /// 本程序集子类调用
    /// </summary>
    private protected void Test4PrivateProtected();

    /// <summary>
    /// 本程序集或子类
    /// </summary>
    protected internal void Test4ProtectInternal();

    /// <summary>
    /// .method private hidebysig instance void
    /// </summary>
    private void Test4PrivateDefault()
    {
        int i = 0;
        i++;
    }

    /// <summary>
    /// 接口中声明的静态成员 不会被实现的类型所继承。
    /// </summary>
    public static int s_i;
}

public interface IInterface2 : IInterface
{
    void Test1()
    {
        Test4Public();
        Test4PublicDefault();
        Test4ProtectInternal();
        Test4Protected();
        Test4PrivateProtected();
    }

    private void Test2()
    {
        Test4Public();
        Test4PublicDefault();
        Test4ProtectInternal();
        Test4Protected();
        Test4PrivateProtected();
    }

    protected void Test3()
    {
        Test4Public();
        Test4PublicDefault();
        Test4ProtectInternal();
        Test4Protected();
        Test4PrivateProtected();
    }

    private protected void Test4()
    {
        Test4Public();
        Test4PublicDefault();
        Test4ProtectInternal();
        Test4Protected();
        Test4PrivateProtected();
    }

    protected internal void Test5()
    {
        Test4Public();
        Test4PublicDefault();
        Test4ProtectInternal();
        Test4Protected();
        Test4PrivateProtected();
    }
}

/// <summary>
/// </summary>
public class SomeType : IInterface
{
    public void Test()
    {
        ((IInterface)this).Test4ProtectInternal();
        ((IInterface)this).Test4PublicDefault();
        ((IInterface)this).Test4Public();

        // 接口中定义的 protected 、private protected 掉不了
    }

    private void Test2()
    {
        ((IInterface)this).Test4ProtectInternal();
        ((IInterface)this).Test4PublicDefault();
        ((IInterface)this).Test4Public();

        // 接口中定义的 protected 、private protected 掉不了
    }

    public void Test4Public()
    {
        throw new NotImplementedException();
    }

    void IInterface.Test4Protected()
    {
        throw new NotImplementedException();
    }

    void IInterface.Test4PrivateProtected()
    {
        throw new NotImplementedException();
    }

    void IInterface.Test4ProtectInternal()
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// 外部访问成员例子
/// </summary>
internal class InterfaceAccessModifier
{
    static void Test()
    {
        IInterface i = new SomeType();
        i.Test4PublicDefault();
        i.Test4ProtectInternal();
        i.Test4Public();
    }
}