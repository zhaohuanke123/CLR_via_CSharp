class Program
{
    static void Main()
    {
        Test();
    }

    private record Person2(string Name, int Age);

    /// <summary>
    /// With是浅拷贝，对于引用属性，只赋值对实例的引用
    /// </summary>
    private static void Test()
    {
        Person2 p = new("XiaoMing", 11);
        // 下面语法会先clone一个对象，然后进行赋值  clone 内部new一个实例
        Person2 q = p with { Name = "XiaoWang" };
        // 解构，获取p的字段
        var (name, age) = p; //  instance void NetStudy.Record/Person2::Deconstruct(string&, int32&)
    }
}

namespace NS1
{
    public record Person
    {
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
    };


// 创建只读的record struct，通过init
    public record struct Point
    {
        public double X { get; init; }
        public double Y { get; init; }
        public double Z { get; init; }
    }
}


namespace NS2
{
    // 创建record class的两种方式（只读）
    public record Person(string FirstName, string LastName);

// 创建只读的record struct
    public readonly record struct Point(double X, double Y, double Z);

// 创建可变的record struct（默认情况下 record struct 是可变的）
    public record struct DataMeasurement(DateTime TakenAt, double Measurement);

// 记录引用类型可以继承
    public record PersonChild(string FirstName, string LastName, string threeName) : Person(FirstName, LastName);
}
// record 只有面临，db或者内存中，面向一行数据记录的抽象的情形才应该被使用