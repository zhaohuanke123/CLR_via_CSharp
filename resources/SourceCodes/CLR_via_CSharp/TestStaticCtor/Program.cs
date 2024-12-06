using System;

namespace TestStaticCtor
{
    internal class MClass
    {
        private static int i = 1;

        static MClass()
        {
            string s = i.ToString();
            i++;
            throw new Exception(s);
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                MClass ms = new MClass();
                Console.WriteLine(ms);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            MClass ms1 = new MClass();
            Console.WriteLine(ms1);
        }
    }
}