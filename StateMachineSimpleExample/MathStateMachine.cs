public class MathStateMachine
{
    private int _state = -1;
    private IntAwaiter? _awaiter;
    private int _localVariableValue;

    public async void MoveNext()
    {
        IntAwaiter awaiter;
        if (_state != 0)
        {
            MachineConsoleOutput("[StateMachine] Starting calculation...");
            var calc = new SlowCalculator(10, 20);
            awaiter = calc.GetAwaiter();

            if (!awaiter.IsCompleted)
            {
                _state = 0;
                _awaiter = awaiter;
                MachineConsoleOutput("[StateMachine] Suspending thread...");
                _awaiter.OnCompleted(MoveNext);
                return;
            }
        }
        else
        {
            MachineConsoleOutput("[StateMachine] Waking up...");
            awaiter = _awaiter;
            _awaiter = default;

            _state = -2; // Done
        }

        _localVariableValue = awaiter.GetResult();
        MachineConsoleOutput($"[StateMachine] Result is: {_localVariableValue}");
    }

    private void MachineConsoleOutput(string message)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(message);
    }
}