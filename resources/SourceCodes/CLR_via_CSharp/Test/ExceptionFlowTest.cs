using System;

namespace Ch20_1_ExceptionHandling
{
    public class ExceptionFlowTest
    {
       public static void Goc()
        {
            try
            {
                Console.WriteLine("Outer try block");

                try
                {
                    Console.WriteLine("Inner try block - Start");
                    throw new InvalidOperationException("Test Exception");
                }
                finally
                {
                    Console.WriteLine("Inner finally block");
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Caught exception: " + ex.Message);

                // Option 1: Rethrow the same exception
                Console.WriteLine("Re-throwing exception...");
                throw;

                // Option 2: Uncomment to throw a new exception
                //Console.WriteLine("Throwing new exception...");
                //throw new ApplicationException("New exception from catch block");

                // Option 3: Uncomment to exit catch block normally
                //Console.WriteLine("Exiting catch block normally.");
            }
            finally
            {
                Console.WriteLine("Outer finally block");
            }
        } 
    }
}