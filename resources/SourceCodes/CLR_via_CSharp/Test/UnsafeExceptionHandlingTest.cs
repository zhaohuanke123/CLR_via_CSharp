using System;

namespace Test
{
    public class UnsafeExceptionHandlingTest
    {
        public static void Go()
        {
            try
            {
                Console.WriteLine("Starting unsafe code and exception handling test...");
                TestUnsafeCodeWithException();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in Main: " + ex.Message);
                Console.WriteLine("StackTrace:\n" + ex.StackTrace);
            }
        }

        static unsafe void TestUnsafeCodeWithException()
        {
            int* ptr = null;

            try
            {
                // Allocating an integer array in unmanaged memory
                ptr = (int*)System.Runtime.InteropServices.Marshal.AllocHGlobal(5 * sizeof(int));
                Console.WriteLine("Memory allocated.");

                // Initializing the array
                for (int i = 0; i < 100; i++)
                {
                    ptr[i] = i * 10;
                    Console.WriteLine($"ptr[{i}] = {ptr[i]}");
                }

                // Trigger an exception after modifying the array
                // throw new InvalidOperationException("Simulated exception in unsafe code.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Caught an exception in unsafe code: " + ex.Message);
                throw; // Re-throwing the exception to test higher-level handling
            }
            finally
            {
                // Freeing allocated memory
                if (ptr != null)
                {
                    System.Runtime.InteropServices.Marshal.FreeHGlobal((IntPtr)ptr);
                    Console.WriteLine("Memory freed in finally block.");
                }
            }
        }
    }
}