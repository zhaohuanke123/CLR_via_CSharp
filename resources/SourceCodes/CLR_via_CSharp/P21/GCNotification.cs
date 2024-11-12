using System;
using System.Threading;

namespace P21
{
    public static class GCNotification
    {
        private static Action<Int32> s_gcDone = null; // 事件的字段

        public static event Action<Int32> GCDone
        {
            add
            {
                // 如果之前没有登记的委托，就开始报告通知
                if (s_gcDone == null)
                {
                    new GenObject(0);
                    new GenObject(2);
                }

                s_gcDone += value;
            }
            remove { s_gcDone -= value; }
        }

        public sealed class GenObject
        {
            private Int32 m_generation;

            public GenObject(Int32 generation)
            {
                m_generation = generation;
            }

            ~GenObject()
            {
                // 这是 Finalize 方法        
                // 如果这个对象在我们希望的(或更高的)代中，
                // 就通知委托一次 GC 刚刚完成
                if (GC.GetGeneration(this) >= m_generation)
                {
                    Action<Int32> temp = Volatile.Read(ref s_gcDone);
                    if (temp != null) temp(m_generation);
                }

                // 如果至少还有一个已登记的委托，而且 AppDomain 并非正在卸载，
                // 而且进程并非正在关闭，就继续报告通知
                if ((s_gcDone != null)
                    && !AppDomain.CurrentDomain.IsFinalizingForUnload()
                    && !Environment.HasShutdownStarted)
                {
                    // 对于第 0 代，创建一个新对象；对于第 2 代，复活对象，
                    // 使第 2 代在下次回收时，GC 会再次调用 Finalize
                    if (m_generation == 0) new GenObject(0);
                    else GC.ReRegisterForFinalize(this);
                }
                else
                {
                    /* 放过对象，让其被回收 */
                }
            }
        }
    }
}