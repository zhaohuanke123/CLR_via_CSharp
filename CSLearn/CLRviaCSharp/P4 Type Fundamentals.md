
> [!PDF|] [[Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012.pdf#page=118&selection=178,0,184,14|Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012, p.92]]
> > Here’s what the new operator does:
> 
> new 的具体过程

> [!PDF|] [[Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012.pdf#page=135&selection=18,0,56,17|Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012, p.109]]
> > Note that if Employee’s Lookup method had discovered that Joe was just an Employee and not a Manager, Lookup would have internally constructed an Employee object whose type object pointer member would have referred to the Employee type object, causing Employee’s implementation of GetProgressReport to execute instead of Manager’s implementation
> 
>如何 调用虚方法？

> [!PDF|] [[Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012.pdf#page=123&selection=204,0,219,9|Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012, p.97]]
> > Note C# allows a type to define conversion operator methods, as discussed in the “Conversion Operator Methods” section of Chapter 8, “Methods.” These methods are invoked only when using a cast expression; they are never invoked when using C#'s as or is operator.
> 
> operator 重载只在转换时起作用，as关键字不起作用

> [!PDF|] [[Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012.pdf#page=148&selection=58,67,80,34|Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012, p.122]]
> >  Due to performance issues with this default implementation, when defining your own value types, you should override and provide explicit implementations for the Equals and GetHashCode methods. I’ll cover the Equals and GetHashCode methods at the end of this chapter
> 
> GetHashCode 为什么要重写方法，有什么性能问题

> [!PDF|] [[Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012.pdf#page=149&selection=6,0,6,50|Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012, p.123]]
> > How the CLR Controls the Layout of a Type’s Fields
> 
> CLR 处理 字节对齐，class 和 struct 默认对齐上的差异

> [!PDF|] [[Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012.pdf#page=152&selection=48,65,61,43|Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012, p.126]]
> > First, the address of the Point fields in the boxed Point object is obtained. This process is called unboxing. Then, the values of these fields are copied from the heap to the stack-based value type instance
> 
>  拆装箱的含义

> [!PDF|] [[Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012.pdf#page=159&selection=51,45,66,29|Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012, p.133]]
> > then the CLR can invoke the method nonvirtually because value types are implicitly sealed and cannot have any types derived from them. In addition, the value type instance being used to invoke the virtual method is not boxed. However, if your override of the virtual method calls into the base type's implementation of the method, then the value type instance does get boxed when calling the base type's implementation so that a reference to a heap object gets passed to the this pointer into the base method.
> 
> 值类型 与 虚方法

> [!PDF|] [[Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012.pdf#page=165&selection=0,9,47,28|Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012, p.139]]
> > CHAPTER 5 Primitive, Reference, and Value Types 139 the Point as a class instead of a struct to appreciate the different behavior that results. Finally, you'll be very happy to know that the core value types that ship in the FCL—Byte, Int32, UInt32, Int64, UInt64, Single, Double, Decimal, BigInteger, Complex, all enums, and so on—are all immutable,
> 
> 不可变指的是 值类型对象本身不可变 如 5（一个Int32对象）本身不可修改

> [!PDF|] [[Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012.pdf#page=166&selection=305,1,329,13|Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012, p.140]]
> >  You should always call ReferenceEquals if you want to check for identity (if two references point to the same object). You shouldn’t use the C# == operator (unless you cast both operands to Object first) because one of the operands’ types could overload the == operator, giving it semantics other than identity
> 
> 正确判断对象引用相同的方式

> [!PDF|] [[Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012.pdf#page=167&selection=48,0,65,78|Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012, p.141]]
> > Internally, ValueType’s Equals method uses reflection (covered in Chapter 23, “Assembly Loading and Reflection”) to accomplish step 3. Because the CLR’s reflection mechanism is slow, when defining your own value type, you should override Equals and provide your own implementation to improve the performance of value equality comparisons that use instances of your type.
> 
> 值类型 Equal 的实现，使用反射
> 有优化，如果成员没有引用类型，就使用按位对比进行优化
> [valuetype.cs (microsoft.com)](https://referencesource.microsoft.com/#mscorlib/system/valuetype.cs,d8b9b308e644b983)

> [!PDF|] [[Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012.pdf#page=192&selection=6,0,12,11|Microsoft.Press.CLR.via.Csharp.4th.Edition.Oct.2012, p.166]]
> > were to call a value type’s virtual method virtually, the CLR would need to have a reference to the value type’s type object in order to refer to the method table within it. This requires boxing the value type. Boxing puts more pressure on the heap, forcing more frequent garbage collections and hurting performance
> 
> 值类型的方法调用 call callvir？