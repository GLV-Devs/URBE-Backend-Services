﻿using System.Collections.Concurrent;
using Serilog;

namespace Urbe.Programacion.Shared.Common;

public static class BackgroundTaskStore
{
    private static readonly HashSet<Task> _tasks = new();
    private static readonly ConcurrentQueue<Func<CancellationToken, Task>> _funcs = new();
    private static bool active = true;

    static BackgroundTaskStore()
    {
        AppDomain.CurrentDomain.ProcessExit += (s, e) => active = false;
    }

    public static bool Add(Task task)
    {
        if (active is false) return false;
        lock (_tasks)
            _tasks.Add(task);
        return true;
    }

    /// <summary>
    /// Adds a new background task to the store
    /// </summary>
    /// <param name="task">The task to add</param>
    /// <param name="onCompletion">An action to execute when the task completes, whether due to an error or not.</param>
    public static bool Add(Func<CancellationToken, Task> task)
    {
        if (active is false) return false;
        _funcs.Enqueue(task);
        return true;
    }

    /// <summary>
    /// Performs a single sweep on the store, searching for completed tasks to await
    /// </summary>
    public static async Task Sweep(ILogger? log, CancellationToken ct = default)
    {
        List<Exception>? exceptions = null;

        if (_funcs.IsEmpty is false)
        {
            lock (_tasks)
                while (_funcs.TryDequeue(out var func))
                    _tasks.Add(func(ct));
        }

        foreach (var task in _tasks)
        {
            if (task.IsCompleted)
                try
                {
                    await task;
                }
                catch (Exception e)
                {
                    (exceptions ??= new()).Add(e);
                }
                finally
                {
                    lock (_tasks)
                        _tasks.Remove(task);
                }
        }

        if (exceptions?.Count is > 0)
            throw new AggregateException(exceptions);
    }
}
