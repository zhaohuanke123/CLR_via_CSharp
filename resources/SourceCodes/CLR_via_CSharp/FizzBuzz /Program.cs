public class FizzBuzz
{
    private readonly int m_n;
    private int m_current = 1;
    private readonly object m_lockObj = new object(); 

    public FizzBuzz(int n)
    {
        this.m_n = n;
    }

    // printFizz() outputs "fizz".
    public void Fizz(Action printFizz)
    {
        while (true)
        {
            lock (m_lockObj)
            {
                while (m_current <= m_n && (m_current % 3 != 0 || m_current % 5 == 0))
                {
                    Monitor.Wait(m_lockObj);
                }

                if (m_current > m_n) break;

                printFizz();
                m_current++;
                Monitor.PulseAll(m_lockObj);
            }
        }
    }

    // printBuzz() outputs "buzz".
    public void Buzz(Action printBuzz)
    {
        while (true)
        {
            lock (m_lockObj)
            {
                while (m_current <= m_n && (m_current % 5 != 0 || m_current % 3 == 0))
                {
                    Monitor.Wait(m_lockObj);
                }

                if (m_current > m_n) break;

                printBuzz();
                m_current++;
                Monitor.PulseAll(m_lockObj);
            }
        }
    }

    // printFizzBuzz() outputs "fizzbuzz".
    public void Fizzbuzz(Action printFizzBuzz)
    {
        while (true)
        {
            lock (m_lockObj)
            {
                while (m_current <= m_n && (m_current % 3 != 0 || m_current % 5 != 0))
                {
                    Monitor.Wait(m_lockObj);
                }

                if (m_current > m_n) break;

                printFizzBuzz();
                m_current++;
                Monitor.PulseAll(m_lockObj);
            }
        }
    }

    // printNumber(x) outputs "x", where x is an integer.
    public void Number(Action<int> printNumber)
    {
        while (true)
        {
            lock (m_lockObj)
            {
                while (m_current <= m_n && (m_current % 3 == 0 || m_current % 5 == 0))
                {
                    Monitor.Wait(m_lockObj);
                }

                if (m_current > m_n) break;

                printNumber(m_current);
                m_current++;
                Monitor.PulseAll(m_lockObj);
            }
        }
    }
}

public class Program
{
    public static void Main()
    {
        FizzBuzz fizzBuzz = new FizzBuzz(15);
        Parallel.Invoke(
            () => { fizzBuzz.Buzz(() => Console.Write("buzz,")); },
            () => { fizzBuzz.Fizz(() => Console.Write("fizz,")); },
            () => { fizzBuzz.Fizzbuzz(() => Console.Write("fizzbuzz,")); },
            () => { fizzBuzz.Number((number) => Console.Write(number + ",")); }
        );
    }
}