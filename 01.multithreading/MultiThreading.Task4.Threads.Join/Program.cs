/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private static int _threadsAmount = 10;

        private static readonly Semaphore Semaphore = new Semaphore(0, 1);
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            DecrementUsingThread(_threadsAmount);
            DecrementUsingThreadPool(_threadsAmount);

            Console.ReadLine();
        }

        static void DecrementUsingThread(object count)
        {
            var countInt = count as int?;
            if (countInt > 0)
            {
                Console.WriteLine($"a) Thread id: {Thread.CurrentThread.ManagedThreadId}. Number: {countInt}");
                var myThread = new Thread(DecrementUsingThread);
                countInt--;
                myThread.Start(countInt);
                myThread.Join();
            }
        }

        static void DecrementUsingThreadPool(object count)
        {
            var countInt = count as int?;
            if (countInt > 0)
            {
                Console.WriteLine($"b) Thread id: {Thread.CurrentThread.ManagedThreadId}. Number: {countInt}");
                countInt--;
                ThreadPool.QueueUserWorkItem(DecrementUsingThreadPool, countInt);
                Semaphore.WaitOne();
            }
            Semaphore.Release();
        }
    }
}
