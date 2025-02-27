using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Forms;

// System.Runtime.CompilerServices.CompilationRelaxationsAttribute
[assembly: CompilationRelaxations(CompilationRelaxations.NoStringInterning)]

public static class Program
{
    public static void Main(String[] args)
    {
        // GetNumericValue.Go();
        CharConvert.Go();
        // ComparingStringForEquality.Go();
        // ComparingStringsForSorting.Go();
        // Interning.Go();
        // UsingStringInfo.Go();
        // UsingStringBuilder.Go();
        // Formatting.Go();
        CustomFormatter.Go();
        // Encodings.Go();
        // EncodingProperties.Go();
        // CodePageConverter.Go(args);
        // Base64Encoding.Go();
        // UsingSecureString.Go();
    }
}

internal static class GetNumericValue
{
    public static void Go()
    {
        Double d;

        // ‘\u0033’ is the “digit 3”
        d = Char.GetNumericValue('\u0033'); // ‘3’ would work too
        Console.WriteLine(d.ToString()); // Displays "3"

        // ‘\u00bc’ is the “vulgar fraction one quarter ('¼')”
        d = Char.GetNumericValue('\u00bc');
        Console.WriteLine(d.ToString()); // Displays "0.25"

        // ‘A’ is the “Latin capital letter A”
        d = Char.GetNumericValue('A');
        Console.WriteLine(d.ToString()); // Displays "-1"
    }
}

internal static class CharConvert
{
    public static void Go()
    {
        char c;
        Int32 n;

        // Convert number <-> character using C# casting
        c = (char)65;
        Console.WriteLine(c); // Displays "A"

        n = (Int32)c;
        Console.WriteLine(n); // Displays "65"

        c = unchecked((Char)(65536 + 65));
        Console.WriteLine(c); // Displays "A"


        // Convert number <-> character using Convert
        c = Convert.ToChar(65);
        Console.WriteLine(c); // Displays "A"

        n = Convert.ToInt32(c);
        Console.WriteLine(n); // Displays "65" 


        // This demonstrates Convert's range checking
        try
        {
            c = Convert.ToChar(70000); // Too big for 16-bits
            Console.WriteLine(c); // Doesn't execute
        }
        catch (OverflowException)
        {
            Console.WriteLine("Can't convert 70000 to a Char.");
        }


        // Convert number <-> character using IConvertible
        c = ((IConvertible)65).ToChar(null);
        Console.WriteLine(c); // Displays "A"

        n = ((IConvertible)c).ToInt32(null);
        Console.WriteLine(n); // Displays "65"
    }
}

internal static class ComparingStringForEquality
{
    public static void Go()
    {
        String s1 = "Strasse";
        String s2 = "Straße";
        Boolean eq;

        // CompareOrdinal returns nonzero.
        eq = String.Compare(s1, s2, StringComparison.Ordinal) == 0;
        Console.WriteLine("Ordinal  comparison: '{0}' {2} '{1}'", s1, s2,
            eq ? "==" : "!=");

        // Compare Strings appropriately for people 
        // who speak German (de) in Germany (DE)
        CultureInfo ci = new CultureInfo("de-DE");

        // Compare returns zero.
        eq = String.Compare(s1, s2, true, ci) == 0;
        Console.WriteLine("Cultural comparison: '{0}' {2} '{1}'", s1, s2,
            eq ? "==" : "!=");
    }
}

internal static class ComparingStringsForSorting
{
    public static void Go()
    {
        String output = String.Empty;
        String[] symbol = new String[] { "<", "=", ">" };
        Int32 x;
        CultureInfo ci;

        // The code below demonstrates how strings compare 
        // differently for different cultures.
        String s1 = "coté";
        String s2 = "côte";

        // Sorting strings for French in France.
        ci = new CultureInfo("fr-FR");
        x = Math.Sign(ci.CompareInfo.Compare(s1, s2));
        output += String.Format("{0} Compare: {1} {3} {2}",
            ci.Name, s1, s2, symbol[x + 1]);
        output += Environment.NewLine;

        // Sorting strings for Japanese in Japan.
        ci = new CultureInfo("ja-JP");
        x = Math.Sign(ci.CompareInfo.Compare(s1, s2));
        output += String.Format("{0} Compare: {1} {3} {2}",
            ci.Name, s1, s2, symbol[x + 1]);
        output += Environment.NewLine;

        // Sorting strings for the thread's culture
        ci = Thread.CurrentThread.CurrentCulture;
        x = Math.Sign(ci.CompareInfo.Compare(s1, s2));
        output += String.Format("{0} Compare: {1} {3} {2}",
            ci.Name, s1, s2, symbol[x + 1]);
        output += Environment.NewLine + Environment.NewLine;

        // The code below demonstrates how to use CompareInfo.Compare's
        // advanced options with 2 Japanese strings. One string represents
        // the word "shinkansen" (the name for the Japanese high-speed 
        // train) in hiragana (one subtype of Japanese writing), and the 
        // other represents the same word in katakana (another subtype of 
        // Japanese writing).
        s1 = "しんかんせん"; // ("\u3057\u3093\u304B\u3093\u305b\u3093")
        s2 = "シンカンセン"; // ("\u30b7\u30f3\u30ab\u30f3\u30bb\u30f3")

        // Here is the result of a default comparison
        ci = new CultureInfo("ja-JP");
        x = Math.Sign(String.Compare(s1, s2, true, ci));
        output += String.Format("Simple {0} Compare: {1} {3} {2}",
            ci.Name, s1, s2, symbol[x + 1]);
        output += Environment.NewLine;

        // Here is the result of a comparison that ignores 
        // kana type (a type of Japanese writing)
        CompareInfo compareInfo = CompareInfo.GetCompareInfo("ja-JP");
        x = Math.Sign(compareInfo.Compare(s1, s2, CompareOptions.IgnoreKanaType));
        output += String.Format("Advanced {0} Compare: {1} {3} {2}",
            ci.Name, s1, s2, symbol[x + 1]);

        MessageBox.Show(output, "Comparing Strings For Sorting");
    }
}

internal static class Interning
{
    public static void Go()
    {
        String s1 = "Hello";
        String s2 = "Hello";
        Console.WriteLine(Object.ReferenceEquals(s1, s2)); // Should be 'False'

        s1 = String.Intern(s1);
        s2 = String.Intern(s2);
        Console.WriteLine(Object.ReferenceEquals(s1, s2)); // 'True'
    }

    private static Int32 NumTimesWordAppearsIntern(String word, String[] wordlist)
    {
        // This method assumes that all entries in wordList refer to interned strings.
        word = String.Intern(word);
        Int32 count = 0;
        for (Int32 wordnum = 0; wordnum < wordlist.Length; wordnum++)
        {
            if (Object.ReferenceEquals(word, wordlist[wordnum]))
                count++;
        }

        return count;
    }

    private static Int32 NumTimesWordAppearsEquals(String word, String[] wordlist)
    {
        Int32 count = 0;
        for (Int32 wordnum = 0; wordnum < wordlist.Length; wordnum++)
        {
            if (word.Equals(wordlist[wordnum], StringComparison.Ordinal))
                count++;
        }

        return count;
    }
}

internal static class UsingStringInfo
{
    public static void Go()
    {
        // The string below contains combining characters
        String s = "a\u0304\u0308bc\u0327";
        s = "你好世界";
        SubstringByTextElements(s);
        EnumTextElements(s);
        EnumTextElementIndexes(s);
    }

    private static void SubstringByTextElements(String s)
    {
        String output = String.Empty;

        StringInfo si = new StringInfo(s);
        for (Int32 element = 0; element < si.LengthInTextElements; element++)
        {
            output += String.Format(
                "Text element {0} is '{1}'{2}",
                element, si.SubstringByTextElements(element, 1),
                Environment.NewLine);
        }

        MessageBox.Show(output, "Result of SubstringByTextElements");
    }

    private static void EnumTextElements(String s)
    {
        String output = String.Empty;

        TextElementEnumerator charEnum =
            StringInfo.GetTextElementEnumerator(s);
        while (charEnum.MoveNext())
        {
            output += String.Format(
                "Text element at index {0} is '{1}'{2}",
                charEnum.ElementIndex, charEnum.GetTextElement(),
                Environment.NewLine);
        }

        MessageBox.Show(output, "Result of GetTextElementEnumerator");
    }

    private static void EnumTextElementIndexes(String s)
    {
        String output = String.Empty;

        Int32[] textElemIndex = StringInfo.ParseCombiningCharacters(s);
        for (Int32 i = 0; i < textElemIndex.Length; i++)
        {
            output += String.Format(
                "Text element {0} starts at index {1}{2}",
                i, textElemIndex[i], Environment.NewLine);
        }

        MessageBox.Show(output, "Result of ParseCombiningCharacters");
    }
}

internal static class UsingStringBuilder
{
    public static void Go()
    {
        // Construct a StringBuilder to do string manipulations.
        StringBuilder sb = new StringBuilder();

        // Perform some string manipulations using the StringBuilder.
        sb.AppendFormat("{0} {1}", "Jeffrey", "Richter").Replace(" ", "-");

        // Convert the StringBuilder to a String in 
        // order to uppercase all the characters.
        String s = sb.ToString().ToUpper();

        // Clear the StringBuilder (allocates a new Char array).
        sb.Length = 0;

        // Load the uppercase String into the StringBuilder, 
        // and do more manipulations.
        sb.Append(s).Insert(8, "Marc-");

        // Convert the StringBuilder back to a String. 
        s = sb.ToString();

        // Display the String to the user.
        Console.WriteLine(s); // "JEFFREY-Marc-RICHTER"
    }
}

internal static class Formatting
{
    public static void Go()
    {
        Decimal price = 123.54M;
        String s = price.ToString("C", new CultureInfo("vi-VN"));
        MessageBox.Show(s);

        s = price.ToString("C", CultureInfo.InvariantCulture);
        MessageBox.Show(s);
    }
}

internal static class CustomFormatter
{
    public static void Go()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(new BoldInt32s(), "{0} {1} {2:M}", "Jeff", 123, DateTime.Now);
        Console.WriteLine(sb);
    }

    private sealed class BoldInt32s : IFormatProvider, ICustomFormatter
    {
        public Object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter)) return this;
            return Thread.CurrentThread.CurrentCulture.GetFormat(formatType);
        }

        public String Format(String format, Object arg, IFormatProvider formatProvider)
        {
            String s;

            IFormattable formattable = arg as IFormattable;

            if (formattable == null) s = arg.ToString();
            else s = formattable.ToString(format, formatProvider);

            if (arg.GetType() == typeof(Int32))
                return "<B>" + s + "</B>";
            return s;
        }
    }
}

internal static class Encodings
{
    public static void Go()
    {
        // This is the string we're going to encode.
        String s = "Hi there.";

        // Obtain an Encoding-derived object that knows how 
        // to encode/decode using UTF8
        Encoding encodingUTF8 = Encoding.UTF8;

        // Encode a string into an array of bytes.
        Byte[] encodedBytes = encodingUTF8.GetBytes(s);

        // Show the encoded byte values.
        Console.WriteLine("Encoded bytes: " +
                          BitConverter.ToString(encodedBytes));

        // Decode the byte array back to a string.
        String decodedString = encodingUTF8.GetString(encodedBytes);

        // Show the decoded string.
        Console.WriteLine("Decoded string: " + decodedString);
    }
}

internal static class EncodingProperties
{
    public static void Go()
    {
        foreach (EncodingInfo ei in Encoding.GetEncodings())
        {
            Encoding e = ei.GetEncoding();
            Console.WriteLine("{1}{0}" +
                              "\tCodePage={2}, WindowsCodePage={3}{0}" +
                              "\tWebName={4}, HeaderName={5}, BodyName={6}{0}" +
                              "\tIsBrowserDisplay={7}, IsBrowserSave={8}{0}" +
                              "\tIsMailNewsDisplay={9}, IsMailNewsSave={10}{0}",
                Environment.NewLine,
                e.EncodingName, e.CodePage, e.WindowsCodePage,
                e.WebName, e.HeaderName, e.BodyName,
                e.IsBrowserDisplay, e.IsBrowserSave,
                e.IsMailNewsDisplay, e.IsMailNewsSave);
        }
    }
}

internal static class CodePageConverter
{
    public static void Go(String[] args)
    {
        if (args.Length != 4)
        {
            Console.WriteLine(
                "CodePageConverter <SrcFile> <SrcCodePage> <DstFile> <DstCodePage>{0}{0}" +
                "Examples:{0}" +
                "   CodePageConverter InFile.txt 65001 OutFile.txt 1200{0}" +
                "      => Converts from UTF-8 (codepage 65001) to UTF-16 (Little Endian){0}{0}" +
                "   CodePageConverter InFile.txt 932 OutFile.txt UTF-8{0}" +
                "      => Converts from shift-jis (codepage 932) to UTF-8",
                Environment.NewLine);
            return;
        }

        // Open the source stream using the specified encoding
        // Create the destination stream using the specified encoding
        using (StreamReader srcText = new StreamReader(args[0], GetEncoding(args[1])))
        using (StreamWriter dstText = new StreamWriter(args[2], false, GetEncoding(args[3])))
        {
            // Read from the source stream and write to the destination stream
            dstText.Write(srcText.ReadToEnd());
        } // Close both streams
    }

    private static Encoding GetEncoding(String s)
    {
        try
        {
            // Assume the user passed an integer identifying a code page
            return Encoding.GetEncoding(Int32.Parse(s));
        }
        catch (FormatException)
        {
            // The user didn't pass an integer code page value
        }

        // Assume the user passed a string identifying a code page
        return Encoding.GetEncoding(s);
    }
}

internal static class Base64Encoding
{
    public static void Go()
    {
        // Get a set of 10 randomly generated bytes
        Byte[] bytes = new Byte[10];
        new Random().NextBytes(bytes);

        // Display the bytes
        Console.WriteLine(BitConverter.ToString(bytes));

        // Decode the bytes into a base-64 string and show the string
        String s = Convert.ToBase64String(bytes);
        Console.WriteLine(s);

        // Encode the base-64 string back to bytes and show the bytes
        bytes = Convert.FromBase64String(s);
        Console.WriteLine(BitConverter.ToString(bytes));
    }
}

internal static class UsingSecureString
{
    public static void Go()
    {
        using (SecureString ss = new SecureString())
        {
            Console.Write("Please enter password: ");
            while (true)
            {
                ConsoleKeyInfo cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Enter) break;

                // Append password characters into the SecureString
                ss.AppendChar(cki.KeyChar);
                Console.Write("*");
            }

            Console.WriteLine();

            // Password entered, display it for demonstrattion purposes
            DisplaySecureString(ss);
        }
        // After 'using', the SecureString is Disposed; no sensitive data in memory
    }

    // This method is unsafe because it accesses unmanaged memory
    private unsafe static void DisplaySecureString(SecureString ss)
    {
        Char* pc = null;
        try
        {
            // Decrypt the SecureString into an unmanaged memory buffer
            pc = (Char*)Marshal.SecureStringToCoTaskMemUnicode(ss);

            // Access the unmanaged memory buffer that 
            // contains the decrypted SecureString
            for (Int32 index = 0; pc[index] != 0; index++)
                Console.Write(pc[index]);
        }
        finally
        {
            // Make sure we zero and free the unmanaged memory buffer that contains
            // the decrypted SecureString characters
            if (pc != null)
                Marshal.ZeroFreeCoTaskMemUnicode((IntPtr)pc);
        }
    }
}