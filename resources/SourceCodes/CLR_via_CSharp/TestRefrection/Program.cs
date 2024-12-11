using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TestRefrection
{
    public class C1
    {
    }

    public class C2
    {
    }

    public class C3
    {
    }

    internal class Program
    {
        public Program()
        {
            Console.WriteLine("Hello World!");
        }

        public static void Main(string[] args)
        {
            // TestAssembly.Go();
            TestTypeCanBeGC.Go();
            // TestCreateInstance.Go();
        }
    }

    class TestAssembly
    {
        public static void Go()
        {
            var assembly = Assembly.GetAssembly(typeof(Program));
            Console.WriteLine(assembly.GetType());
            foreach (var type in assembly.ExportedTypes)
            {
                Console.WriteLine(type);
            }
        }
    }

    class TestTypeCanBeGC
    {
        public static void Go()
        {
            var t = typeof(int);
            GCHandle handle = GCHandle.Alloc(t, GCHandleType.Weak);
            GCHandle handle1 = GCHandle.Alloc(new bool(), GCHandleType.Weak);

            GC.AddMemoryPressure(200000000);
            GC.Collect(2, GCCollectionMode.Forced);
            Console.WriteLine(handle.Target == null);
            Console.WriteLine(handle1.Target == null);
            Console.WriteLine((Type)handle.Target == typeof(int));
        }
    }

    class TestCreateInstance
    {
        public static void Go()
        {
            var t = typeof(C1);
            var c1 = Activator.CreateInstance(t);
            Console.WriteLine(c1.GetType());

            var c2 = Activator.CreateInstanceFrom(t.Assembly.Location, t.FullName).Unwrap();
            Console.WriteLine(c2.GetType());

            var constructorInfo = t.GetConstructor(new Type[0]);
            var o3 = constructorInfo.Invoke(new object[] { });
            Console.WriteLine(o3.GetType());
        }
    }
}