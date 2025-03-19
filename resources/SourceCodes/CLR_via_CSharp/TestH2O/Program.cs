public class H2O
{
    // 用于控制氢原子数量的信号量
    private readonly SemaphoreSlim m_hydrogenSemaphore;

    // 用于控制氧原子数量的信号量
    private readonly SemaphoreSlim m_oxygenSemaphore;

    // 用于同步的互斥锁
    private readonly object m_lockObject;

    // 计数器
    private int m_hydrogenCount;

    public H2O()
    {
        m_hydrogenSemaphore = new SemaphoreSlim(2);
        m_oxygenSemaphore = new SemaphoreSlim(1);
        m_lockObject = new object();
        m_hydrogenCount = 0;
    }

    public void Hydrogen(Action releaseHydrogen)
    {
        m_hydrogenSemaphore.Wait(); 

        lock (m_lockObject)
        {
            releaseHydrogen();
            m_hydrogenCount++;

            if (m_hydrogenCount == 2)
            {
                m_oxygenSemaphore.Release();
                m_hydrogenCount = 0;
            }
        }
    }

    public void Oxygen(Action releaseOxygen)
    {
        m_oxygenSemaphore.Wait();

        lock (m_lockObject)
        {
            releaseOxygen();
            m_hydrogenSemaphore.Release(2);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        TestH2O("HOH");
        TestH2O("OOHHHH");
    }

    static void TestH2O(string input)
    {
        Console.WriteLine($"输入序列: {input}");

        var h2o = new H2O();
        var tasks = new List<Task>();

        foreach (char c in input)
        {
            if (c == 'H')
            {
                tasks.Add(Task.Run(() =>
                {
                    h2o.Hydrogen(() =>
                    {
                        Console.Write("H");
                    });
                }));
            }
            else if (c == 'O')
            {
                tasks.Add(Task.Run(() =>
                {
                    h2o.Oxygen(() =>
                    {
                        Console.Write("O");
                    });
                }));
            }
        }

        Task.WaitAll(tasks.ToArray());
        Console.WriteLine("\n完成");
    }
}