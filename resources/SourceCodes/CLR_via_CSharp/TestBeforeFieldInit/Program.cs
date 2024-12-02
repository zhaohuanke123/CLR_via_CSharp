﻿using System;
using System.Diagnostics;

namespace TestBeforeFieldInit
{
    class TestCtor
    {
        partial interface IF
        {
        }

        partial interface IF
        {
        }

        public TestCtor()
        {
            // Console.WriteLine($"TestCtor {name}");
            // 获取当前调用堆栈的信息
            var stackTrace = new StackTrace();
            var mb = stackTrace.GetFrame(1).GetMethod();
            var type = mb.DeclaringType;
            var method = mb.Name;
            var typeName = type.Name;

            Console.WriteLine("当前类: " + typeName + " " + method);
        }
    }

    internal class BeforeInit
    {
        public static TestCtor testCtor = new TestCtor();
    }

    internal class Class1
    {
        public static TestCtor testCtor = new TestCtor();
    }

    internal class NotBefore
    {
        public static TestCtor testCtor;

        static NotBefore()
        {
            testCtor = new TestCtor();
        }
    }

    internal partial class Program
    {
        public static void Main(string[] args)
        {
            _ = NotBefore.testCtor;
            Console.WriteLine("------------");
            _ = BeforeInit.testCtor;
            Console.WriteLine("------------");
            _ = Class1.testCtor;
            Console.WriteLine("------------");
        }
    }
}