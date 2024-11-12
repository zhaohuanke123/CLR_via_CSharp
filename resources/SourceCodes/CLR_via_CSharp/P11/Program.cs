using System;

namespace P11
{
    internal class Program
    {
        public static void Main()
        {
            TypeWithLotsOfEvents twle = new TypeWithLotsOfEvents();

            // 添加一个回调
            twle.Foo += HandleFooEvent;

            // 证明确实可行
            twle.SimulateFoo();
        }

        private static void HandleFooEvent(object sender, FooEventArgs e)
        {
            Console.WriteLine("Handling Foo Event here...");
        }
    }
}