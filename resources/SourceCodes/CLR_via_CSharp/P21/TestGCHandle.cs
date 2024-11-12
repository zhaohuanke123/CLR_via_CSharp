using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace P21
{
    public class TestGCHandle
    {
        public static void Run()
        {
            Object o = new Object().GCWatch("My Object created at " + DateTime.Now);
            GC.Collect(); // 此时看不到 GC 通知
            GC.KeepAlive(o); // 确定 o 引用的对象现在还活着
            o = null; // o 引用的对象现在可以死了

            GC.Collect(); // 此时才会看到 GC 通知
            Console.ReadLine();
        }
    }


    internal static class GCWatcher
    {
        // 注意：由于字符串留用(interning) 和 MarshalByRefObject 代理对象，所以
        // 使用 String 要当心
        public readonly static ConditionalWeakTable<Object, NotifyWhenGCd<String>> s_cwt =
            new ConditionalWeakTable<object, NotifyWhenGCd<string>>();

        internal sealed class NotifyWhenGCd<T>
        {
            private readonly T m_value;

            internal NotifyWhenGCd(T value)
            {
                m_value = value;
            }

            public override string ToString()
            {
                return m_value.ToString();
            }

            ~NotifyWhenGCd()
            {
                Console.WriteLine(DateTime.Now);
                Console.WriteLine("GC'd: " + m_value);
            }
        }

        public static T GCWatch<T>(this T @object, String tag) where T : class
        {
            s_cwt.Add(@object, new NotifyWhenGCd<String>(tag));
            return @object;
        }
    }
}