using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TestAttrbuites
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal sealed class OSVERSIONINFO
    {
        public OSVERSIONINFO()
        {
        }

        public UInt32 OSVersionInfoSize = 0;
        public UInt32 MajorVersion = 0;
        public UInt32 MinorVersion = 0;
        public UInt32 BuildNumber = 0;
        public UInt32 PlatformId = 0;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string CSDVersion = null;
    }

    internal sealed class MyClass
    {
        [DllImport("Kernal32", CharSet = CharSet.Auto, SetLastError = new bool())]
        public static extern bool GetVersionEx([In, Out] OSVERSIONINFO osVersionInfo);
    }

    [AttributeUsage(AttributeTargets.All)]
    internal class MyAttribute : Attribute
    {
        object[] a;

        public MyAttribute()
        {
        }

        public MyAttribute(object[] a)
        {
            this.a = a;
        }
    }

    [My(new object[] { "123", 2, 3, 4, typeof(Base) })]
    class Base
    {
    }

    class Derive : Base
    {
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var isDefined = typeof(Derive).IsDefined(typeof(MyAttribute), true);
            Console.WriteLine(isDefined);

            Console.WriteLine("================");

            var customAttribute = typeof(Derive).GetCustomAttribute(typeof(MyAttribute), true);
            Console.WriteLine(customAttribute);

            Console.WriteLine("================");

            var customAttributes = typeof(Derive).GetCustomAttributes();
            foreach (var attribute in customAttributes)
            {
                Console.WriteLine(attribute);
            }

            Console.WriteLine("================");

            var customAttributeDatas = CustomAttributeData.GetCustomAttributes(typeof(Base));
            foreach (var customAttributeData in customAttributeDatas)
            {
                Console.WriteLine(customAttributeData);
            }

            Console.WriteLine("================");
        }
    }
}