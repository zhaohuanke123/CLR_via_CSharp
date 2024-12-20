// return type

using System.ComponentModel;
using System.Diagnostics;

var lambdaWithReturnValue0 = int? () => null;
// return type and input type
var lambdaWithReturnValue1 = int? (string s)
    => string.IsNullOrEmpty(s) ? 1 : null;
// Func<bool, object>
var choose = object (bool b) => b ? 1 : "two";

// Console.WriteLine(lambdaWithReturnValue0.Invoke());
// Console.WriteLine(lambdaWithReturnValue1.Invoke("asdasd"));
// Console.WriteLine(choose.Invoke(false));

var AttrLambda = [Description("123")]() => Console.WriteLine("123");
// AttrLambda.Invoke();

var refFunc = (ref int x) => { x++; };
var outFunc = (out int x) => { x = -1; };
var inFunc = (in int x) => { };

var num = 1;
refFunc(ref num);
// Console.WriteLine(num);
outFunc(out num);
// Console.WriteLine(num);

Console.WriteLine(lambdaWithReturnValue0.GetType());
Console.WriteLine(lambdaWithReturnValue1.GetType());

Span<int> p = new int[1000];
for (int i = 0; i < p.Length; i++)
{
    p[i] = i;
}