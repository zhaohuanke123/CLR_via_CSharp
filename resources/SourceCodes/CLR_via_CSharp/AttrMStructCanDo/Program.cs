using System;

namespace AttrMStructCanDo
{
    [Serializable]
    struct MyStruct
    {
    }

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    sealed class MyAttribute : Attribute
    {
        private MyStruct ms;

        public MyAttribute(MyStruct ms = default)
        {
            this.ms = ms;
        }
    }

    [My]
    internal class Program
    {
        public static void Main(string[] args)
        {
        }
    }
}