## 1. C#编译器通过什么辨别一个类型，只是命名空间+类名吗

可以通过程序集标识+完整的类名（命名空间+名字）

测试如下：

### 程序集 Assembly1
```csharp
namespace Assembly1
{
    public class SomeType
    {
        public int i = 0;
    }
}
```

### 程序集 Assembly2

```csharp
namespace Assembly1
{
    public class SomeType
    {
        public int i = 0;
    }
}

```

### 程序集 TestAssembly

直接引用Assembly1，Assembly2，不做任何其他操作，会报错，这没问题。
>Ambiguous reference:
	Assembly1.SomeType
	Assembly1.SomeType
	match
```csharp
namespace TestAssembly
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Assembly1.SomeType st;
        }
    }
}
```

### 解决通过在项目配置中给两个程序集添加不同的外部别名

[外部别名 - C# reference | Microsoft Learn](https://learn.microsoft.com/zh-cn/dotnet/csharp/language-reference/keywords/extern-alias)
[命名空间别名运算符 - `::` 用于访问别名命名空间的成员。 - C# reference | Microsoft Learn](https://learn.microsoft.com/zh-cn/dotnet/csharp/language-reference/operators/namespace-alias-qualifier)

具体操作：
- 选择TestAssembly程序集
- 打开引用
- 选择要设置别名的程序集
- 吧别名global修改为其他的名字
- 修改测试代码
```csharp
extern alias Asm1;
extern alias Asm2;

namespace TestAssembly
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Asm1::Assembly1.SomeType st1 = new Asm1::Assembly1.SomeType();
            Asm2::Assembly1.SomeType st2 = new Asm2::Assembly1.SomeType();
        }
    }
}

```

确实说明编译器不只是通过命名空间+类名辨别一个类型

## 2 下面的代码，程序栈空间如何分配

```csharp
    struct MyStruct
    {
        // 没有成员
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            MyStruct ms = new MyStruct();
        }
    }
```

看一下IL
```cs
  .method private hidebysig static void
    Main(
      string[] args
    ) cil managed
  {
    .entrypoint
    .maxstack 1
    .locals init (
      [0] valuetype TestNoMemberStruct.MyStruct ms
    )

    // [15 9 - 15 10]
    IL_0000: nop

    // [16 13 - 16 42]
    IL_0001: ldloca.s     ms
    IL_0003: initobj      TestNoMemberStruct.MyStruct

    // [17 13 - 17 46]
    IL_0009: ldloca.s     ms
    IL_000b: constrained. TestNoMemberStruct.MyStruct
    IL_0011: callvirt     instance string [mscorlib]System.Object::ToString()
    IL_0016: call         void [mscorlib]System.Console::WriteLine(string)
    IL_001b: nop

    // [23 9 - 23 10]
    IL_001c: ret

  } // end of method Program::Main

```
这看不出啥

看下MyStruct的IL
```cs
.class private sealed sequential ansi beforefieldinit
  TestNoMemberStruct.MyStruct
    extends [mscorlib]System.ValueType
{
  .pack 0
  .size 1
} // end of class TestNoMemberStruct.MyStruct

```
可知道MyStruct的大小是1，这个跟C++类似，需要有自己的内存地址，不能重叠，所以大小至少要是1

看一下反汇编的赋值指令
```cs
	MyStruct ms = new MyStruct();
03010873 lea eax,[ebp-40h]
03010876 mov byte ptr [eax],0 将该地址处的 1 字节置为 0
```
从这两条指令可以看出，`MyStruct` 实际占用了 1 字节 的内存。


## 3 能访问未初始化的对象吗（值类型|引用类型）

以为是报警告
```cs
    internal class Program
    {
        static void Main(string[] args)
        {
            int i;
            object obj;
            Console.WriteLine(i);
            Console.WriteLine(obj);
        }
    }
//严重性	代码	说明	项目	文件	行	禁止显示状态	详细信息
//错误(活动)	CS0165	使用了未赋值的局部变量“i”	TestInit	//D:\Study\CLR_via_CSharp\resources\SourceCodes\TestInit\Program.cs	11		
//严重性	代码	说明	项目	文件	行	禁止显示状态	详细信息
//错误(活动)	CS0165	使用了未赋值的局部变量“obj”	TestInit	//D:\Study\CLR_via_CSharp\resources\SourceCodes\TestInit\Program.cs	12		

```

引用书上内容：
>
上述代码中， `SomeVal` 类型用 `struct` 声明，而不是用更常用的 `class`。在 C# 中，用 `struct` 声明的类型是值类型，用 `class` 声明的类型是引用类型。可以看出，引用类型和值类型的区别相当大。在代码中使用类型时，必须注意是引用类型还是值类型，因为这会极大地影响在代码中表达自己意图的方式。
>
上述代码中有这样一行：
>
>SomeVal vl = new SomeVal();    // 在栈上分配
因为这行代码的写法，似乎是要在托管堆上分配一个 `SomeVal` 实例。但 C# 编译器知道 `SomeVal` 是值类型，所以会生成正确的 IL 代码，在线程栈上分配一个 `SomeVal` 实例。 C# 还会确保值类型中的所有字段都初始化为零。
>
>上述代码还可以像下面这样写：
>SomeVal v1;    // 在栈上分配 
>这一行生成的 IL 代码也会在线程栈上分配实例，并将字段初始化为零。唯一的区别在于，如果使用 `new` 操作符，C# 会认为实例已初始化。以下代码更清楚地进行了说明：  
>
>// 这两行代码能通过编译，因为 C# 认为
// v1 的字段已初始化为 0
SomeVal v1 = new SomeVal();
Int32 a = v1.x;
>
>// 这两行代码不能通过编译，因为 C# 不认为
// v1 的字段已初始化为 0
SomeVal v1；
Int32 a = v1.x;   // error CS0170：使用了可能未赋值的字段 “x” 

下面内容总结自文章
[clr - C# 未初始化变量危险吗？ - Stack Overflow --- clr - Are C# uninitialized variables dangerous? - Stack Overflow](https://stackoverflow.com/questions/8931226/are-c-sharp-uninitialized-variables-dangerous)

>C# 强制要求在使用局部变量之前必须对其进行赋值。这是编译时检查，旨在防止使用未明确初始化的变量，从而减少潜在的错误。
>- 
>- 未初始化局部变量的运行时行为：虽然 C# 编译器要求显式赋值局部变量，但公共语言运行时（CLR）在运行时会将所有局部变量初始化为其默认值。然而，不建议依赖这种行为，因为：
    - 编译器的明确赋值分析旨在在编译时捕获潜在错误。
    - 依赖运行时初始化可能导致代码不够清晰，并且可能容易出错。

下面做个骚操作
```cs
    internal class Program
    {
        static void Main(string[] args)
        {
            int i = 0;
            object obj = null;

            Console.WriteLine(i);
            Console.WriteLine(obj);
        }
    }

```
编译后，使用dnspy工具，把初始化的代码删掉。

最后IL如下：
```cs
  .method private hidebysig static void
    Main(
      string[] args
    ) cil managed
  {
    .entrypoint
    .maxstack 1
    .locals init (
      [0] int32 V_0,
      [1] object V_1
    )

    // [9 9 - 9 10]
    IL_0000: nop
							// 初始化代码被我删掉了
    // [13 13 - 13 34]
    IL_0001: ldloc.0      // V_0
    IL_0002: call         void [mscorlib]System.Console::WriteLine(int32)
    IL_0007: nop

    // [14 13 - 14 36]
    IL_0008: ldloc.1      // V_1
    IL_0009: call         void [mscorlib]System.Console::WriteLine(object)
    IL_000e: nop

    // [15 9 - 15 10]
    IL_000f: ret

  } // end of method Program::Main

```

运行最后的exe文件，发现可以正常输出 0 和一个空行。

## 4 拆箱语法，编译器是否报错问题
```cs
public static void Test()
{
	long i = 10;
	object o = i;
	int j = (int)o; // 拆箱成不同的类型 
	Console.WriteLine(j);
}
```

我说不会报错，后面回来试了也是不会报错，乌龙问题，跳过

## 5 类型成员，类型可见性，成员可访问性，C#编译器和jit编译器检查

抄书：
- 类型成员有：常量，字段，实例构造器，类型构造器，方法，操作符重载，转换操作符，属性，事件，类型（嵌套类型）
- 类型可见性：public，internal 
	- 我说嵌套类可以private，（补充protected）
	- 祝哥说但是一般类型可见性不讨论这个。
- 成员可访问性：常用的private，protected，public，internal，其他的protect internal（or的结果，可有派生类（包含其他程序集），嵌套类，本程序集的其他类型访问）
- 访问性检查：C#编译器和jit编译器都会做

### 测试jit编译器检查如下
```cs
    internal class Program
    {
        struct MyStruct
        {
            public int a;
        }

        public static void Main(string[] args)
        {
            MyStruct myStruct = new MyStruct();
            Console.WriteLine(myStruct.a);
        }
    }

```

使用dnspy，修改 MyStruct的a访问性为private
```cs
// Token: 0x02000013 RID: 19  
.class nested private sequential ansi sealed beforefieldinit MyStruct    extends [mscorlib]System.ValueType  
{    // Fields    // Token: 0x04000006 RID: 6    
.field private int32 a  
  
} // end of class MyStruct
```

运行程序报错：
```
未经处理的异常:  System.FieldAccessException: 方法“Test.Program.Main(System.String[])”尝试访问字段“Test.Program+MyStruct.a”失败。
   在 Test.Program.Main(String[] args) 位置 D:\Study\CLR_via_CSharp\resources\SourceCodes\CLR_via_CSharp\Test\Program.cs:行号 15
```

## 6 partial 可以修饰什么，分布类可以在不同模块吗？

修饰类，接口，结构体（书上标题没好好看），（还有后面的分布方法）

### 测试 **`partial` 类** 能否在不同的模块中

```cs
// 文件名：PartialClassPart1.cs
// 编译为 PartialClassPart1.netmodule

public partial class PartialClass
{
    public string Part1Method()
    {
        return "This is from Part1.";
    }
}

```
编译命令：
`csc /target:module /out:PartialClassPart1.netmodule PartialClassPart1.cs`


```cs
// 文件名：PartialClassPart2.cs
// 编译为 PartialClassPart2.netmodule

public partial class PartialClass
{
    public string Part2Method()
    {
        return "This is from Part2.";
    }
}

```
编译命令：
`csc /target:module /out:PartialClassPart2.netmodule PartialClassPart2.cs`

### 文件3：`PartialClassTest.cs`（测试文件）

编译命令（链接两个模块）：
```cs
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


```
`csc /target:exe /addmodule:PartialClassPart1.netmodule,PartialClassPart2.netmodule /out:PartialClassTest.exe PartialClassTest.cs`

编译时报错：error CS0101: 命名空间 '\<global namespace>' 已经包含“PartialClass”的定义

## 7 Sealed 修饰符可以修饰直接写吗，直接修饰普通方法

```cs
internal class Program
{
	static void Main(string[] args)
	{
	}

	public sealed void Test()
	{

	}
}
//严重性	代码	说明	项目	文件	行	禁止显示状态	详细信息
// 错误(活动)	CS0238	'因为“Program.Test()”不是重写，所以无法将其密封	TestSealed	//D:\Study\CLR_via_CSharp\resources\SourceCodes\TestSealed\Program.cs	9		

```
会报错，只能用于重写的方法，替代override关键字

## 8 Sealed修饰类，调用方法，jit编译器会优化虚调用吗

测试环境 .net framework 4.6.2
测试代码
```cs
sealed class MSealedClass
{
	public void Test() 
	{
		Console.WriteLine(nameof(Test));
	}
	public override string ToString() 
	{
		return nameof(MSealedClass);
	}
}
internal class Program
{
	static void Main(string[] args)
	{
		MSealedClass mSealed = new MSealedClass();
		mSealed.Test(); // 普通方法
		var s = mSealed.ToString(); // 重写的虚方法
		Console.WriteLine(s);
		var hs = mSealed.GetHashCode(); // 没有重写的虚方法
		Console.WriteLine(hs);
	}
}
```

生成IL如下：
```cs

  .method private hidebysig static void
    Main(
      string[] args
    ) cil managed
  {
    .entrypoint
    .maxstack 1
    .locals init (
      [0] class TestSealed.MSealedClass mSealed,
      [1] string s,
      [2] int32 hs
    )

    // [19 9 - 19 10]
    IL_0000: nop

    // [20 13 - 20 55]
    IL_0001: newobj       instance void TestSealed.MSealedClass::.ctor()
    IL_0006: stloc.0      // mSealed

    // [21 13 - 21 28]
    IL_0007: ldloc.0      // mSealed
    IL_0008: callvirt     instance void TestSealed.MSealedClass::Test() //// 普通方法
    IL_000d: nop

    // [22 13 - 22 40]
    IL_000e: ldloc.0      // mSealed
    IL_000f: callvirt     instance string [mscorlib]System.Object::ToString()  // 重写的虚方法
    IL_0014: stloc.1      // s

    // [23 13 - 23 34]
    IL_0015: ldloc.1      // s
    IL_0016: call         void [mscorlib]System.Console::WriteLine(string)
    IL_001b: nop

    // [24 13 - 24 44]
    IL_001c: ldloc.0      // mSealed
    IL_001d: callvirt     instance int32 [mscorlib]System.Object::GetHashCode() // 没有重写的虚方法
    IL_0022: stloc.2      // hs

    // [25 13 - 25 35]
    IL_0023: ldloc.2      // hs
    IL_0024: call         void [mscorlib]System.Console::WriteLine(int32)
    IL_0029: nop

    // [26 9 - 26 10]
    IL_002a: ret

  } // end of method Program::Main
```

release 模式下，反汇编如下：
```cs
//mSealed.Test();

0544087D mov ecx,dword ptr [ebp-0Ch]

05440880 cmp dword ptr [ecx],ecx

05440882 call dword ptr [指针指向: TestSealed.MSealedClass.Test() (02C04E24h)]

//var s = mSealed.ToString();

05440888 mov ecx,dword ptr [ebp-0Ch]

0544088B mov eax,dword ptr [ecx]

0544088D mov eax,dword ptr [eax+28h]

05440890 call dword ptr [eax] // 虚调用

05440892 mov dword ptr [ebp-10h],eax 

//Console.WriteLine(s);

05440895 mov ecx,dword ptr [ebp-10h]

05440898 call System.Console.WriteLine(System.String) (6A904028h)

//var hs = mSealed.GetHashCode();

0544089D mov ecx,dword ptr [ebp-0Ch]

054408A0 mov eax,dword ptr [ecx]

054408A2 mov eax,dword ptr [eax+28h]

054408A5 call dword ptr [eax+8]  // 虚调用

054408A8 mov dword ptr [ebp-8],eax
```

看起来，普通方法会优化虚调用，虚方法不管有没有重写都是不是优化虚调用, 
跟普通的类优化行为没什么差别？

后续使用 Benchmark.NET 进行测试，发现
在.NetFramework 4.6.2下面，密封类调用重写的虚方法和非密封类调用重写的虚方法性能差不多

测试代码
```cs

namespace TestSealed
{
    public class SealedBenchmark
    {
        readonly NonSealedType nonSealedType = new NonSealedType();
        readonly SealedType sealedType = new SealedType();

        static void Main()
        {
            _ = BenchmarkRunner.Run<SealedBenchmark>();
        }

        [Benchmark(Baseline = true)]
        public void NonSealed()
        {
            nonSealedType.Method();
        }

        [Benchmark]
        public void Sealed()
        {
            sealedType.Method();
        }

    }

    internal class BaseType
    {
        public virtual void Method() { }
    }
    internal class NonSealedType : BaseType
    {
        public override void Method() { }
    }
    internal sealed class SealedType : BaseType
    {
        public override void Method() { }
    }

}

```

.Net Framework 4.6.2

| Method    | Mean      | Error     | StdDev    | Ratio |
|---------- |----------:|----------:|----------:|------:|
| NonSealed | 0.9833 ns | 0.0021 ns | 0.0017 ns |  1.00 |
| Sealed    | 0.9864 ns | 0.0079 ns | 0.0074 ns |  1.00 |

.Net 8

| Method    | Mean      | Error     | StdDev    | Median    | Ratio |
|---------- |----------:|----------:|----------:|----------:|------:|
| NonSealed | 0.2241 ns | 0.0013 ns | 0.0012 ns | 0.2236 ns | 1.000 |
| Sealed    | 0.0007 ns | 0.0010 ns | 0.0009 ns | 0.0004 ns | 0.003 |
明显.net 8 有优化了
## 9 Vs项目文件和编译生成的文件有什么，分别的作用的什么？

新建一个vs .Net Framework 项目，然后查看目录结构
### 目录结构
#### .vs
包含一些编辑器的设置和缓存。
#### bin
- **ARM64**  
	- **Debug**  
	- **Release**  
  ARM64 平台的输出目录。
- **Debug**  
  项目AnyCpu的 Debug 模式编译输出目录。
- **Release**  
  项目AnyCpu的 Release 模式编译输出目录。
- **x64**  
	- **Debug**  
	- **Release**  
  针对 64 位平台的编译输出目录。
- **x86**  
	- **Debug**  
	- **Release**  
  针对 32 位平台的编译输出目录。
#### obj
- **ARM64**  
	- **Debug**  
	- **Release**  
  ARM64 平台的中间文件目录。
- **Debug**  
  AnyCpu Debug 模式的中间文件目录。
- **Release**  
  AnyCpu Release 模式的中间文件目录。
- **x64**  
	- **Debug**  
	- **Release**  
  针对 64 位平台的中间文件目录。
- **x86**  
	- **Debug**  
	- **Release**  
  针对 32 位平台的中间文件目录。

在一个生成目录的Debug或者Release文件夹中有
1. **xxx.exe**：可运行的程序集
2. **xxx.exe.config**
   - 可执行文件的配置文件。
   - 由项目的 `App.config` 文件在编译时生成，包含应用程序的配置内容。
3. **TestFramework.pdb**
   - 程序数据库文件（Program Database File）。
   - 包含调试信息，如符号表和源代码信息，帮助开发人员在调试时定位代码中的错误。
#### Properties
- **AssemblyInfo.cs**  
  包含程序集的元信息，例如版本号、公司名称和产品名称等。
- **App.config**  
  项目的应用程序配置文件，用于定义应用程序的设置，例如数据库连接字符串和 AppSettings。
- **Program.cs**  
  项目源代码
- **TestFramework.csproj**  
  项目的主文件，包含项目的构建配置和依赖项信息，一个.net工程方面使用的
- **TestFramework.sln**  
  解决方案文件，包含该项目及其他子项目的引用，vs编辑器方面使用的

当我们在对一个项目点击生成或者运行时，vs会把 .csobj 里面依赖的项目都编译生成（有修改或者没编译过的话）

## 10 字段有什么修饰符

static：表明类型状态的一部分
readonly：类型实例字段，只读，只允许在构造器写入（可以多次）
volatile：编译器会假定代码单线程运行，并对代码进行优化，这个修饰可以阻止做这样的优化。

## 11 readonly 修饰字段，修饰结构体字段，可以修改结构体里成员吗

我的理解是 添加readonly之后，编译器会限制对 声明成员的内存空间的写入，
对于引用类型来说，声明成员的空间是一个指针的空间，不可再次对指针进行写入，但是可以写入引用类型的成员。
对于值类型，声明成员的空间包含了值类型里各个成员的字段，所以对这些字段也不能写入。

## 12 原问题：静态类有没有生成默认构造器

我理解成静态类可以有类型构造器吗，

原问题答案：静态类不会有默认构造器（实例），也不能定义实例构造器，但是可以有类型构造器。