using System;
using System.Threading;

namespace C21APPDomain
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var app1 = AppDomain.CreateDomain("NewDomain");
            try
            {
                app1.DoCallBack(() =>
                {
                    Thread.Sleep(1000);
                    try
                    {
                        var appDomain = Thread.GetDomain();
                        AppDomain.Unload(appDomain);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }
    }
}