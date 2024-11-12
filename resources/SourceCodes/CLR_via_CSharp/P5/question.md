```csharp
    public static void Test ()
    {
        GameObject go; // 假设go已经有引用了
        DestroyImmediate(go);
        
        if (go == null) {
            // 这个会进来，因为 == 被重载了
        }
       // 根据书上的一句话  如果对象引用 null，is 操作符总是返回 false
       // 这里的 "如果对象引用 null" 会收到 == 重载 的影响吗？
        if (o is null) {
           // 进过测试，这里实际结果不会进入分支 
            ...
        }
        // 我的理解是 Destory了go 是C++成面的对象，但是C# 这里的go还是有引用的，所以go不是null
    }
```
