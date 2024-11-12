namespace P11
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

// 这个类的目的是在使用 EventSet 时，提供
// 多一点的类型安全性和代码可维护性
    public sealed class EventKey
    {
    }

    public sealed class EventSet
    {
        // 该私有字典用于维护 EventKey -> Delegate 映射
        private readonly Dictionary<EventKey, Delegate> m_events = new Dictionary<EventKey, Delegate>();

        // 添加 EventKey -> Delegate 映射(如果 EventKey 不存在)，
        // 或者将委托和现有的 EventKey 合并
        public void Add(EventKey eventKey, Delegate handler)
        {
            Monitor.Enter(m_events);
            Delegate d;
            m_events.TryGetValue(eventKey, out d);
            m_events[eventKey] = Delegate.Combine(d, handler);
            Monitor.Exit(m_events);
        }

        // 从 EventKey(如果它存在)删除委托，并且
        // 在删除最后一个委托时删除 EventKey -> Delegate 映射
        public void Remove(EventKey eventKey, Delegate handler)
        {
            Monitor.Enter(m_events);
            // 调用 TryGetValue，确保在尝试从集合中删除不存在的 EventKey 时不会抛出异常
            Delegate d;
            if (m_events.TryGetValue(eventKey, out d))
            {
                d = Delegate.Remove(d, handler);

                // 如果还有委托，就设置新的头部(地址)，否则删除 EventKey
                if (d != null) m_events[eventKey] = d;
                else m_events.Remove(eventKey);
            }

            Monitor.Exit(m_events);
        }

        // 为指定的 EventKey 引发事件
        public void Raise(EventKey eventKey, Object sender, EventArgs e)
        {
            // 如果 EventKey 不在集合中，不抛出异常
            Delegate d;
            Monitor.Enter(m_events);
            m_events.TryGetValue(eventKey, out d);
            Monitor.Exit(m_events);

            if (d != null)
            {
                // 由于字典可能包含几个不同的委托类型，
                // 所以无法在编译时构造一个类型安全的委托调用
                // 因此，我调用 System.Delegate 类型的 DynamicInvoke
                // 方法，以一个对象数组的形式向它传递回调方法的参数
                // 在内部，DynamicInvoke 会向调用的回调方法查证参数的
                // 类型安全性，并调用方法
                // 如果存在类型不匹配的情况，DynamicInvoke 会抛出异常
                d.DynamicInvoke(new Object[] { sender, e });
            }
        }
    }
}