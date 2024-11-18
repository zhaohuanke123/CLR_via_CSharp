using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: CLSCompliant(true)]

[Serializable]
[DefaultMemberAttribute("Main")]
[DebuggerDisplayAttribute("Richter", Name = "Jeff", Target = typeof(Program))]
public sealed class Program
{
    [Conditional("Debug")]
    [Conditional("Release")]
    public void DoSomething()
    {
    }

    public Program()
    {
    }

    [CLSCompliant(true)]
    [STAThread]
    public static void Main()
    {
        // 显示应用于这个类型的特性类
        ShowAttributes(typeof(Program));

        // 获取与类型关联的方法集
        var members = from m in typeof(Program).GetTypeInfo().DeclaredMembers.OfType<MethodBase>()
            where m.IsPublic
            select m;

        foreach (MemberInfo member in members)
        {
            // 显示应用于这个成员的特性集
            ShowAttributes(member);
        }
    }

    private static void ShowAttributes(MemberInfo attributeTarget)
    {
        IList<CustomAttributeData> attributes = CustomAttributeData.GetCustomAttributes(attributeTarget);

        Console.WriteLine("Attributes applied to {0}: {1}", attributeTarget.Name,
            (attributes.Count == 0 ? "None" : String.Empty));

        foreach (CustomAttributeData attribute in attributes)
        {
            // 显示所应用的每个特性的类型
            Type t = attribute.Constructor.DeclaringType;
            Console.WriteLine(" {0}", t.ToString());
            Console.WriteLine("    Constructor called={0}", attribute.Constructor);

            IList<CustomAttributeTypedArgument> posArgs = attribute.ConstructorArguments;
            Console.WriteLine("    Positonal arguments passed to constructor:" +
                              ((posArgs.Count == 0) ? " None" : String.Empty));
            foreach (CustomAttributeTypedArgument pa in posArgs)
            {
                Console.WriteLine("    Type={0}, Value={1}", pa.ArgumentType, pa.Value);
            }

            IList<CustomAttributeNamedArgument> namedArgs = attribute.NamedArguments;
            Console.WriteLine("    Named arguments set after construction:" +
                              ((namedArgs.Count == 0) ? " None" : String.Empty));
            foreach (CustomAttributeNamedArgument na in namedArgs)
            {
                Console.WriteLine("    Name={0}, Type={1}, Value={2}", na.MemberInfo.Name, na.TypedValue.ArgumentType,
                    na.TypedValue.Value);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
}