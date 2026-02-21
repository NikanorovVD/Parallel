using MatrixAlgorithms;
using System;

namespace MatrixMultTest
{
    internal class Program
    {
        static void Main()
        {
            var random = new Random();

            Console.WriteLine("\n--- УМНОЖЕНИЕ INT МАТРИЦ 4x4 ---");
            var intAlg = new MatrixMultiplyAlgorithms<int>(new IntArithmetic());

            // Генерируем рандомные int матрицы 4x4
            int[,] intA = GenerateRandomIntMatrix(4, 4, random);
            int[,] intB = GenerateRandomIntMatrix(4, 4, random);

            Console.WriteLine("Матрица A:");
            PrintIntMatrix(intA);
            Console.WriteLine("\nМатрица B:");
            PrintIntMatrix(intB);

            Console.WriteLine("\nРезультат A x B последовательно:");
            var intResult = intAlg.SequentialMultiply(intA, intB);
            PrintIntMatrix(intResult);

            Console.WriteLine("\nРезультат A x B Кэннон:");
            var cannonResult = intAlg.CannonMultiply(intA, intB, 2);
            PrintIntMatrix(cannonResult);

            Console.WriteLine("\nРезультат A x B ленточный:");
            var rowWiseResult = intAlg.RowStripedMultiply(intA, intB, 4);
            PrintIntMatrix(rowWiseResult);

            Console.WriteLine("\n--- УМНОЖЕНИЕ BOOL МАТРИЦ 4x4 ---");
            var boolAlg = new MatrixMultiplyAlgorithms<bool>(new BoolArithmetic());

            bool[,] boolA = GenerateRandomBoolMatrix(4, 4, random);
            bool[,] boolB = GenerateRandomBoolMatrix(4, 4, random);

            Console.WriteLine("Матрица A:");
            PrintBoolMatrix(boolA);
            Console.WriteLine("\nМатрица B:");
            PrintBoolMatrix(boolB);

            Console.WriteLine("\nРезультат A x B последовательно:");
            var boolResultSeq = boolAlg.SequentialMultiply(boolA, boolB);
            PrintBoolMatrix(boolResultSeq);

            Console.WriteLine("\nРезультат A x B Кэннон:");
            var boolResultCannon = boolAlg.CannonMultiply(boolA, boolB, 2);
            PrintBoolMatrix(boolResultCannon);

            Console.WriteLine("\nРезультат A x B ленточный:");
            var boolResultRowWise = boolAlg.RowStripedMultiply(boolA, boolB, 4);
            PrintBoolMatrix(boolResultRowWise);
        }


        static int[,] GenerateRandomIntMatrix(int rows, int cols, Random random)
        {
            int[,] matrix = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = random.Next(1, 10); // числа от 1 до 9
                }
            }
            return matrix;
        }

        static bool[,] GenerateRandomBoolMatrix(int rows, int cols, Random random)
        {
            bool[,] matrix = new bool[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = random.Next(2) == 1; // 50% вероятность true/false
                }
            }
            return matrix;
        }

        static void PrintIntMatrix(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write($"{matrix[i, j],6}");
                }
                Console.WriteLine();
            }
        }

        static void PrintBoolMatrix(bool[,] matrix)
        {
            int n = matrix.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write($"{(matrix[i, j] ? "1" : "0"),2}");
                }
                Console.WriteLine();
            }
        }
    }
}
