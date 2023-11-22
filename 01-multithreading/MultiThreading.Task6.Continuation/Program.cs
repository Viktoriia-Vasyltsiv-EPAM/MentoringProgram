/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            await RunTaskAAsync();
            await RunTaskBAsync();
            await RunTaskCAsync();
            await RunTaskDAsync();

            Console.ReadLine();
        }

        static async Task RunTaskAAsync()
        {
            Console.WriteLine($"a.    Continuation task should be executed regardless of the result of the parent task.\n");

            Task parentTask = Task.Run(() =>
            {
                Console.WriteLine("Parent task threw an exception.");
                throw new Exception();
            });

            var continuationTask = parentTask.ContinueWith(antecedent =>
            {
                Console.WriteLine("Second task is complete despite the parent task throwing an exception.");
            });

            try
            {
                await parentTask;
                await continuationTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static async Task RunTaskBAsync()
        {
            Console.WriteLine($"b.    Continuation task should be executed when the parent task finished without success.\n");

            Task parentTask = Task.Run(() =>
            {
                Console.WriteLine("Parent task threw an exception.");
                throw null;
            });

            var continuationTask = parentTask.ContinueWith(antecedent =>
            {
                Console.WriteLine("Second task is complete.");
            }, TaskContinuationOptions.OnlyOnFaulted);

            try
            {
                await parentTask;
                await continuationTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static async Task RunTaskCAsync()
        {
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Task parentTask = Task.Run(() =>
            {
                Console.WriteLine($"Parent fails in thread - {Thread.CurrentThread.ManagedThreadId}");
                throw null;
            });
            await parentTask.ContinueWith(antecedent =>
            {
                if (antecedent.IsFaulted)
                {
                    Console.WriteLine($"Continuation task is running and parent task thread - {Thread.CurrentThread.ManagedThreadId} is reused.");
                }
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        static async Task RunTaskDAsync()
        {
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            using var cts = new CancellationTokenSource();
            

            Task parentTask = Task.Run(() =>
            {
                if (cts.IsCancellationRequested)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    Console.WriteLine("Parent task was canceled.");
                }

            }, cts.Token);

            Task continuationTask =
                parentTask.ContinueWith(
                    antecedent => Console.WriteLine("The continuation is running."),
                    TaskContinuationOptions.OnlyOnCanceled);

            cts.Cancel();

            try
            {
                await Task.WhenAll(parentTask, continuationTask);
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
