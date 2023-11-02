/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        public static Random randomizer = new Random();
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var chainOfTasks = Task.Run(() => TenRandomInt())
                .ContinueWith(x => MultiplyWithRandomInt(x.Result))
                .ContinueWith(x => SortArrayByAsc(x.Result))
                .ContinueWith(x => CalculateAvgValue(x.Result));

            Console.ReadLine();
        }

        static int[] TenRandomInt()
        {
            int randomIntegerAmount = 10;

            var array = Enumerable.Range(0, randomIntegerAmount).Select(_ => randomizer.Next()).ToArray();

            Console.WriteLine();
            Console.WriteLine($"Method: {MethodBase.GetCurrentMethod().Name}");
            PrintArray(array);

            return array;
        }
        static int[] MultiplyWithRandomInt(int[] array)
        {
            int randomNumber = randomizer.Next(1, 10000);

            array = array.Select(x => x * randomNumber).ToArray();

            Console.WriteLine();
            Console.WriteLine($"Method: {MethodBase.GetCurrentMethod().Name}");
            Console.WriteLine($"Random integer:{randomNumber}");
            PrintArray(array);

            return array;
        }
        static int[] SortArrayByAsc(int[] array)
        {
            array = array.OrderBy(x => x).ToArray();

            Console.WriteLine();
            Console.WriteLine($"Method: {MethodBase.GetCurrentMethod().Name}");
            PrintArray(array);

            return array;
        }
        static double CalculateAvgValue(int[] array)
        {
            var arrayAvg = array.Average(x => x);

            Console.WriteLine();
            Console.WriteLine($"Method: {MethodBase.GetCurrentMethod().Name}");
            Console.WriteLine($"The average value: {arrayAvg}");

            return arrayAvg;
        }

        private static void PrintArray(IEnumerable<int> array)
        {
            Console.WriteLine($"Array: {string.Join(", ", array)}");
        }
    }
}
