using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Test
{
    [StructLayout(LayoutKind.Auto)]
    struct St
    {
        public char a;
        public byte c;
    }
    internal class Program
    {

        static void Main(string[] args)
        {
            St st = new St();
            st.a = char.MaxValue;
            st.c = Byte.MaxValue;
            St[] program = new St[10];
            program[0] = new St();
            program[1] = new St();
            program[0].a = char.MaxValue;
            program[0].c = byte.MaxValue;
            program[1].a = char.MaxValue;
            program[1].c = byte.MaxValue;
        }
    }
}