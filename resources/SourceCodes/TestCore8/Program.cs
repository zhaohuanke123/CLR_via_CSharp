using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using MToolKitCore;

namespace TestCore8
{

    public class SealedBenchmark
    {
        readonly NonSealedType nonSealedType = new NonSealedType();
        readonly SealedType sealedType = new SealedType();

        static void Main()
        {
            _ = BenchmarkRunner.Run<SealedBenchmark>();
            //_ = BenchmarkRunner.Run<BenchToString>();
            //SealedBenchmark s = new SealedBenchmark();
            //s.NonSealed();
            //s.Sealed();
        }

        [Benchmark(Baseline = true)]
        public void NonSealed()
        {
            nonSealedType.Method();
        }

        [Benchmark]
        public void Sealed()
        {
            sealedType.Method();
        }
    }

    internal class BaseType
    {
        public virtual void Method()
        {
        }
    }

    internal class NonSealedType : BaseType
    {
        public override void Method()
        {
        }
    }

    internal sealed class SealedType : BaseType
    {
        public override void Method()
        {
        }
    }
}

public class BenchToString
{
    private readonly Point point = new();
    private readonly PointSealed pointSealed = new();

    [Benchmark(Baseline = true)]
    public void NonSealed()
    {
        for (int i = 0; i < 10; i++)
            _ = point.ToString();
    }

    [Benchmark]
    public void Sealed()
    {
        for (int i = 0; i < 10; i++)
            _ = pointSealed.ToString();
    }

    internal class Point
    {
        public override string ToString()
        {
            return "";
        }
    }

    internal sealed class PointSealed
    {
        public override string ToString()
        {
            return "";
        }
    }
}