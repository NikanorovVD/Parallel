using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Sort
{
    public static class SortingAlgorithms
    {
        public static void BubbleSort<T>(T[] array) where T: IComparable<T>
        {
            int n = array.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (array[j].CompareTo(array[j + 1]) > 0)
                    {
                        (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    }
                }
            }
        }

        public static void OddEvenSort<T>(T[] array, int maxDegreeOfParallelism) where T : IComparable<T>
        {
            if (array == null || array.Length < 2)
                return;

            var options = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };
            bool sorted;
            int n = array.Length;


            int chunkSize = n / (maxDegreeOfParallelism * 4);

            do
            {
                sorted = true;

                // Четная фаза
                var evenPartitioner = Partitioner.Create(0, n / 2, chunkSize);
                Parallel.ForEach(evenPartitioner, options, range =>
                {
                    bool localSorted = true;
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        int index = i * 2;
                        if (index < n - 1 && array[index].CompareTo(array[index + 1]) > 0)
                        {
                            (array[index], array[index + 1]) = (array[index + 1], array[index]);
                            localSorted = false;
                        }
                    }
                    if (!localSorted) sorted = false;
                });

                // Нечетная фаза
                var oddPartitioner = Partitioner.Create(0, (n - 1) / 2, chunkSize);
                Parallel.ForEach(oddPartitioner, options, range =>
                {
                    bool localSorted = true;
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        int index = i * 2 + 1;
                        if (index < n - 1 && array[index].CompareTo(array[index + 1]) > 0)
                        {
                            (array[index], array[index + 1]) = (array[index + 1], array[index]);
                            localSorted = false;
                        }
                    }
                    if (!localSorted) sorted = false;
                });

            } while (!sorted);
        }
    }   
}
