/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static List<int> sharedCollection = new List<int>();
        static ManualResetEvent elementAdded = new ManualResetEvent(false);
        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            Task t1 = Task.Run(() => AddElementToCollection());
            Task t2 = Task.Run(() => PrintCollection());

            Task.WaitAll(t1, t2);

            Console.ReadLine();
        }

        static void AddElementToCollection()
        {
            for (int i = 1; i <= 10; i++)
            {
                lock (sharedCollection)
                {
                    sharedCollection.Add(i);
                    elementAdded.Set();
                }
                Thread.Sleep(100);
            }
        }

        static void PrintCollection()
        {
            for (int i = 1; i <= 10; i++)
            {
                elementAdded.WaitOne(); 
                lock (sharedCollection)
                {
                    List<int> elementsToPrint = new List<int>(sharedCollection);
                    Console.WriteLine("Collection after adding " + i + ": [" + string.Join(", ", elementsToPrint) + "]");
                }
                elementAdded.Reset();
            }
        }
    }
}
