using System;
using System.Buffers;
using System.Threading;
using NUnit.Framework;

[TestFixture]
public class ArrayPoolTests
{
    private ArrayPool<int> _arrayPool;

    [SetUp]
    public void SetUp()
    {
        // 获取共享的 ArrayPool 实例
        _arrayPool = ArrayPool<int>.Shared;
    }

    [Test]
    public void Rent_ShouldReturnValidArray()
    {
        // 请求一个长度为 100 的数组
        int[] array = _arrayPool.Rent(100);

        // 验证返回的数组不为 null，并且长度为请求的大小
        Assert.That(() => array, Is.Not.Null);
        Assert.That(() => array.Length, Is.GreaterThan(100));

        // 归还数组
        _arrayPool.Return(array);

        // 验证数组可以重新租借
        int[] newArray = _arrayPool.Rent(100);
        Assert.That(() => newArray, Is.Not.Null);
    }

    [Test]
    public void Rent_ShouldThrowWhenRequestedSizeExceedsMaximum()
    {
        // 请求一个大于最大值的数组
        Assert.Throws<ArgumentOutOfRangeException>(() => _arrayPool.Rent(1024 * 1024 * 1025));
    }

    [Test]
    public void Return_ShouldCorrectlyReturnArrayToPool()
    {
        int[] array = _arrayPool.Rent(100);

        // 模拟修改数组内容
        array[0] = 42;

        // 归还数组，默认不清空数组
        _arrayPool.Return(array);

        // 重新租借该数组并验证内容未被清空
        int[] newArray = _arrayPool.Rent(100);
        Assert.That(() => newArray[0], Is.EqualTo(42));
    }

    [Test]
    public void Return_ShouldClearArrayWhenRequested()
    {
        int[] array = _arrayPool.Rent(100);

        // 模拟修改数组内容
        array[0] = 42;

        // 归还数组并清空内容
        _arrayPool.Return(array, clearArray: true);

        // 重新租借该数组并验证内容已被清空
        int[] newArray = _arrayPool.Rent(100);
        for (int i = 0; i < newArray.Length; i++)
        {
            int j = i;
            Assert.That(() => newArray[j], Is.EqualTo(0));
        }
    }

    [Test]
    public void Rent_ShouldHandleMultipleThreads()
    {
        const int numThreads = 10;
        const int arraySize = 100;
        var threads = new Thread[numThreads];

        // 使用多个线程并行测试租借和归还操作
        for (int i = 0; i < numThreads; i++)
        {
            threads[i] = new Thread(() =>
            {
                // 每个线程都租借并归还数组
                int[] array = _arrayPool.Rent(arraySize);
                Assert.That(() => array.Length, Is.EqualTo(arraySize));
                _arrayPool.Return(array);
            });
        }

        // 启动所有线程
        foreach (var thread in threads)
        {
            thread.Start();
        }

        // 等待所有线程执行完毕
        foreach (var thread in threads)
        {
            thread.Join();
        }
    }

    [Test]
    public void Rent_ShouldHandleZeroLengthArray()
    {
        // 请求一个零长度数组
        int[] array = _arrayPool.Rent(0);

        // 验证返回的数组不为 null，且长度为 0
        // Assert.NotNull(array);
        // Assert.AreEqual(0, array.Length);
        Assert.That(() => array, Is.Not.Null);
        Assert.That(() => array.Length, Is.EqualTo(0));

        // 归还零长度数组
        _arrayPool.Return(array);
    }

    [Test]
    public void Return_ShouldThrowArgumentExceptionWhenArrayDoesNotMatchPool()
    {
        // 请求一个大小为 100 的数组
        int[] array = _arrayPool.Rent(100);

        // 修改数组大小，使其与池中不匹配
        int[] invalidArray = new int[200];

        // 验证归还时会抛出 ArgumentException
        Assert.Throws<ArgumentException>(() => _arrayPool.Return(invalidArray));
    }

    [Test]
    public void Return_ShouldHandleLargeArraySizes()
    {
        // 请求一个最大值大小的数组
        int[] array = _arrayPool.Rent(1024 * 1024 * 1024);

        // 验证大数组分配成功
        // Assert.NotNull(array);
        // Assert.AreEqual(1024 * 1024 * 1024, array.Length);
        Assert.That(() => array, Is.Not.Null);
        Assert.That(() => array.Length, Is.EqualTo(1024 * 1024 * 1024));

        // 归还大数组
        _arrayPool.Return(array);
    }
}