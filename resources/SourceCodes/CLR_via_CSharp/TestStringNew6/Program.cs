string s = new string("123123");

void Test<T>(T a) where T : class
{
    Console.WriteLine(a);
}

Test(s);