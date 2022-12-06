using DustInTheWind.ParallelEvolution.Logging;

namespace DustInTheWind.ParallelEvolution.Examples;

internal class ThreadPoolExample
{
    private readonly ManualResetEventSlim manualResetEventSlim = new();

    public WaitHandle Execute()
    {
        manualResetEventSlim.Reset();

        ThreadPool.QueueUserWorkItem(Work);

        return manualResetEventSlim.WaitHandle;
    }

    internal void Work(object? state)
    {
        ConsoleLog.WriteLine("I am a thread from the thread pool.");
        ConsoleLog.WriteLine("Let me sleep for 3 seconds.");
        Thread.Sleep(3000);
        ConsoleLog.WriteLine("Parallel execution ended.");

        manualResetEventSlim.Set();
    }
}