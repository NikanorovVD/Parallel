using MatrixAlgorithms;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MatrixMultiply
{
    class MatrixTest
    {
        static void Main()
        {
            int[] matrixSizes = {256};
            int[] gridSizes = { 2, 4, 8 }; 
            int numberOfRuns = 1;

            foreach (int size in matrixSizes)
            {
                Console.WriteLine($"\n=== РАЗМЕР МАТРИЦЫ: {size} x {size} (прогонов: {numberOfRuns}) ===");
                TestWithMatrixSize(size, gridSizes, numberOfRuns);
                Console.WriteLine("=" + new string('=', 40));
            }
        }

        static void TestWithMatrixSize(int size, int[] gridSizes, int numberOfRuns)
        {
            double[,] matrixA = GenerateRandomMatrix(size, size);
            double[,] matrixB = GenerateRandomMatrix(size, size);

            var results = new List<TestResult>();
            MatrixMultiplyAlgorithms<double> matrixAlgorithms = new(new DoubleArithmetic());


            results.Add(TestAlgorithm("Последовательный", matrixA, matrixB, matrixAlgorithms.SequentialMultiply, numberOfRuns));

            results.Add(TestAlgorithm("Ленточный (4 потока)", matrixA, matrixB,
                (A, B) => matrixAlgorithms.RowStripedMultiply(A, B, 4), numberOfRuns));

            foreach (int gridSize in gridSizes)
            {
                if (size % gridSize == 0) 
                {
                    results.Add(TestAlgorithm($"Кэннон ({gridSize}x{gridSize})", matrixA, matrixB,
                        (A, B) => matrixAlgorithms.CannonMultiply(A, B, gridSize), numberOfRuns));
                }
            }


            PrintResultsTable(results, numberOfRuns);
            ValidateResults(matrixA, matrixB, results);
        }

        static TestResult TestAlgorithm(string name, double[,] A, double[,] B, Func<double[,], double[,], double[,]> multiplyFunction, int numberOfRuns)
        {
            var stopwatch = new Stopwatch();
            double[,] result = null;

            stopwatch.Start();
            for (int i = 0; i < numberOfRuns; i++)
            {
                result = multiplyFunction(A, B);
            }
            stopwatch.Stop();
            long totalTime = stopwatch.ElapsedMilliseconds;

            return new TestResult
            {
                AlgorithmName = name,
                TotalTime = totalTime,
                ResultMatrix = result
            };
        }

        static void PrintResultsTable(List<TestResult> results, int numberOfRuns)
        {
            Console.WriteLine("\nАлгоритм\t\t\tСуммарное время");
            Console.WriteLine("--------\t\t\t--------------");

            foreach (var result in results)
            {
                Console.WriteLine($"{result.AlgorithmName,-25}\t{result.TotalTime,8} мс");
            }
        }

        static double[,] GenerateRandomMatrix(int rows, int cols)
        {
            var random = new Random();
            double[,] matrix = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = random.NextDouble() * 2 - 1;
                }
            }

            return matrix;
        }

        static void ValidateResults(double[,] A, double[,] B, List<TestResult> results)
        {
            double[,] reference = results[0].ResultMatrix;

            for (int i = 1; i < results.Count; i++)
            {
                if (!CheckMatricesEqual(reference, results[i].ResultMatrix, tolerance: 1e-10))
                {
                    Console.WriteLine($"Ошибка: алгоритм {results[i].AlgorithmName} дал некорректный результат");
                }
            }
        }

        static bool CheckMatricesEqual(double[,] A, double[,] B, double tolerance)
        {
            if (A.GetLength(0) != B.GetLength(0) || A.GetLength(1) != B.GetLength(1))
                return false;

            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    if (Math.Abs(A[i, j] - B[i, j]) > tolerance)
                        return false;
                }
            }

            return true;
        }
    }
}