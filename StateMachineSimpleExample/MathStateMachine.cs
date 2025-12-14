public class MathStateMachine
{
    private int _state = -1;
    private IntAwaiter _awaiter; // Holds the reference to the running calc

    // We need a field to store the local variable 'value' 
    // because it needs to survive between function calls.
    private int _localVariableValue;

    public void MoveNext()
    {
        if (_state == -1) // START
        {
            MachineConsoleOutput("[StateMachine] Starting calculation...");

            // 1. Create the operation
            var calc = new SlowCalculator(10, 20);

            // 2. Get the awaiter
            _awaiter = calc.GetAwaiter();

            if (!_awaiter.IsCompleted)
            {
                _state = 0; // "Paused at await #1"

                // 3. Register callback and RETURN immediately
                MachineConsoleOutput("[StateMachine] Suspending thread...");
                _awaiter.OnCompleted(MoveNext);
                return;
            }
        }

        if (_state == 0) // RESUME
        {
            MachineConsoleOutput("[StateMachine] Waking up...");

            // 4. EXTRACT THE DATA
            // This is equivalent to: int value = await ...
            _localVariableValue = _awaiter.GetResult();

            // 5. Continue with the rest of the code
            MachineConsoleOutput($"[StateMachine] Result is: {_localVariableValue}");

            _state = -2; // Done
        }
    }











    private void MachineConsoleOutput(string message)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(message);
    }
}