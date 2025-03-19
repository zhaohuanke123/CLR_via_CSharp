internal class DiningPhilosophers
{
    private Semaphore m_leftForkSemaphore;
    private Semaphore m_rightForkSemaphore;
    private WaitHandle m_waitHandle;
    private int m_eatCount;


    public DiningPhilosophers(Semaphore leftForkSemaphore, Semaphore rightForkSemaphore, int eatCount)
    {
        m_leftForkSemaphore = leftForkSemaphore;
        m_rightForkSemaphore = rightForkSemaphore;
        m_eatCount = eatCount;
    }

    // call the run() method of any Action to execute its code
    public void WantsToEat(int philosopher, Action pickLeftFork, Action pickRightFork, Action eat, Action putLeftFork,
        Action putRightFork)
    {
        for (int i = 0; i < m_eatCount; i++)
        {
            WaitHandle.WaitAll(new WaitHandle[] { m_leftForkSemaphore, m_rightForkSemaphore });
            pickLeftFork();
            pickRightFork();
            Thread.Sleep(100);

            eat();
            Thread.Sleep(100);

            putLeftFork();
            Thread.Sleep(100);
            m_leftForkSemaphore.Release();
            putRightFork();
            Thread.Sleep(100);
            m_rightForkSemaphore.Release();
        }
    }

    // call the run() method of any Action to execute its code
    public void WantsToEat2(int philosopher, Action pickLeftFork, Action pickRightFork, Action eat, Action putLeftFork,
        Action putRightFork)
    {
        for (int i = 0; i < m_eatCount; i++)
        {
            if (philosopher % 2 == 0)
            {
                m_rightForkSemaphore.WaitOne();
                pickRightFork();
                Thread.Sleep(100);
                m_leftForkSemaphore.WaitOne();
                pickLeftFork();
                Thread.Sleep(100);
            }
            else
            {
                m_leftForkSemaphore.WaitOne();
                pickLeftFork();
                Thread.Sleep(100);
                m_rightForkSemaphore.WaitOne();
                pickRightFork();
                Thread.Sleep(100);
            }

            eat();
            Thread.Sleep(100);

            putLeftFork();
            Thread.Sleep(100);
            m_leftForkSemaphore.Release();
            putRightFork();
            Thread.Sleep(100);
            m_rightForkSemaphore.Release();
        }
    }
}

internal static class Program
{
    const int n = 5;

    public static void Main(string[] args)
    {
        Semaphore[] semaphores = new Semaphore[5];
        for (int i = 0; i < semaphores.Length; i++)
        {
            semaphores[i] = new Semaphore(1, 1);
        }

        DiningPhilosophers[] philosophers = new DiningPhilosophers[5];
        for (int i = 0; i < philosophers.Length; i++)
        {
            philosophers[i] = new DiningPhilosophers(semaphores[i], semaphores[(i + 1) % semaphores.Length], n);
        }


        // 只能同时拿起Fork
        // Parallel.For(0, philosophers.Length, i =>
        // {
        //     philosophers[i].WantsToEat(i,
        //         () => Console.WriteLine($"[{i} 1 {i}]"),
        //         () => Console.WriteLine($"[{i} 1 {(i + 1) % semaphores.Length}]"),
        //         () => Console.WriteLine($"[{i} 0 2]"),
        //         () => Console.WriteLine($"[{i} 3 {i}]"),
        //         () => Console.WriteLine($"[{i} 3 {(i + 1) % semaphores.Length}]")
        //     );
        // });

        // 偶数哲学家先拿右Fork再拿左Fok，奇数相反。
        Parallel.For(0, philosophers.Length, i =>
        {
            philosophers[i].WantsToEat2(i,
                () => Console.WriteLine($"[{i} 1 {i}]"),
                () => Console.WriteLine($"[{i} 1 {(i + 1) % semaphores.Length}]"),
                () => Console.WriteLine($"[{i} 0 2]"),
                () => Console.WriteLine($"[{i} 3 {i}]"),
                () => Console.WriteLine($"[{i} 3 {(i + 1) % semaphores.Length}]")
            );
        });
    }
}