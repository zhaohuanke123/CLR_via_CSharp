using System;
// using System.Windows.Forms;
using System.IO;
using p17e;

// 声明一个委托类型，它的实例引用一个方法，
// 该方法获取一个 Int32 参数，返回 void
internal delegate void Feedback(Int32 value);

public sealed class Program
{
    delegate void Bar(out Int32 z);

    public static void Main()
    {
        DelegateReflection.Run(new[] { "DelegateReflection", "Add", "Subtract", "NumChars", "Reverse" });
        // StaticDelegateDemo();
        // InstanceDelegateDemo();
        // ChainDelegateDemo1(new Program());
        // ChainDelegateDemo2(new Program());
    }

    private static void StaticDelegateDemo()
    {
        Console.WriteLine("----- Static Delegate Demo -----");
        Counter(1, 3, null);
        Counter(1, 3, new Feedback(Program.FeedbackToConsole));
        Counter(1, 3, new Feedback(FeedbackToMsgBox)); // 前缀 "Program." 可选
        Console.WriteLine();
    }

    private static void InstanceDelegateDemo()
    {
        Console.WriteLine("----- Instance Delegate Demo -----");
        Program p = new Program();
        Counter(1, 3, new Feedback(p.FeedbackToFile));
        Console.WriteLine();
    }

    private static void ChainDelegateDemo1(Program p)
    {
        Console.WriteLine("----- Chain Delegate Demo 1 -----");
        Feedback fb1 = new Feedback(FeedbackToConsole);
        Feedback fb2 = new Feedback(FeedbackToMsgBox);
        Feedback fb3 = new Feedback(p.FeedbackToFile);

        Feedback fbChain = null;
        fbChain = (Feedback)Delegate.Combine(fbChain, fb1);
        fbChain = (Feedback)Delegate.Combine(fbChain, fb2);
        fbChain = (Feedback)Delegate.Combine(fbChain, fb3);
        Counter(1, 2, fbChain);

        Console.WriteLine();
        fbChain = (Feedback)
            Delegate.Remove(fbChain, new Feedback(FeedbackToMsgBox));
        Counter(1, 2, fbChain);
    }

    private static void ChainDelegateDemo2(Program p)
    {
        Console.WriteLine("----- Chain Delegate Demo 2 -----");
        Feedback fb1 = new Feedback(FeedbackToConsole);
        Feedback fb2 = new Feedback(FeedbackToMsgBox);
        Feedback fb3 = new Feedback(p.FeedbackToFile);

        Feedback fbChain = null;
        fbChain += fb1;
        fbChain += fb2;
        fbChain += fb3;
        Counter(1, 2, fbChain);

        Console.WriteLine();
        fbChain -= new Feedback(FeedbackToMsgBox);
        Counter(1, 2, fbChain);
    }

    private static void Counter(Int32 from, Int32 to, Feedback fb)
    {
        for (Int32 val = from; val <= to; val++)
        {
            // 如果指定了任何回调，就调用它们
            if (fb != null)
            {
                try
                {
                    fb(val);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }

    private static void FeedbackToConsole(Int32 value)
    {
        Console.WriteLine("Item=" + value);
        throw new Exception();
    }

    private static void FeedbackToMsgBox(Int32 value)
    {
        // MessageBox.Show("Item=" + value);
        Console.WriteLine("Box Item=" + value);
    }

    private void FeedbackToFile(Int32 value)
    {
        using (StreamWriter sw = new StreamWriter("Status", true))
        {
            sw.WriteLine("Item=" + value);
        }
    }
}