namespace P5
{
    public class TestDymaic
    {
       public static void Run ()
       {
           dynamic x = 10;
           dynamic y = 5;
           dynamic z = x + y;
           System.Console.WriteLine(z);
       }
    }
}