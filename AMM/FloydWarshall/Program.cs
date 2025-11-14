using MatrixAlgorithms;
using System;

namespace FloydWarshall
{
    internal class Program
    {
        static void Main()
        {
            var tropicalAlg = new FloydWarshallAlgorithm<double>(new TropicalArithmetic());

            var inf = double.PositiveInfinity;

            // Матрица смежности 
            double[,] graph = {
                { 0, 3, inf, 7 },
                { 8, 0, 2, inf },
                { 5, inf, 0, 1 },
                { 2, inf, inf, 0 }
            };

            Console.WriteLine("Исходная матрица смежности:");
            PrintMatrix(graph);

            var shortestPaths = tropicalAlg.FloydWarshall(graph, gridSize: 2);

            Console.WriteLine("\nМатрица кратчайших путей:");
            PrintMatrix(shortestPaths);

            // Проверка корректности через классический алгоритм
            Console.WriteLine("\nКлассический алгоритм Флойда-Уоршелла:");
            var classicResult = ClassicFloydWarshall(graph);
            PrintMatrix(classicResult);

            static void PrintMatrix(double[,] matrix)
            {
                int n = matrix.GetLength(0);

                Console.Write("    ");
                for (int j = 0; j < n; j++)
                {
                    Console.Write($"{j,8}");
                }
                Console.WriteLine();

                Console.Write("    ");
                for (int j = 0; j < n; j++)
                {
                    Console.Write("--------");
                }
                Console.WriteLine();

                for (int i = 0; i < n; i++)
                {
                    Console.Write($"{i,2} |");
                    for (int j = 0; j < n; j++)
                    {
                        if (double.IsPositiveInfinity(matrix[i, j]))
                            Console.Write("     -  ");
                        else
                            Console.Write($"{matrix[i, j],8:F1}");
                    }
                    Console.WriteLine();
                }
            }

            static double[,] ClassicFloydWarshall(double[,] dist)
            {
                int n = dist.GetLength(0);
                double[,] result = (double[,])dist.Clone();

                for (int k = 0; k < n; k++)
                {
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            if (result[i, k] + result[k, j] < result[i, j])
                            {
                                result[i, j] = result[i, k] + result[k, j];
                            }
                        }
                    }
                }

                return result;
            }
        }
    }
}