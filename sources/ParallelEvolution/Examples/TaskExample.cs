using DustInTheWind.ParallelEvolution.Logging;

namespace DustInTheWind.ParallelEvolution.Examples;

internal class TaskExample
{
    public Task Execute()
    {
        return Task.Run(() =>
        {
            ConsoleLog.WriteLine("I am a task.");
            ConsoleLog.WriteLine("Let me sleep for 3 seconds.");
            Thread.Sleep(3000);
            ConsoleLog.WriteLine("Parallel execution ended.");
        });
    }
}