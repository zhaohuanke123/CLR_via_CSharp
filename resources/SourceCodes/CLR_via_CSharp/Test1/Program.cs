
public class FooMonitor
{
    private static readonly object m_lock = new object();
    private static int m_counter = 0;

    public FooMonitor()
    {
    }

    public void First(Action printFirst)
    {
        Monitor.Enter(m_lock);

        // printFirst() outputs "first". Do not change or remove this line.
        printFirst();

        m_counter = 1;
        Monitor.PulseAll(m_lock);
        Monitor.Exit(m_lock);
    }

    public void Second(Action printSecond)
    {
        Monitor.Enter(m_lock);

        while (m_counter != 1)
        {
            Monitor.Wait(m_lock);
        }

        // printSecond() outputs "second". Do not change or remove this line.
        printSecond();

        m_counter = 2;
        Monitor.PulseAll(m_lock);
        Monitor.Exit(m_lock);
    }

    public void Third(Action printThird)
    {
        Monitor.Enter(m_lock);

        while (m_counter != 2)
        {
            Monitor.Wait(m_lock);
        }

        // printThird() outputs "third". Do not change or remove this line.
        printThird();

        m_counter = 3;
        Monitor.PulseAll(m_lock);
        Monitor.Exit(m_lock);
    }
}

public class FooManualResetEvent
{
    ManualResetEvent mre1 = new ManualResetEvent(false);
    ManualResetEvent mre2 = new ManualResetEvent(false);

    public FooManualResetEvent()
    {
    }

    public void First(Action printFirst)
    {
        // printFirst() outputs "first". Do not change or remove this line.
        printFirst();
        mre1.Set();
    }

    public void Second(Action printSecond)
    {
        mre1.WaitOne();

        // printSecond() outputs "second". Do not change or remove this line.
        printSecond();

        mre2.Set();
    }

    public void Third(Action printThird)
    {
        mre2.WaitOne();
        // printThird() outputs "third". Do not change or remove this line.
        printThird();
    }
}

public class FooInterLock
{
    private int m_counter = 0;

    public FooInterLock()
    {
    }

    public void First(Action printFirst)
    {
        while (Interlocked.CompareExchange(ref m_counter, 1, 0) != 0)
        {
        }

        // printFirst() outputs "first". Do not change or remove this line.
        printFirst();
        Volatile.Write(ref m_counter, 2);
    }

    public void Second(Action printSecond)
    {
        while (Interlocked.CompareExchange(ref m_counter, 3, 2) != 2)
        {
        }

        // printSecond() outputs "second". Do not change or remove this line.
        printSecond();

        Volatile.Write(ref m_counter, 4);
    }

    public void Third(Action printThird)
    {
        while (Interlocked.CompareExchange(ref m_counter, 5, 4) != 4)
        {
        }

        // printThird() outputs "third". Do not change or remove this line.
        printThird();
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Foo Monitor");
        FooMonitor fooMonitor = new FooMonitor();
        Parallel.Invoke(
            () => { fooMonitor.Second(() => Console.WriteLine("Second")); },
            () => { fooMonitor.First(() => Console.WriteLine("First")); },
            () => { fooMonitor.Third(() => Console.WriteLine("Third")); }
        );

        Console.WriteLine("FooManualResetEvent");
        FooManualResetEvent fooManualResetEvent = new FooManualResetEvent();
        Parallel.Invoke(
            () => { fooManualResetEvent.First(() => Console.WriteLine("First")); },
            () => { fooManualResetEvent.Third(() => Console.WriteLine("Third")); },
            () => { fooManualResetEvent.Second(() => Console.WriteLine("Second")); }
        );

        Console.WriteLine("FooInterLock");
        var fooInterlock = new FooInterLock();
        Parallel.Invoke(
            () => { fooInterlock.First(() => Console.WriteLine("First")); },
            () => { fooInterlock.Third(() => Console.WriteLine("Third")); },
            () => { fooInterlock.Second(() => Console.WriteLine("Second")); }
        );
    }
}