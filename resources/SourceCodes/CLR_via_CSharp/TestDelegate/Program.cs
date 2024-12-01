using System;

internal sealed class AClass
{
    public delegate void MyDelegate(int i);

    public static string str = "Hello World";

    public static void Method(MyDelegate myDelegate, int i)
    {
        myDelegate(i);
    }

    //public static void Callback(int i)
    //{
    //    Console.WriteLine(i);
    //}

    public void CallbackWithoutNewingADelegateObject()
    {
        Program p = new Program();
        int arg = 10;
        Method( // This is the line that is causing the error
            delegate(int i) { Console.WriteLine(i); },
            arg);
    }
}

public sealed class Program
{
    public static void Main(string[] args)
    {
        AClass aClass = new AClass();
        aClass.CallbackWithoutNewingADelegateObject();
    }
}