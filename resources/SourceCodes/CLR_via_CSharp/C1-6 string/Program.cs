using System.Runtime.CompilerServices;
using System.Text;

namespace C16string
{
    class Program
    {
        public static void Main()
        {
            Test0();
            Test1();
            Test2();
        }

        private static void Test0()
        {
            var str1 = "hello world"u8;

            ReadOnlySpan<byte> sp1 = str1.ToArray(); // byte[]
            // Console.WriteLine(str1.ToString());
        }

        private static void Test1()
        {
            string singleLine = """Friends say "hello" as they pass by.""";
            string multiLine = """
                               "Hello World!" is typically the first program someone writes.
                               """;
            string embeddedXML = """
                                 <element attr = "content">
                                     <body style="normal">
                                         Here is the main text
                                     </body>
                                     <footer>
                                         Excerpts from "An amazing story"
                                     </footer>
                                 </element >
                                 """;
            // The line "<element attr = "content">" starts in the first column.
            // All whitespace left of that column is removed from the string.

            string rawStringLiteralDelimiter = """"
                                               Raw string literals are delimited
                                               by a string of at least three double quotes,
                                               like this: """
                                               """";
        }

        private static void Test2()
        {
            var json = $$"""
                         {
                             "summary": "text",
                             "length": {{5.ToString()}}
                         };
                         """;
            Console.WriteLine(json);
        }

        private static void TestInterpolate()
        {
            double a = 3;
            double b = 4;
            // + +=
            var str1 = "Area of the right triangle with legs of " + a + " and " + b + " is " + 0.5 * a * b;
            // Interpolate 内插
            var str2 = $"Area of the right triangle with legs of {a} and {b} is {0.5 * a * b}";
            // string.Format
            var str3 = string.Format("Area of the right triangle with legs of {0} and {1} is {2}", a, b, 0.5 * a * b);
            // string.Concat
            var str4 = string.Concat("Area of the right triangle with legs of ", a, " and ", b, " is ", 0.5 * a * b);
            // string.Join
            var str5 = string.Join("", "Area of the right triangle with legs of ", a, " and ", b, " is ", 0.5 * a * b);
            // stringBuilder
            var sb = new StringBuilder();
            sb.Append("Area of the right triangle with legs of ").Append(a).Append(" and ").Append(b).Append(" is ")
                .Append(0.5 * a * b);
            var str6 = sb.ToString();
        }

        private void TestLiteralStringInterpolate()
        {
            // 现在可以内插常量字符串
            const string S1 = $"Hello world";
            const string S2 = $"Hello{" "}World";
            const string S3 = $"{S1} Kevin, welcome to the team!";
        }
    }
}