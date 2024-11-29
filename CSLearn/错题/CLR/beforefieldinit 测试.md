
```cs
  class TestCtor
    {
        public TestCtor()
        {
            var stackTrace = new StackTrace();
            var typeName = stackTrace.GetFrame(1).GetMethod().DeclaringType.Name;

            Console.WriteLine("当前类: " + typeName);
        }
    }

    internal class BeforeInit
    {
        public static TestCtor testCtor = new TestCtor();
    }

    internal class Class1
    {
        public static TestCtor testCtor = new TestCtor();
    }

    internal class NotBefore
    {
        public static TestCtor testCtor;

        static NotBefore()
        {
            testCtor = new TestCtor();
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            _ = NotBefore.testCtor;
            Console.WriteLine("------------");
            _ = BeforeInit.testCtor;
            Console.WriteLine("------------");
            _ = Class1.testCtor;
            Console.WriteLine("------------");
        }
    }
```

.Net Framework  下面输出
```
当前类: BeforeInit
当前类: Class1
当前类: NotBefore
------------
------------
------------

```

.Net 6|8 下面输出

```
当前类: NotBefore
------------
当前类: BeforeInit
------------
当前类: Class1
------------
```

