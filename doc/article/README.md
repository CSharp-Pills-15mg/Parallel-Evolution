# Parallel Evolution

My target for this pill is to explain the `async-await` construct. I decided to do that by providing a short overview on the evolution of parallel execution of some code.

I will talk about:

- `Thread`
- `ThreadPool`
- `Task`
- `async-await`

Let's begin with a metaphor that may better explain the difference between `Thread` and `Task`.

## `Thread` vs `Task`

A thread is like an employ that does some work in a company. So:

- **the application = the company**
- **a thread = an employ**

Fallowing the same idea, the tasks are the projects that the employ must do.

- **a task = a project**

### Hire and Fire Strategy (when projects are scarce)

When the company has few and rare projects, it does not afford to keep employs around, so the strategy is to hire them for a specific project and then immediately fire them, when the project is done.

| Real Life                                          | Programming                                                |
| -------------------------------------------------- | ---------------------------------------------------------- |
| The company has a project to be done for a client. | The application has some task to be executed for the user. |
| The company hires an employ.                       | The application creates a new `Thread` object.             |
| The employ completes the project.                  | The thread completes the task.                             |
| The employ is fired.                               | The thread is destroyed.                                   |

### Hire and Keep Strategy (when constant flow of projects)

When the company has a constant flow of projects that need to be implemented, a better approach is to keep a number of employs around and reallocate them on the next project when the previous one is done.

The same is true for threads and tasks.

| Real Life                                                    | Programming                                                  |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| The company has a project to be done for a client.           | The application has a `Task` to be executed for the user.    |
| The company looks for a free employ, one that already exists in the company, to work on the project. | The application uses the `ThreadPool` to find a free `Thread` that can execute the `Task`. |
| If no free employs exist, the company may decide to wait until one becomes free or hire a new one. | If no free `Thread`s exist, the `ThreadPool` may decide to wait until one becomes free or create a new one. (The `ThreadPool` has a max number of threads that are allowed to be created.) |
| After the project is finished, the employ remains in the company and will be allocated on another project. | After the `Task` is completed, the `Thread` remains in the `ThreadPool`, waiting for another `Task`. |

## `Thread`

Let's create a thread that executes some code.

```csharp
internal class ThreadExample
{
    public void Execute()
    {
        Thread thread = new(Work);
        thread.Start();
    }

    private void Work()
    {
        // Some work to be done.
    }
}
```

The `Work` method will be executed on a different thread, in parallel with the main thread.

### Background vs Foreground Threads

The newly created thread, by default is a foreground thread. As long as there is at least a foreground thread running in the application, the application will not be closed. Even if the main thread is finished

> **Note**
>
> In our example, while our newly created thread is running, the main thread may finish its execution, but the application will still be kept alive by this second thread.

When a thread is manually created (with the `new` keyword), we have the option to create a foreground (default) or background thread. To create a background thread:

```csharp
Thread thread = new(Work);
thread.IsBackground = true;
```

## `ThreadPool`

The same job can be done using a thread from the `ThreadPool`.

For that, we must send the method to the `ThreadPool` which will find a thread "willing" to execute out function.

```csharp
internal class ThreadPoolExample
{
    public void Execute()
    {
        ThreadPool.QueueUserWorkItem(Work);
    }

    internal void Work(object? state)
    {
        // Some work to be done.
    }
}
```

> **Important note about background threads**
>
> When using threads from the `ThreadPool`, they are always background threads. Background threads cannot keep the application alive. If the main thread is finished before the background thread does its job, the execution of the `Work` method will be brutally stopped and the application will close.

## `Task`

A task receives on its constructor the method that needs to be executes.

When the task is started, it automatically queues the method to be executed by the `ThreadPool`.

```csharp
internal class TaskExample
{
    public Task Execute2()
    {
        Task task = new(() =>
        {
            // Some work to be done.
        });
        task.Start();
        
        return task;
    }
}
```

A easier way to do the same thing is to use the static method `Task.Run()`:

```csharp
internal class TaskExample
{
    public Task Execute()
    {
        return Task.Run(() =>
        {
            // Some work to be done.
        });
    }
}
```

The `Task.Run()` method is creating a new `Task` object containing the specified method (delegate) to be executed and also queues it to be executed by a thread from the `ThreadPool`.

There are some advantages for using `Tasks`. One of them is the ease with which we can schedule additional work to be performed after the task is finished, using the `ContinueWith()` method. For example:

```csharp
internal class Counsumer
{
    private Task Execute()
    {
        TaskExample taskExample = new();
        Task task = taskExample.Execute();

        return task.ContinueWith(t =>
        {
            Console.WriteLine("Some additional work to be done after the task is finished.");
        });
    }
}
```

## `async-await`

Another way of scheduling some additional work after the first task is done, is by using the `async-await` construct:

```csharp
internal class Counsumer
{
    private async Task Execute()
    {
        TaskExample taskExample = new();
        await taskExample.Execute();

        Console.WriteLine("Some additional work to be done after the task is finished.");
    }
}
```

> **Important Note**
>
> The `ContinueWith()` method does NOT have, in fact, an identical behavior with `async-await`.
>
> For example, if an exception is thrown, the two constructions behave differently, but this topic is the subject for another time.
>
> For now, I find the analogy between two constructs useful for easier understanding the `async-await`.