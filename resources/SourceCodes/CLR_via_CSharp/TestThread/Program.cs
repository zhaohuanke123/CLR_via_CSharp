using System;
using System.Threading;
using System.Collections.Generic;

class Program
{
    // 创建一个线程类，用于模拟不同优先级的线程
    class PriorityThread
    {
        public int Priority { get; set; }
        public string Name { get; set; }
        public Thread Thread { get; set; }

        public PriorityThread(int priority, string name)
        {
            Priority = priority;
            Name = name;
            Thread = new Thread(ThreadProc);
            Thread.Priority = (ThreadPriority)priority;
        }

        // 模拟线程执行的任务
        private void ThreadProc()
        {
            Console.WriteLine($"线程 {Name}（优先级：{Priority}）开始执行.");
            int counter = 0;
            while (counter < 5)
            {
                // 模拟线程工作
                Thread.Sleep(1000); // 每秒打印一次
                Console.WriteLine($"线程 {Name} 执行中...");
                counter++;
            }

            Console.WriteLine($"线程 {Name}（优先级：{Priority}）结束执行.");
        }
    }

    static void Main()
    {
        // 创建线程池，线程优先级从 0 到 31
        List<PriorityThread> threads = new List<PriorityThread>();

        // 优先级为 31 的线程
        for (int i = 0; i < 3; i++)
        {
            threads.Add(new PriorityThread(31, $"高优先级线程 {i + 1}"));
        }

        // 优先级为 1 和 2 的线程（模拟低优先级）
        for (int i = 0; i < 3; i++)
        {
            threads.Add(new PriorityThread(1, $"低优先级线程 {i + 1}"));
        }

        // 启动所有线程
        foreach (var t in threads)
        {
            t.Thread.Start();
        }

        // 等待线程执行完成
        foreach (var t in threads)
        {
            t.Thread.Join();
        }

        Console.WriteLine("所有线程执行完毕.");
    }
}