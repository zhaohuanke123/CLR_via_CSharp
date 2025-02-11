using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestBarrier
{
    internal class Program
    {
        public static void Main(string[] args)
        {
        }

        /// <summary>
        /// 分阶段并行算法
        /// </summary>
        /// <param name="i"></param>
        /// <param name="phase"></param>
        private static void Test1()
        {
            var barrier = new Barrier(3, b =>
                Console.WriteLine($"阶段 {b.CurrentPhaseNumber} 完成"));

            Parallel.For(0, 3, i =>
            {
                for (int phase = 0; phase < 3; phase++)
                {
                    ProcessPhaseData(i, phase);
                    barrier.SignalAndWait(); // 同步点
                }
            });
        }

        private static void ProcessPhaseData(int i, int phase)
        {
            Console.Out.WriteLine($"i : {i}, phase : {phase}");
        }
    }
}