using System.Threading.Tasks.Sources;

class ThreadPoolTaskSource : IValueTaskSource
{
    private ManualResetValueTaskSourceCore<object> _core;

    public ValueTask WaitForThreadAsync()
    {
        _core.Reset();
        return new ValueTask(this, _core.Version);
    }

    public void ReleaseThread()
    {
        _core.SetResult(null);
    }

    public void OnCompleted(Action<object> continuation, object state, short token,
        ValueTaskSourceOnCompletedFlags flags)
    {
        _core.OnCompleted(continuation, state, token, flags);
    }

    public ValueTaskSourceStatus GetStatus(short token) => _core.GetStatus(token);

    public void GetResult(short token) => _core.GetResult(token);
}

class EventLoop : IValueTaskSource
{
    private ManualResetValueTaskSourceCore<object> _core;

    public ValueTask WaitForEventAsync()
    {
        _core.Reset();
        return new ValueTask(this, _core.Version);
    }

    public void TriggerEvent()
    {
        _core.SetResult(null);
    }

    public void OnCompleted(Action<object> continuation, object state, short token,
        ValueTaskSourceOnCompletedFlags flags)
    {
        _core.OnCompleted(continuation, state, token, flags);
    }

    public ValueTaskSourceStatus GetStatus(short token) => _core.GetStatus(token);

    public void GetResult(short token) => _core.GetResult(token);
}