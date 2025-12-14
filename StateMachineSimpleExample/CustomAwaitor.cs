using System.Runtime.CompilerServices;

// This represents the "Task<int>"
public class SlowCalculator(int a, int b)
{
    // Returns an awaiter that produces an 'int'
    public IntAwaiter GetAwaiter() => new(a, b);
}

// The Awaiter that holds the result
public class IntAwaiter(int a, int b) : INotifyCompletion
{
    private int _result; // <--- The storage for our return value

    public bool IsCompleted { get; private set; } = false;

    // THIS is the method that 'returns' the value to the variable
    // var result = await ... calls this!
    public int GetResult() => _result;

    public void OnCompleted(Action continuation)
    {
        // Simulate background work
        new Thread(() =>
        {
            Thread.Sleep(3000); // Wait 1 second

            _result = a + b;    // Calculate the result
            IsCompleted = true; // Mark done

            continuation();     // Resume the State Machine
        }).Start();
    }
}
