using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal sealed class Type1
{
}

internal sealed class Type2
{
}

internal class Po
{
    private static async Task<String> MyMethodAsync(Int32 argument)
    {
        Int32 local = argument;
        try
        {
            Type1 result1 = await Method1Async();
            for (Int32 x = 0; x < 3; x++)
            {
                Type2 result2 = await Method2Async();
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Catch");
        }
        finally
        {
            Console.WriteLine("Finally");
        }

        return "Done";
    }

    private static async Task<Type1> Method1Async()
    {
        /* 以异步方式执行一些操作，最后返回一个 Type1 对象 */
        return null;
    }

    private static async Task<Type2> Method2Async()
    {
        /* 以异步方式执行一些操作，最后返回一个 Type2 对象 */
        return null;
    }
}

internal class Program
{
    private static void Main()
    {
    }

    [DebuggerStepThrough, AsyncStateMachine(typeof(StateMachine))]
    private static Task<String> MyMethodAsync(Int32 argument)
    {
        // 创建状态机实例并初始化它
        StateMachine stateMachine = new StateMachine()
        {
            // 创建 builder ，总这个存根方法返回 Task<String>
            // 状态机访问 builder 来设置 Task 完成/异常
            m_builder = AsyncTaskMethodBuilder<String>.Create(),
            m_state = -1, // 初始化状态机位置
            m_argument = argument // 将实参拷贝到状态机字段
        };

        // 开始执行状态机
        stateMachine.m_builder.Start(ref stateMachine);
        return stateMachine.m_builder.Task; // 返回状态机的 Task
    }

// 这是状态机结构
    [CompilerGenerated, StructLayout(LayoutKind.Auto)]
    private struct StateMachine : IAsyncStateMachine
    {
        // 代表状态机 builder(Task)及其位置的字段
        public AsyncTaskMethodBuilder<String> m_builder;
        public Int32 m_state;

        // 实参和局部变量现在成了字段 
        public Int32 m_argument, m_local, m_x;
        public Type1 m_resultType1;
        public Type2 m_resultType2;

        // 每个 awaiter 类型一个字段。
        // 任何时候这些字段只有一个是重要的，那个字段引用最近执行的、以异步方式完成的 await
        private TaskAwaiter<Type1> m_awaiterType1;
        private TaskAwaiter<Type2> m_awaiterType2;

        // 这是状态机方法本身
        void IAsyncStateMachine.MoveNext()
        {
            String result = null; // Task 的结果值

            // 编译器插入 try 块来确保状态机的任务完成
            try
            {
                Boolean executeFinally = true; // 先假定逻辑上离开 try 块
                if (m_state == -1)
                {
                    // 如果第一次在状态机方法中，
                    m_local = m_argument; // 原始方法就从头开始执行
                }

                // 原始代码中的 try 块
                try
                {
                    TaskAwaiter<Type1> awaiterType1 = default;
                    TaskAwaiter<Type2> awaiterType2 = default;

                    switch (m_state)
                    {
                        case -1: // 开始执行 try 块中的代码
                            // 调用 Method1Async 并获得它的 awaiter
                            awaiterType1 = Method1Async().GetAwaiter();
                            if (!awaiterType1.IsCompleted)
                            {
                                m_state = 0; // Method1Async 要以异步方式完成
                                m_awaiterType1 = awaiterType1; // 保存 awaiter 以便将来返回

                                // 告诉 awaiter 在操作完成时调用 MoveNext
                                m_builder.AwaitUnsafeOnCompleted(ref awaiterType1, ref this);
                                // 上述带按摩调用 awaiterType1 的 OnCompleted，它会在被等待的任务上
                                // 调用 ContinueWith(t => MoveNext())。
                                // 任务完成后，ContinueWith 任务调用 MoveNext

                                executeFinally = false; // 逻辑上不离开 try 块
                                return; // 线程返回至调用者
                            }

                            // Method1Async 以同步方式完成了
                            break;

                        case 0: // Method1Async 以异步方式完成了
                            awaiterType1 = m_awaiterType1; // 恢复最新的 awaiter
                            break;

                        case 1: // Method2Async 以异步方式完成了
                            awaiterType2 = m_awaiterType2; // 恢复最新的 awaiter
                            goto ForLoopEpilog;
                    }

                    // 在第一个 await 后, 我们捕捉结果并启动 for 循环
                    m_resultType1 = awaiterType1.GetResult(); // 获取 awaiter 的结果

                    ForLoopPrologue:
                    m_x = 0; // for 循环初始化
                    goto ForLoopBody; // 跳过 for 循环主体

                    ForLoopEpilog:
                    m_resultType2 = awaiterType2.GetResult();
                    m_x++; // 每次循环迭代都递增 x
                    // ↓↓ 直通到 for 循环主体 ↓↓

                    ForLoopBody:
                    if (m_x < 3)
                    {
                        // for 循环测试
                        // 调用 Method2Async 并获取它的 awaiter
                        awaiterType2 = Method2Async().GetAwaiter();
                        if (!awaiterType2.IsCompleted)
                        {
                            m_state = 1; // Method2Async 要以异步方式完成
                            m_awaiterType2 = awaiterType2; // 保存 awaiter 以便将来返回

                            // 告诉 awaiter 在操作完成时调用 MoveNext
                            m_builder.AwaitUnsafeOnCompleted(ref awaiterType2, ref this);
                            executeFinally = false; // 逻辑上不离开 try 块
                            return; // 线程返回至调用者
                        }

                        // Method2Async 以同步方式完成了
                        goto ForLoopEpilog; // 以同步方式完成就再次循环
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Catch");
                }
                finally
                {
                    // 只要线程物理上离开 try 就会执行 finally。
                    // 我们希望在线程逻辑上离开 try 时才执行这些代码
                    if (executeFinally)
                    {
                        Console.WriteLine("Finally");
                    }
                }

                result = "Done"; // 这是最终从异步函数返回的东西
            }
            catch (Exception exception)
            {
                // 未处理的异常：通常设置异常来完成状态机的 Task
                m_builder.SetException(exception);
                return;
            }

            // 无异常：通过返回结果来完成状态机的 Task
            m_builder.SetResult(result);
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            m_builder.SetStateMachine(stateMachine);
        }

        private static async Task<Type1> Method1Async()
        {
            /* 以异步方式执行一些操作，最后返回一个 Type1 对象 */
            return null;
        }

        private static async Task<Type2> Method2Async()
        {
            /* 以异步方式执行一些操作，最后返回一个 Type2 对象 */
            return null;
        }
    }
}