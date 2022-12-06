using System.Diagnostics;
using DustInTheWind.ParallelEvolution.Examples;
using DustInTheWind.ParallelEvolution.Logging;

namespace DustInTheWind.ParallelEvolution;

internal static class Program
{
    private static async Task Main()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        try
        {
            ConsoleLog.WriteLine("Main thread started.");

            //RunThreadExample();
            //RunThreadPoolExample();
            //RunTaskExample();
            //await RunAsyncAwaitExample();
            //await RunAsyncAwaitExample2();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
            ConsoleLog.WriteLine();
            ConsoleLog.WriteLine($"Program ended. Execution time: {stopwatch.Elapsed}");
        }
    }

    private static void RunThreadExample()
    {
        ThreadExample threadExample = new();
        WaitHandle waitHandle = threadExample.Execute();
        waitHandle.WaitOne();

        ConsoleLog.WriteLine("Example is finished.");
    }

    private static void RunThreadPoolExample()
    {
        ThreadPoolExample threadPoolExample = new();
        WaitHandle waitHandle = threadPoolExample.Execute();
        waitHandle.WaitOne();

        ConsoleLog.WriteLine("Example is finished.");
    }

    private static void RunTaskExample()
    {
        TaskExample taskExample = new();
        Task task = taskExample.Execute();
        task.Wait();

        ConsoleLog.WriteLine("Example is finished.");
    }

    private static async Task RunAsyncAwaitExample()
    {
        TaskExample taskExample = new();
        await taskExample.Execute();

        ConsoleLog.WriteLine("Example is finished.");
    }

    private static Task RunAsyncAwaitExample2()
    {
        TaskExample taskExample = new();
        Task task = taskExample.Execute();

        return task.ContinueWith(t =>
        {
            ConsoleLog.WriteLine("Example is finished.");
        });
    }
}