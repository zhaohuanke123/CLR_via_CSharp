using System;
using System.Runtime.ConstrainedExecution;

namespace Test
{
    public class TestFailFast
    {
        public static void Run()
        {
            try
            {
                Console.WriteLine("TestFailFast");
                Environment.FailFast("TestFailFast");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("Finally");
            }
        }
    }

    class CFO : CriticalFinalizerObject
    {
        ~CFO()
        {
            Console.WriteLine("CFO.Finalize");
        }        
    }
}