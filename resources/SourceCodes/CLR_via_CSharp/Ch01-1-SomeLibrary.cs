#if !DEBUG
#pragma warning disable 3002, 3005
#endif
using System;

// Tell compiler to check for CLS compliance
[assembly: CLSCompliant(true)]

namespace SomeLibrary
{
    // Warnings appear because the class is public
    public sealed class SomeLibraryType
    {
        // Warning: Return type of 'SomeLibrary.SomeLibraryType.Abc()' 
        // is not CLS-compliant
        public UInt32 Abc()
        {
            Test test = new Test();
            var t = test.AProperty;
            return 0;
        }

        // Warning: Identifier 'SomeLibrary.SomeLibraryType.abc()' 
        // differing only in case is not CLS-compliant
        public void abc()
        {
        }

        // No warning: Method is private
        private UInt32 ABC()
        {
            return 0;
        }
    }

    internal partial class Test
    {
        // 终结器 析构函数
        ~Test()
        {
        }

        // 操作符重载
        public static Boolean operator ==(Test t1, Test t2)
        {
            return true;
        }

        public static Boolean operator !=(Test t1, Test t2)
        {
            return false;
        }

        // 操作符重载
        public static Test operator +(Test t1, Test t2)
        {
            return null;
        }

        // 属性
        public String AProperty
        {
            get { return null; }
            set { }
        }

        // 索引器
        public String this[Int32 x]
        {
            get { return null; }
            set { }
        }

        // 事件
        event EventHandler AnEvent;
    }
}