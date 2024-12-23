using Microsoft.Extensions.ObjectPool;

ObjectPool<object> pool = new DefaultObjectPool<object>(new DefaultPooledObjectPolicy<object>());

for (int i = 0; i < 100; i++)
{
    var o = pool.Get();
    Console.WriteLine(o);
    Console.WriteLine(GC.GetTotalMemory(false));

    pool.Return(o);
}