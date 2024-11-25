# 1 类型对象的创建时机和类型构造器的调用时机

在一个方法执行前，CLR会检测出方法引用的类型，保证该类型（无论是值类型或者引用类型）的一个内部数据结构（即类型对象）已经创建好（但未调用类型构造器初始化静态字段）, 就是说当方法引用到第一次引用到的类型就会创建对应的类型对象。

> "The execution of a static constructor is triggered by the first of the following events to occur within an application domain: (1) An instance of the class is created. (2) Any of the static members of the class are referenced."

根据文档，在类型构造器的调用时机在 类型实例第一次被创建时或者第一次访问类的静态成员

测试如下：
```csharp
  internal class Class1
  {
      public static int x;

      static Class1()
      {
          Console.WriteLine("Class1 static constructor");
      }
  }

  internal class Class2
  {
      public static int x;

      static Class2()
      {
          Console.WriteLine("Class2 static constructor");
      }
  }

```

测试1
```csharp
 static void TestNew()
 {
     Class1 c1 = new Class1();
     Console.WriteLine("-------------" + c1);
     Class2 c2 = new Class2();
     Console.WriteLine("-------------" + c2);
 }
// 输出
// Class1 static constructor
//-------------TestTypeObject.Class1
//Class2 static constructor
//-------------TestTypeObject.Class2
```

测试2

```csharp

   static void TestStaticField()
   {
       Class1.x = 1;
       Console.WriteLine("-------------");
       Class2.x = 2;
       Console.WriteLine("-------------");
   }
//Class1 static constructor
//-------------
//Class2 static constructor
//-------------
```

对于值类型来说，显示 new一个值类型，编译并不会生成调用构造函数的代码，此时不会调用类型构造器，只有但显示new 调用有参数构造器或者显示new无参构造后再调用类型里的方法，或者是同引用类型一样访问静态字段才会调用类型构造器。

测试如下：
```csharp
    struct Struct1
    {
        static Struct1()
        {
            Console.WriteLine($"{nameof(Struct1)} static constructor");
        }

        public Struct1(int a)
        {
        }

        public override string ToString()
        {
            return "Struct1";
        }
    }


```

```csharp
        static void TestStruct()
        {
            Struct1 s1 = new Struct1();
            Console.WriteLine("-------------" + s1);
        }
//输出：
//-------------Struct1
```

```csharp
        static void TestStructHasParameterizedConstructor()
        {
            Struct1 s1 = new Struct1(1);
            Console.WriteLine("-------------" + s1);
        }

// 输出：
//Struct1 static constructor
//-------------Struct1

```

# 2 程序集清单文件中的清单是元数据吗

是元数据。

清单是程序集的描述信息，全称清单元数据表描述了哪些文件是程序集的一部分，具体包含

- AssemblyDef 标识一个程序集
- FileDef 标识除了清单文件本身包含的模块文件和资源文件，记录文件信息...
- ManifestResourceDef 标识程序集包含的资源文件，记录资源名称信息...
- ExportedTypesDef 导出public的类型

# 3 ModuleDef 和 ModuleRef的作用，记录了什么信息


- ModuleDef：一条记录项，模块的文件和扩展名和一个ID
- ModuleRef：模块中引用模块的记录项，文件名和扩展名

# 4 私有部署和公有部署

程序集部署位置的区别
- 私有：应用程序的某个目录，不被其他应用程序共享
- 公有：公共位置，可被共享

# 5 Main方法里面一个DoSamething，CLR查找类型元数据的过程

大致过程：IL -> MemberRef -> TypeRef

1. 从IL指令 call xxx.DoSamething 开始，这条指令是一个定位token，指示了MemberRef的第几个记录项
2. 检查MemberRef，找到TypeRef的一项
3. 根据TypeRef指示的不同情况加载程序集|模块，再创建类型的内部数据结构

# 6 Object 的Equal 和 GetHashCode方法的默认实现


看.Net 6.0.6  源码的实现

## Equal的实现

```csharp
    public partial class Object
        // Returns a boolean indicating if the passed in object obj is
        // Equal to this.  Equality is defined as object equality for reference
        // types and bitwise equality for value types using a loader trick to
        // replace Equals with EqualsValue for value types).
        public virtual bool Equals(object? obj)
        {
            return RuntimeHelpers.Equals(this, obj);
        }
	}
```

```csharp
// RuntimeHelpers.CoreCLR.cs文件
    public static partial class RuntimeHelpers
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern new bool Equals(object? o1, object? o2);
	}
```

以下引用内容来自GitHub Coplot 的解释

>在 .NET 中，带有 `[MethodImpl(MethodImplOptions.InternalCall)]` 特性的静态方法表示该方法是一个内部调用（Internal Call），其实现位于运行时的本机代码中，而不是托管代码中。这些方法通常在 CoreCLR 的源代码中实现。
 查找 Internal Call 方法的实现
 >要找到 [Equals(object? o1, object? o2)](vscode-file://vscode-app/c:/Users/zhaohuanke/AppData/Local/Programs/Microsoft%20VS%20Code/resources/app/out/vs/code/electron-sandbox/workbench/workbench.html) 方法的实现，我们需要查看 CoreCLR 的源代码，特别是与内部调用相关的部分。以下是查找步骤：
>1. **查找 Internal Call 表**：Internal Call 方法的映射通常在一个表中定义，该表将托管方法映射到本机实现。
>2. **查找本机实现**：找到映射后，查看对应的本机实现文件。
>查找 Internal Call 表
>Internal Call 表通常定义在 `ecalllist.h` 文件中，该文件包含了所有 Internal Call 方法的映射。
 >`ecalllist.h`
>在 CoreCLR 项目中，`ecalllist.h` 文件定义了 Internal Call 方法的映射。我们可以在该文件中查找 [Equals](vscode-file://vscode-app/c:/Users/zhaohuanke/AppData/Local/Programs/Microsoft%20VS%20Code/resources/app/out/vs/code/electron-sandbox/workbench/workbench.html) 方法的映射。

所以再找到 ecalllist.h文件
```cpp

FCFuncStart(gRuntimeHelpers)
    FCFuncElement("GetObjectValue", ObjectNative::GetObjectValue)
    FCIntrinsic("InitializeArray", ArrayNative::InitializeArray, CORINFO_INTRINSIC_InitializeArray)
    QCFuncElement("RunClassConstructor", ReflectionInvocation::RunClassConstructor)
    QCFuncElement("RunModuleConstructor", ReflectionInvocation::RunModuleConstructor)
    QCFuncElement("CompileMethod", ReflectionInvocation::CompileMethod)
    QCFuncElement("PrepareMethod", ReflectionInvocation::PrepareMethod)
    FCFuncElement("PrepareDelegate", ReflectionInvocation::PrepareDelegate)
    FCFuncElement("GetHashCode", ObjectNative::GetHashCode)
    FCFuncElement("Equals", ObjectNative::Equals) //  在这里
    FCFuncElement("AllocateUninitializedClone", ObjectNative::AllocateUninitializedClone)
    FCFuncElement("EnsureSufficientExecutionStack", ReflectionInvocation::EnsureSufficientExecutionStack)
    FCFuncElement("TryEnsureSufficientExecutionStack", ReflectionInvocation::TryEnsureSufficientExecutionStack)
    QCFuncElement("GetUninitializedObject", ReflectionSerialization::GetUninitializedObject)
    QCFuncElement("AllocateTypeAssociatedMemory", RuntimeTypeHandle::AllocateTypeAssociatedMemory)
    FCFuncElement("AllocTailCallArgBuffer", TailCallHelp::AllocTailCallArgBuffer)
    FCFuncElement("GetTailCallInfo", TailCallHelp::GetTailCallInfo)
FCFuncEnd()
```

```cpp

FCIMPL2(FC_BOOL_RET, ObjectNative::Equals, Object *pThisRef, Object *pCompareRef)
{
    CONTRACTL
    {
        FCALL_CHECK;
        INJECT_FAULT(FCThrow(kOutOfMemoryException););
    }
    CONTRACTL_END;

    if (pThisRef == pCompareRef)
        FC_RETURN_BOOL(TRUE);

    // Since we are in FCALL, we must handle NULL specially.
    if (pThisRef == NULL || pCompareRef == NULL)
        FC_RETURN_BOOL(FALSE);

    MethodTable *pThisMT = pThisRef->GetMethodTable();

    // If it's not a value class, don't compare by value
    if (!pThisMT->IsValueType())
        FC_RETURN_BOOL(FALSE);

    // Make sure they are the same type.
    if (pThisMT != pCompareRef->GetMethodTable())
        FC_RETURN_BOOL(FALSE);

    // Compare the contents (size - vtable - sync block index).
    DWORD dwBaseSize = pThisMT->GetBaseSize();
    if(pThisMT == g_pStringClass)
        dwBaseSize -= sizeof(WCHAR);
    BOOL ret = memcmp(
        (void *) (pThisRef+1),
        (void *) (pCompareRef+1),
        dwBaseSize - sizeof(Object) - sizeof(int)) == 0;

    FC_GC_POLL_RET();

    FC_RETURN_BOOL(ret);
}
FCIMPLEND

```

以下引用内容来自GitHub Coplot 的解释
> `   ObjectNative::Equals` 方法通过以下步骤比较两个对象是否相等：
>1. **比较对象引用**：如果引用相同，返回 `TRUE`。
>2. **处理空引用**：如果任一对象为 `NULL`，返回 `FALSE`。
>3. **获取方法表**：获取对象的类型信息。
>4. **检查是否为值类型**：如果不是值类型，返回 `FALSE`。
>5. **检查类型是否相同**：如果类型不同，返回 `FALSE`。
>6. **比较对象内容**：使用 `memcmp` 比较对象的内存内容，如果相同，返回 `TRUE`，否则返回 `FALSE`。

对于引用类型比较对象的同一性，对于值类型（装箱成object），进行值类型未装箱部分的内存按位比较

## GetHashCode

```cpp

// Note that we obtain a sync block index without actually building a sync block.
// That's because a lot of objects are hashed, without requiring support for
FCIMPL1(INT32, ObjectNative::GetHashCode, Object* obj) {

    CONTRACTL
    {
        FCALL_CHECK;
        INJECT_FAULT(FCThrow(kOutOfMemoryException););
    }
    CONTRACTL_END;

    VALIDATEOBJECT(obj);

    if (obj == 0)
        return 0;

    OBJECTREF objRef(obj);

    {
        DWORD bits = objRef->GetHeader()->GetBits();

        if (bits & BIT_SBLK_IS_HASH_OR_SYNCBLKINDEX)
        {
            if (bits & BIT_SBLK_IS_HASHCODE)
            {
                // Common case: the object already has a hash code
                return  bits & MASK_HASHCODE;
            }
            else
            {
                // We have a sync block index. This means if we already have a hash code,
                // it is in the sync block, otherwise we generate a new one and store it there
                SyncBlock *psb = objRef->PassiveGetSyncBlock();
                if (psb != NULL)
                {
                    DWORD hashCode = psb->GetHashCode();
                    if (hashCode != 0)
                        return  hashCode;
                }
            }
        }
    }

    FC_INNER_RETURN(INT32, GetHashCodeHelper(objRef));
}
FCIMPLEND


```

以下引用内容来自GitHub Coplot 的解释
>1. **验证对象**：确保对象引用有效。
>2. **处理空对象引用**：如果对象引用为 `NULL`，返回 `0`。
>3. **获取对象头的位字段**：检查对象头是否已有哈希码或同步块索引。
>4. **检查现有哈希码**：如果对象已有哈希码，直接返回。
>5. **检查同步块中的哈希码**：如果对象有同步块索引，从同步块中获取哈希码并返回。
>6. **生成新的哈希码**：如果对象没有现有的哈希码，调用 [GetHashCodeHelper](vscode-file://vscode-app/c:/Users/zhaohuanke/AppData/Local/Programs/Microsoft%20VS%20Code/resources/app/out/vs/code/electron-sandbox/workbench/workbench.html) 方法生成一个新的哈希码并返回。


下面做一个测试实验辅助验证
```csharp
    internal class GetHashClass
    {
        private int a;
        private int b;
        public string s;

        public GetHashClass(int a, int b, string s)
        {
            this.a = a;
            this.b = b;
            this.s = s;
        }
    }
  internal class Program
    {
        public static void Main(string[] args)
        {
            GetHashClass a = new GetHashClass(1, 2, "hhhh");
			var hsah = a.GetHashCode(); // 断点调试到这一句时，查看a对象内存，同步块索引内容为全0
			// 跳过上面之后，同步块索引被修改成一个值，与返回值hash对比，hash变量是同步块索引的截断，应该还有一部分内容用于标记是否存储了哈希码
			Console.WriteLine(hsah);
        }
    }

```

修改一下代码，使用lock语法
```csharp
  internal class Program
    {
        public static void Main(string[] args)
        {

            GetHashClass a = new GetHashClass(1, 2, "hhhh");
            lock (a) // 这里会修改同步块索引
            {
                var hsah = a.GetHashCode();  // 再使用GetHashCode不会修改同步块索引
                Console.WriteLine(hsah);
            }
        }
    }
```

实验结果就是如果你的类型没有重写GetHashCode，此时调用GetHashCode会调用到Object默认的实现，在这个实现里面会吧hashcode的计算结果缓存到同步块索引里面（假如同步块索引没有用到），然后设置一个标记位，表示存储了hashcode。


# 7 同步块索引的作用

- 标记位，是否写入了同步块的索引，是否缓存了hashcode
- GC 可达标记
- 存储同步块数组的一个索引
- 如果没被用于同步，则调用Object的GetHashCode会用于把结果缓存起来



# 8 类型转换的IL 查看

```csharp
    class Base
    {
    }

    class Base2 : Base
    {
    }

    class Derived : Base2
    {
    }

    class Derived2 : Derived
    {
    }

  internal class Program
    {
        public static void Main(string[] args)
        {
            Base b = new Base();
            var d2 = (Derived2)b;
            Console.WriteLine(d2);
        }
    }
```

```csharp

  .method public hidebysig static void
    Main(
      string[] args
    ) cil managed
  {
    .entrypoint
    .maxstack 1
    .locals init (
      [0] class TestObjectCast.Base b,
      [1] class TestObjectCast.Derived2 d2
    )

    // [24 9 - 24 10]
    IL_0000: nop

    // [25 13 - 25 33]
    IL_0001: newobj       instance void TestObjectCast.Base::.ctor()
    IL_0006: stloc.0      // b

    // [26 13 - 26 34]
    IL_0007: ldloc.0      // b
    IL_0008: castclass    TestObjectCast.Derived2 // 类型转换 这里
    IL_000d: stloc.1      // d2

    // [27 13 - 27 35]
    IL_000e: ldloc.1      // d2
    IL_000f: call         void [mscorlib]System.Console::WriteLine(object)
    IL_0014: nop

    // [46 9 - 46 10]
    IL_0015: ret

  } // end of method Program::Main


```

（）显示类型转换将一个对象转换为子类或者接口时，会生成 castclass 某个类或者接口 的代码。

引用MSDN文档内容 [OpCodes.Castclass Field (System.Reflection.Emit) | Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.opcodes.castclass?view=netframework-4.6.2)
>The `castclass` instruction attempts to cast the object reference (type `O`) atop the stack to a specified class. The new class is specified by a metadata token indicating the desired class. If the class of the object on the top of the stack does not implement the new class (assuming the new class is an interface) and is not a derived class of the new class then an InvalidCastException is thrown. If the object reference is a null reference, `castclass` succeeds and returns the new object as a null reference.

castclass用于转换为指定的类型，失败就抛出异常InvalidCastException。


# 9 as 和类型转换的区别

同上一问部分代码，修改Main方法代码

```csharp
Base b = new Base();
d2 = b as Derived2;
Console.WriteLine(d2);
```

```csharp
  .method public hidebysig static void
    Main(
      string[] args
    ) cil managed
  {
    .entrypoint
    .maxstack 1
    .locals init (
      [0] class TestObjectCast.Base b,
      [1] class TestObjectCast.Derived2 d2
    )

    // [28 9 - 28 10]
    IL_0000: nop

    // [29 13 - 29 33]
    IL_0001: newobj       instance void TestObjectCast.Base::.ctor()
    IL_0006: stloc.0      // b

    // [33 13 - 33 36]
    IL_0007: ldloc.0      // b
    IL_0008: isinst       TestObjectCast.Derived2 // 生成对应代码 这里
    IL_000d: stloc.1      // d2

    // [34 13 - 34 35]
    IL_000e: ldloc.1      // d2
    IL_000f: call         void [mscorlib]System.Console::WriteLine(object)
    IL_0014: nop

    // [53 9 - 53 10]
    IL_0015: ret

  } // end of method Program::Main

```

as 会生成 isinst 指令

>Tests if an object reference is an instance of `class`, returning either a null reference or an instance of that class or interface.

进行Test是否是某个类型或者接口的实例（兼容性），失败返回null

## 测试速度对比

测试代码 (测试转换成功)
```csharp
   public static void Main(string[] args)
        {
            Base b = new Derived2();
            const int n = 1000000000;
            // var d2 = (Derived2)b;
            // Console.WriteLine(d2);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Derived2 d2 = null;
            for (int i = 0; i < n; i++)
            {
                d2 = (Derived2)b;
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);

            sw.Restart();
            for (int i = 0; i < n; i++)
            {
                d2 = b as Derived2;
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
//1566
//1380
```

如果加入try catch，第一部分代码就更慢了
```csharp
    try
	{
		d2 = (Derived2)b;
	}
	catch (InvalidCastException e)
	{
		Console.WriteLine(e);
	}

//2125
//1387
```

测试代码2

```csharp
  public static void Main(string[] args)
        {
            Base b = new Derived();
            const int n = 1000000; // 改少一点
            // var d2 = (Derived2)b;
            // Console.WriteLine(d2);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Derived2 d2 = null;
            for (int i = 0; i < n; i++)
            {
                try
                {
                    d2 = (Derived2)b; // 转换失败
                }
                catch (InvalidCastException e)
                {
                }
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);

            sw.Restart();
            for (int i = 0; i < n; i++)
            {
                d2 = b as Derived2;
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
// 21283 转换失败，抛出异常捕获异常，效率较低
// 2

```

总体上as 优于 （）显示类型转换


# 10 压栈过程描述
```csharp

void Main(){
	int i;
	int j;
	Test(i, j);
}

int Test(int i,int j)
{
	return i + j;
}

```

Main方法中

执行到Test之前

| i   |
| --- |
| j   |
|     |
执行Test方法时

| i         |
| --------- |
| j         |
| [返回地址]    |
| i (方法参数)  |
| j (方法参数)  |
| i + j 的结果 |
|           |

# 11 对于值类型为什么不能定义无参构造器（.Net FrameWork）

 当创建值类型的实例时，C# 会自动将其字段初始化为默认值（数值类型为 `0`，引用类型为 `null`）。如果允许定义无参数构造函数，可能会导致开发者误以为该构造函数会在所有情况下被调用，进而对初始化行为产生误解。
 
比如 
```csharp
    struct MyStruct
    {
       // public MyStruct()
       // {
         //   Console.WriteLine("MyStruct ctor");
        //}
        public MyStruct(int a) {
	        
        }
    }

    internal class Program
    {
        public static void Main()
        {
            MyStruct[] arr = new MyStruct[10]; // 不会调用任何构造器
            MyStruct s = new MyStruct(); // 不会调用任何构造器
			MyStruct s = new MyStruct(1); // 调用有参构造器
        }
    }

```

# 12 类型对象分配到哪个堆上

引用20章 P416面 注解部分2

>这个托管堆内部又按照功能进行了不同的划分，其中最重要的就是 GC 堆和 Loader 堆，前者存储引用类型的实例，也就是会被垃圾回收机制”照顾“到的东西。而 Loader 堆负责存储类型的元数据，也就是所谓的“类型对象”。