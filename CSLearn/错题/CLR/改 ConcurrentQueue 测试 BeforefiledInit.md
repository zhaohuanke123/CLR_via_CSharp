
 ```cs
    class TestCtor
    {
        public TestCtor()
        {
            var stackTrace = new StackTrace();
            var mb = stackTrace.GetFrame(1).GetMethod();
            var type = mb.DeclaringType;
            var typeName = type.Name;

            var s = "当前类: " + typeName + "   当前线程ID : " + Thread.CurrentThread.ManagedThreadId;
            Program.que.Enqueue(s);
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

    internal partial class Program
    {
        public static ConcurrentQueue<string> que = new ConcurrentQueue<string>();

        public static void Main(string[] args)
        {
            _ = NotBefore.testCtor;
            que.Enqueue("-----------------");
            _ = BeforeInit.testCtor;
            que.Enqueue("-----------------");
            _ = Class1.testCtor;
            que.Enqueue("-----------------");
            
            foreach (var item in que)
            {
                Console.WriteLine(item);
            }
        }
    }
```

Framework 4.6.2 下，输出：

```
当前类: BeforeInit   当前线程ID : 1
当前类: Class1   当前线程ID : 1
当前类: NotBefore   当前线程ID : 1
-----------------
-----------------
-----------------
```

.Net Core 8下，输出：

```
当前类: NotBefore   当前线程ID : 1
-----------------
当前类: BeforeInit   当前线程ID : 1
-----------------
当前类: Class1   当前线程ID : 1
-----------------

```