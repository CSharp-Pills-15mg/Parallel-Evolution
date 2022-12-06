namespace DustInTheWind.ParallelEvolution.Logging;

internal static class ConsoleLog
{
    public static void WriteLine(string message = "")
    {
        Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] {message}");
    }
}