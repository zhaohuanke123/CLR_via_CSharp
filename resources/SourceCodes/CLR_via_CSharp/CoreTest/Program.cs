using System.Reflection;
using System.Security.Cryptography;
using MToolKit;

namespace CoreTest;

class Program
{
    struct MyStruct
    {
        public int a;
        public int b;

        public MyStruct(int a)
        {
            this.a = a;
            this.b = a;
            //this.b = a;
            //this.c = a;
            //this.d = a;
            //this.e = a;
            //this.f = a;
            //this.g = a;
            //this.h = a;
            //this.i = a;
        }

        public static MyStruct operator ++(MyStruct self)
        {
            self.a++;
            return self;
        }
    }

    static void Main(string[] args)
    {
        MultiDimArrayPerformance.Go();
    }
}

internal static class MultiDimArrayPerformance
{
    private const Int32 CNumElements = 20000;

    public static void Go()
    {
        // Declare a 2-dimensional array
        Int32[,] a2Dim = new Int32[CNumElements, CNumElements];

        // Declare a 2-dimensional array as a jagged array (a vector of vectors)
        Int32[][] aJagged = new Int32[CNumElements][];
        for (Int32 x = 0; x < CNumElements; x++)
            aJagged[x] = new Int32[CNumElements];


        using (new PerformanceTester("DimArr"))
        {
            // 1: Access all elements of the array using the usual, safe technique
            Safe2DimArrayAccess(a2Dim);
        }

        using (new PerformanceTester("JaggedArr"))
        {
            // 2: Access all elements of the array using the jagged array technique
            SafeJaggedArrayAccess(aJagged);
        }

        using (new PerformanceTester("UnsafeArr"))
        {
            // 3: Access all elements of the array using the unsafe technique
            Unsafe2DimArrayAccess(a2Dim);
        }
    }

    private static Int32 Safe2DimArrayAccess(Int32[,] a)
    {
        Int32 sum = 0;
        for (Int32 x = 0; x < CNumElements; x++)
        {
            for (Int32 y = 0; y < CNumElements; y++)
            {
                sum += a[x, y];
            }
        }
        return sum;
    }

    private static Int32 SafeJaggedArrayAccess(Int32[][] a)
    {
        Int32 sum = 0;
        for (Int32 x = 0; x < CNumElements; x++)
        {
            for (Int32 y = 0; y < CNumElements; y++)
            {
                sum += a[x][y];
            }
        }
        return sum;
    }

    private static unsafe Int32 Unsafe2DimArrayAccess(Int32[,] a)
    {
        Int32 sum = 0, numElements = CNumElements * CNumElements;
        fixed (Int32* pi = a)
        {
            for (Int32 x = 0; x < numElements; x++)
            {
                sum += pi[x];
            }
        }
        return sum;
    }
}
