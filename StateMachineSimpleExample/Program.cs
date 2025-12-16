//var slowCalculatorTask = new SlowCalculator(1, 2);
//var result = await slowCalculatorTask;

//Console.WriteLine($"Result is: {result}");

//Console.ReadLine();




using System.Diagnostics;

MainThreadConsoleOutput("[Main] Creating Machine...");
var machine = new MathStateMachine();

MainThreadConsoleOutput("[Main] Starting Machine...");
machine.MoveNext();

MainThreadConsoleOutput("[Main] Machine started." +
"The main thread is freed up and begins another work: counting to 10.");

//This code is intended only to demonstrate that the main thread is 
// free from previous work and can do something else.
var stopwatch = new Stopwatch();
stopwatch.Start();

var i = 0;
var elapsedSeconds = 0;
while (stopwatch.Elapsed.Seconds < 10)
{
    if (elapsedSeconds < stopwatch.Elapsed.Seconds)
    {
        MainThreadConsoleOutput($"[Main] {i}");
        i++;
        elapsedSeconds++;
    }
}
stopwatch.Stop();

FinishedMessage();

Console.WriteLine();


static void MainThreadConsoleOutput(string message)
{
    Console.BackgroundColor = ConsoleColor.White;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.WriteLine(message);
}
static void FinishedMessage()
{
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("Finished.");
}
