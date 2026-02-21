using Sort;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AddEvenSort
{

    class EnhancedSortingTest
    {
        static void Main()
        {
            int[] arraySizes = { 1000, 5000, 10000, 20000, 40000, 60000, 80000 };
            int repetitions = 1;

            foreach (int size in arraySizes)
            {
                Console.WriteLine($"\n=== РАЗМЕР МАССИВА: {size} ===");
                TestWithArraySize(size, repetitions);
                Console.WriteLine("=" + new string('=', 30));
            }
        }

        static void TestWithArraySize(int size, int repetitions)
        {
            int[] originalArray = GenerateRandomArray(size);

            var results = new List<TestResult>
            {
                TestAlgorithm("Пузырьковая", originalArray, repetitions, arr => SortingAlgorithms.BubbleSort(arr)),
                TestAlgorithm("Odd-Even (2 потока)", originalArray, repetitions, arr => SortingAlgorithms.OddEvenSort(arr, 2)),
                TestAlgorithm("Odd-Even (4 потока)", originalArray, repetitions,  arr => SortingAlgorithms.OddEvenSort(arr, 4))
            };

            PrintResultsTable(results);
        }

        static TestResult TestAlgorithm(string name, int[] originalArray, int repetitions, Action<int[]> sortFunction)
        {
            var stopwatch = new Stopwatch();
            long totalTime = 0;

            for (int i = 0; i < repetitions; i++)
            {
                int[] testArray = CopyArray(originalArray);

                stopwatch.Restart();
                sortFunction(testArray);
                stopwatch.Stop();

                totalTime += stopwatch.ElapsedMilliseconds;

                if (!IsSorted(testArray))
                    throw new InvalidOperationException($"Алгоритм {name} не отсортировал массив");
            }

            return new TestResult
            {
                AlgorithmName = name,
                AverageTime = (double)totalTime / repetitions,
                TotalTime = totalTime
            };
        }

        static void PrintResultsTable(List<TestResult> results)
        {
            Console.WriteLine("\nАлгоритм\t\tСреднее время\tОбщее время");
            Console.WriteLine("--------\t\t------------\t-----------");

            foreach (var result in results)
            {
                Console.WriteLine($"{result.AlgorithmName,-20}\t{result.AverageTime,8:F2} мс\t{result.TotalTime,8} мс");
            }
        }

        static int[] GenerateRandomArray(int size)
        {
            var random = new Random();
            int[] array = new int[size];

            for (int i = 0; i < size; i++)
            {
                array[i] = random.Next(1, 10000);
            }

            return array;
        }

        static int[] CopyArray(int[] original)
        {
            int[] copy = new int[original.Length];
            Array.Copy(original, copy, original.Length);
            return copy;
        }

        static bool IsSorted(int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                if (array[i] > array[i + 1])
                    return false;
            }
            return true;
        }
    }
}
