using System.Reflection.Metadata;

public class ZeroEvenOdd
{
    private int n;
    private int m_currentNumber;
    private readonly int m_oddCount;
    private readonly int m_evenCount;

    private readonly SemaphoreSlim m_zeroSem = new SemaphoreSlim(1, 1);
    private readonly SemaphoreSlim m_oddSem = new SemaphoreSlim(0, 1);
    private readonly SemaphoreSlim m_evenSem = new SemaphoreSlim(0, 1);

    public ZeroEvenOdd(int n)
    {
        this.n = n;
        this.m_oddCount = (n + 1) / 2;
        this.m_evenCount = n / 2;
    }

    public void Zero(Action<int> printNumber)
    {
        for (int i = 1; i <= n; i++)
        {
            m_zeroSem.Wait();
            printNumber(0);
            m_currentNumber = i;
            if (i % 2 == 1)
            {
                m_oddSem.Release();
            }
            else
            {
                m_evenSem.Release();
            }
        }
    }

    public void Odd(Action<int> printNumber)
    {
        for (int i = 0; i < m_oddCount; i++)
        {
            m_oddSem.Wait();
            printNumber(m_currentNumber);
            m_zeroSem.Release();
        }
    }

    public void Even(Action<int> printNumber)
    {
        for (int i = 0; i < m_evenCount; i++)
        {
            m_evenSem.Wait();
            printNumber(m_currentNumber);
            m_zeroSem.Release();
        }
    }
}

public class ZeroEvenOddInterLock
{
    private int n;
    private int m_current = 1;
    private int m_flag = 0; 

    public ZeroEvenOddInterLock(int n)
    {
        this.n = n;
    }

    public void Zero(Action<int> printNumber)
    {
        for (int i = 0; i < n; i++)
        {
            var spin = new SpinWait();
            while (Interlocked.CompareExchange(ref m_flag, 0, 0) != 0)
            {
                spin.SpinOnce();
            }

            printNumber(0);

            if (m_current % 2 == 1)
            {
                Interlocked.Exchange(ref m_flag, 1);
            }
            else
            {
                Interlocked.Exchange(ref m_flag, 2); 
            }
        }
    }

    public void Odd(Action<int> printNumber)
    {
        var spin = new SpinWait();
        while (true)
        {
            int currentLocal = Interlocked.CompareExchange(ref m_current, 0, 0);
            if (currentLocal > n) break;

            if (Interlocked.CompareExchange(ref m_flag, 1, 1) == 1 &&
                currentLocal % 2 == 1)
            {
                printNumber(currentLocal);
                Interlocked.Increment(ref m_current);
                Interlocked.Exchange(ref m_flag, 0); 
            }

            spin.SpinOnce();
        }
    }

    public void Even(Action<int> printNumber)
    {
        var spin = new SpinWait();
        while (true)
        {
            int currentLocal = Interlocked.CompareExchange(ref m_current, 0, 0);
            if (currentLocal > n) break;

            if (Interlocked.CompareExchange(ref m_flag, 2, 2) == 2 &&
                currentLocal % 2 == 0)
            {
                printNumber(currentLocal);
                Interlocked.Increment(ref m_current); // 原子递增数字
                Interlocked.Exchange(ref m_flag, 0); // 允许打印0
            }

            spin.SpinOnce();
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("zeroEvenOdd");
        var zeroEvenOdd = new ZeroEvenOdd(5);
        Parallel.Invoke(
            () => zeroEvenOdd.Zero(Console.Write),
            () => zeroEvenOdd.Odd(Console.Write),
            () => zeroEvenOdd.Even(Console.Write)
        );
        
        Console.WriteLine("\nzeroEvenOddInterLock");
        var zeroEvenOddInterLock = new ZeroEvenOddInterLock(5);
        Parallel.Invoke(
            () => zeroEvenOddInterLock.Zero(Console.Write),
            () => zeroEvenOddInterLock.Odd(Console.Write),
            () => zeroEvenOddInterLock.Even(Console.Write)
            );
    }
}