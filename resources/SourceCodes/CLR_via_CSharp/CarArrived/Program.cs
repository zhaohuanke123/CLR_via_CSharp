using System.Threading;

public class TrafficLight
{
    private readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
    private volatile int currentRoad = 1;

    public void CarArrived(
        int carId, int roadId, int direction,
        Action turnGreen, Action crossCar)
    {
        // 第一阶段：尝试快速读
        rwLock.EnterReadLock();
        try
        {
            if (roadId == currentRoad)
            {
                crossCar();
                return;
            }
        }
        finally
        {
            rwLock.ExitReadLock();
        }

        // 第二阶段：写锁处理
        rwLock.EnterWriteLock();
        try
        {
            if (roadId != currentRoad)
            {
                currentRoad = roadId;
                turnGreen();
            }

            // 提前进入读锁（锁降级）
            rwLock.EnterReadLock();
        }
        finally
        {
            // 释放写锁但保持读锁
            rwLock.ExitWriteLock(); 
        }

        // 第三阶段：在降级的读锁下执行
        try
        {
            crossCar();
        }
        finally
        {
            rwLock.ExitReadLock();
        }
    }
}