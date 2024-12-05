using System;

namespace TestChar
{
	internal class Program
	{
		static void Main(string[] args)
		{
			// CharApiTest.Go(null);
			CharConversionTest.Go(null);
		}
	}
}

class CharApiTest
{
	public static void Go(string[] args)
	{
		// Test Equals
		char char1 = 'A';
		char char2 = 'A';
		char char3 = 'B';
		Console.WriteLine($"Equals Test 1: {char1.Equals(char2)} (Expected: True)"); // True
		Console.WriteLine($"Equals Test 2: {char1.Equals(char3)} (Expected: False)"); // False

		// Test CompareTo
		Console.WriteLine($"CompareTo Test 1: {char1.CompareTo(char2)} (Expected: 0)"); // 0
		Console.WriteLine($"CompareTo Test 2: {char1.CompareTo(char3)} (Expected: -1)"); // -1
		Console.WriteLine($"CompareTo Test 3: {char3.CompareTo(char1)} (Expected: 1)"); // 1

		// Test ConvertFromUtf32
		string utf16String = char.ConvertFromUtf32(0x1F600); // Unicode for 😀
		Console.WriteLine($"ConvertFromUtf32 Test: {utf16String} (Expected: 😀)");

		// Test ConvertToUtf32
		int utf32Char = char.ConvertToUtf32("\uD83D\uDE00", 0); // Pair of surrogate characters for 😀
		Console.WriteLine($"ConvertToUtf32 Test: {utf32Char} (Expected: 128512)");

		// Test ToString
		string charToString = char1.ToString();
		Console.WriteLine($"ToString Test: {charToString} (Expected: A)");

		// Test Parse
		char parsedChar = char.Parse("Z");
		Console.WriteLine($"Parse Test: {parsedChar} (Expected: Z)");

		// Test TryParse
		bool isParsed = char.TryParse("X", out char tryParsedChar);
		Console.WriteLine($"TryParse Test 1: {isParsed} (Expected: True)");
		Console.WriteLine($"TryParse Test 2: {tryParsedChar} (Expected: X)");

		bool isParsedInvalid = char.TryParse("Invalid", out char invalidChar);
		Console.WriteLine($"TryParse Invalid Test: {isParsedInvalid} (Expected: False)");
	}
}


class CharConversionTest
{
	public static void Go(string[] args)
	{
		char charValue = char.MaxValue;

		// 强制类型转换 (直接转换为数值)
		checked
		{
			byte intValue = (byte)charValue;
			Console.WriteLine($"强制类型转换: Char '{charValue}' -> Int {intValue} (Expected: 65)");
		}

		// 使用 Convert 类型
		try
		{
			int convertedValue = Convert.ToInt32(charValue);
			Console.WriteLine($"System.Convert 转换: Char '{charValue}' -> Int {convertedValue} (Expected: 65)");
		}
		catch (OverflowException e)
		{
			Console.WriteLine($"System.Convert 转换抛出异常: {e.Message}");
		}

		// 使用 IConvertible 接口
		try
		{
			IConvertible convertible = charValue; // 显式转换为 IConvertible
			int iConvertibleValue = convertible.ToInt32(null); // null 表示无格式提供者
			Console.WriteLine($"IConvertible 转换: Char '{charValue}' -> Int {iConvertibleValue} (Expected: 65)");
		}
		catch (InvalidCastException e)
		{
			Console.WriteLine($"IConvertible 转换抛出异常: {e.Message}");
		}

		// 测试无效转换
		try
		{
			bool booleanValue = Convert.ToBoolean(charValue); // 无效转换
			Console.WriteLine($"无效转换: Char '{charValue}' -> Boolean {booleanValue}");
		}
		catch (InvalidCastException e)
		{
			Console.WriteLine($"无效转换抛出异常: {e.Message} (Expected: System.InvalidCastException)");
		}

		// 测试 IConvertible 的无效转换
		try
		{
			IConvertible convertible = charValue;
			bool invalidBoolean = convertible.ToBoolean(null);
			Console.WriteLine($"IConvertible 无效转换: {invalidBoolean}");
		}
		catch (InvalidCastException e)
		{
			Console.WriteLine($"IConvertible 无效转换抛出异常: {e.Message} (Expected: System.InvalidCastException)");
		}
	}
}
