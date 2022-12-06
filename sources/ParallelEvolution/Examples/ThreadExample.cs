using DustInTheWind.ParallelEvolution.Logging;

namespace DustInTheWind.ParallelEvolution.Examples;

internal class ThreadExample
{
    private readonly ManualResetEventSlim manualResetEventSlim = new();

    public WaitHandle Execute()
    {
        manualResetEventSlim.Reset();

        Thread thread = new(Work);
        thread.Start();

        return manualResetEventSlim.WaitHandle;
    }

    private void Work()
    {
        ConsoleLog.WriteLine("I am a thread.");
        ConsoleLog.WriteLine("Let me sleep for 3 seconds.");
        Thread.Sleep(3000);
        ConsoleLog.WriteLine("Parallel execution ended.");

        manualResetEventSlim.Set();
    }
}