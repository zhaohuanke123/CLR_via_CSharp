namespace P11
{
    using System;

// 为这个事件定义从 EventArgs 派生的类型
    public class FooEventArgs : EventArgs
    {
    }

    public class TypeWithLotsOfEvents
    {
        // 定义私有实例字段来引用集合
        // 集合用于管理一组“事件/委托”对
        // 注意：EventSet 类型不是 FCL 的一部分，它是我自己的类型
        private readonly EventSet m_eventSet = new EventSet();

        // 受保护的属性使派生类型能访问集合
        protected EventSet EventSet
        {
            get { return m_eventSet; }
        }

        #region 用于支持 Foo 事件的代码(为附加的事件重复这个模式)

        // 定义 Foo 事件必要的成员
        // 2a. 构造一个静态只读对象来标识这个事件.
        // 每个对象都有自己的哈希码，以便在对象的集合中查找这个事件的委托链表
        protected static readonly EventKey s_fooEventKey = new EventKey();

        // 2b. 定义事件的访问器方法，用于在集合中增删委托
        public event EventHandler<FooEventArgs> Foo
        {
            add { m_eventSet.Add(s_fooEventKey, value); }
            remove { m_eventSet.Remove(s_fooEventKey, value); }
        }

        // 2c. 为这个事件定义受保护的虚方法 OnFoo
        protected virtual void OnFoo(FooEventArgs e)
        {
            m_eventSet.Raise(s_fooEventKey, this, e);
        }

        // 2d. 定义将输入转换成这个事件的方法
        public void SimulateFoo()
        {
            OnFoo(new FooEventArgs());
        }

        #endregion
    }
}