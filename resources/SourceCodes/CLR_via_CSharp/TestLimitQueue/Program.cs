using System.Collections.Concurrent;

public class BoundedBlockingQueue
{
    private readonly Queue<int> m_queue;
    private readonly SemaphoreSlim m_emptySlots;
    private readonly SemaphoreSlim m_fullSlots;
    private readonly object m_lockObject;
    private int m_capacity;

    public BoundedBlockingQueue(int capacity)
    {
        this.m_capacity = capacity;
        m_queue = new Queue<int>();
        m_emptySlots = new SemaphoreSlim(capacity, capacity);
        m_fullSlots = new SemaphoreSlim(0, capacity);
        m_lockObject = new object();
        List<Task> tasks = new List<Task>();
        Task.WaitAll(tasks.ToArray());
    }

    public void Enqueue(int element)
    {
        m_emptySlots.Wait();
        lock (m_lockObject)
        {
            m_queue.Enqueue(element);
        }

        m_fullSlots.Release();
    }

    public int Dequeue()
    {
        m_fullSlots.Wait();
        int result;
        lock (m_lockObject)
        {
            result = m_queue.Dequeue();
        }

        m_emptySlots.Release();
        return result;
    }

    public int Size()
    {
        lock (m_lockObject)
        {
            return m_queue.Count;
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        BoundedBlockingQueue queue = new BoundedBlockingQueue(3);
        ConcurrentQueue<string> logQueue = new ConcurrentQueue<string>();
        Parallel.Invoke(
            () =>
            {
                for (int i = 0; i < 6; i++)
                {
                    queue.Enqueue(i);
                    logQueue.Enqueue($"En {i.ToString()}");
                }
            },
            () =>
            {
                for (int i = 0; i < 4; i++)
                {
                    int item = queue.Dequeue();
                    logQueue.Enqueue($"De {item.ToString()}");
                }
            }
        );

        foreach (string se in logQueue)
        {
            Console.WriteLine(se);
        }

        Console.WriteLine(queue.Size());
    }
}