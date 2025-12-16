using System.Runtime.CompilerServices;

// This represents the "Task<int>"
public class SlowCalculator(int a, int b)
{
    public IntAwaiter GetAwaiter() => new(a, b);
}

public class IntAwaiter(int a, int b) : INotifyCompletion
{
    private int _result;

    public bool IsCompleted { get; private set; } = false;

    public int GetResult() => _result;

    public void OnCompleted(Action continuation)
    {
        // Simulate background work
        new Thread(() =>
        {
            Thread.Sleep(3000);

            _result = a + b;
            IsCompleted = true;

            continuation(); // Resume the State Machine
        }).Start();
    }
}
