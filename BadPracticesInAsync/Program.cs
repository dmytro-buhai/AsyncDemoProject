using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    private static readonly object _lock = new object();
    private static readonly object _lock1 = new object();
    private static readonly object _lock2 = new object();

    static void Main(string[] args)
    {
        Console.WriteLine("Starting BAD async examples...\n");

        // Uncomment ONE at a time to test behavior
        //DeadlockInsideLock();
        //AsyncVoidExample();
        //AsyncWithoutAwait();
        // AwaitInsideLockExample().Wait();
        //FireAndForgetExample();
        BlockingInsideAsync().Wait();
        // OverusingTaskRun().Wait();

        Console.WriteLine("\nProgram finished.");
        Console.ReadLine();
    }

    // 1️ SYNC OVER ASYNC (Deadlock Trap)
    static void DeadlockInsideLock()
    {
        Console.WriteLine("Deadlock inside lock example started...");

        var thread1 = new Thread(ExecuteThread1);
        var thread2 = new Thread(ExecuteThread2);

        thread1.Start();
        thread2.Start();

        Console.WriteLine("DeadlockInsideLock has finished.");
    }

    static void ExecuteThread1()
    {
        lock (_lock1)
        {
            Console.WriteLine("Thread 1: Holding lock1...");
            Thread.Sleep(100);

            Console.WriteLine("Thread 1: Waiting for lock2...");
            lock (_lock2)
            {
                Console.WriteLine("Thread 1: Acquired lock2!");
            }
        }
    }

    static void ExecuteThread2()
    {
        lock (_lock2)
        {
            Console.WriteLine("Thread 2: Holding lock2...");
            Thread.Sleep(100);

            Console.WriteLine("Thread 2: Waiting for lock1...");
            lock (_lock1)
            {
                Console.WriteLine("Thread 2: Acquired lock1!");
            }
        }
    }

    // 2️ ASYNC VOID (EXCEPT EVENT HANDLERS)
    static void AsyncVoidExample()
    {
        try
        {
            Console.WriteLine("Async void example started...");
            DangerousAsyncVoid();
            Thread.Sleep(2000);
        }
        catch (Exception ex) 
        {
            Console.WriteLine("Async void execption catched...");
        }
    }

    static async void DangerousAsyncVoid()
    {
        await Task.Delay(500);
        throw new Exception("Crash from async void!");
    }

    // 3️ ASYNC METHOD WITHOUT AWAIT
    static void AsyncWithoutAwait()
    {
        Console.WriteLine("Async without await...");
        var task = GetNumberAsync();
        Console.WriteLine($"Result: {task.Result}");
    }

    static async Task<int> GetNumberAsync()
    {
        return 42;
    }

    // 4️ AWAIT INSIDE LOCK
    static async Task AwaitInsideLockExample()
    {
        Console.WriteLine("Await inside lock started...");

        lock (_lock)
        {
            // Compiler error in real code, shown for demonstration
            //await Task.Delay(1000);
            Console.WriteLine("Inside lock");
        }
    }

    // 5️ FIRE-AND-FORGET TASK - swallow exceptions
    static void FireAndForgetExample()
    {
        Console.WriteLine("Fire-and-forget example...");
        Task.Run(() => FireAndForgetAsync());
        Thread.Sleep(2000);
    }

    static async Task FireAndForgetAsync()
    {
        await Task.Delay(500);
        throw new Exception("Unexpected task exception...");
    }

    // 6️ BLOCKING CALL INSIDE ASYNC METHOD
    static async Task BlockingInsideAsync()
    {
        Console.WriteLine("Blocking inside async...");
        Thread.Sleep(1000); // blocks thread
        await Task.Delay(500);
        Console.WriteLine("Finished blocking async method");
    }

    // 7️ OVERUSING TASK.RUN FOR ASYNC I/O
    static async Task OverusingTaskRun()
    {
        Console.WriteLine("Overusing Task.Run...");
        await Task.Run(() => FakeIoAsync()); // unnecessary
    }

    static async Task FakeIoAsync()
    {
        await Task.Delay(1000);
        Console.WriteLine("Fake I/O done");
    }
}
