public class FooBarMonitor
{
    private int n;
    private object m_lock = new object();
    private bool m_isFooTurn = true;

    public FooBarMonitor(int n)
    {
        this.n = n;
    }

    public void Foo(Action printFoo)
    {
        for (int i = 0; i < n; i++)
        {
            lock (m_lock)
            {
                while (!m_isFooTurn)
                {
                    Monitor.Wait(m_lock);
                }

                printFoo();
                m_isFooTurn = false;
                Monitor.Pulse(m_lock);
            }
        }
    }

    public void Bar(Action printBar)
    {
        for (int i = 0; i < n; i++)
        {
            lock (m_lock)
            {
                while (m_isFooTurn)
                {
                    Monitor.Wait(m_lock);
                }

                printBar();
                m_isFooTurn = true;
                Monitor.Pulse(m_lock);
            }
        }
    }
}

public class FooBarSemaphoreSlim
{
    private int n;
    private SemaphoreSlim m_fooSem = new SemaphoreSlim(1, 1);
    private SemaphoreSlim m_barSem = new SemaphoreSlim(0, 1);

    public FooBarSemaphoreSlim(int n)
    {
        this.n = n;
    }

    public void Foo(Action printFoo)
    {
        for (int i = 0; i < n; i++)
        {
            m_fooSem.Wait();
            printFoo();
            m_barSem.Release();
        }
    }

    public void Bar(Action printBar)
    {
        for (int i = 0; i < n; i++)
        {
            m_barSem.Wait();
            printBar();
            m_fooSem.Release();
        }
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("fooBarMonitor");
        var fooBarMonitor = new FooBarMonitor(5);
        Parallel.Invoke(
            () => { fooBarMonitor.Foo(() => Console.WriteLine("Foo")); },
            () => { fooBarMonitor.Bar(() => Console.WriteLine("Bar")); });

        Console.WriteLine("fooBarSemaphoreSlim");
        var fooBarSemaphoreSlim = new FooBarSemaphoreSlim(5);
        Parallel.Invoke(
            () => { fooBarSemaphoreSlim.Foo(() => Console.WriteLine("Foo")); },
            () => { fooBarSemaphoreSlim.Bar(() => Console.WriteLine("Bar")); });
    }
}