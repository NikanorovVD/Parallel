using System;
using System.Threading.Tasks;

namespace MatrixAlgorithms
{
    public class MatrixMultiplyAlgorithms<T>
    {
        private readonly IArithmetic<T> arithmetic;

        public MatrixMultiplyAlgorithms(IArithmetic<T> arithmetic)
        {
            this.arithmetic = arithmetic;
        }

        public T[,] SequentialMultiply(T[,] A, T[,] B)
        {
            int rowsA = A.GetLength(0);
            int colsA = A.GetLength(1);
            int colsB = B.GetLength(1);

            if (colsA != B.GetLength(0))
                throw new ArgumentException("Несовместимые размеры матриц для умножения");

            T[,] result = new T[rowsA, colsB];

            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < colsB; j++)
                {
                    T sum = default;
                    for (int k = 0; k < colsA; k++)
                    {
                        sum = arithmetic.Sum( sum,  arithmetic.Multiply( A[i, k] ,B[k, j]));
                    }
                    result[i, j] = sum;
                }
            }

            return result;
        }

        public T[,] RowStripedMultiply(T[,] A, T[,] B, int numThreads)
        {
            int rowsA = A.GetLength(0);
            int colsA = A.GetLength(1);
            int colsB = B.GetLength(1);

            if (colsA != B.GetLength(0))
                throw new ArgumentException("Несовместимые размеры матриц для умножения");

            T[,] result = new T[rowsA, colsB];

            Parallel.For(0, numThreads, new ParallelOptions { MaxDegreeOfParallelism = numThreads }, threadId =>
            {
                int rowsPerThread = rowsA / numThreads;
                int startRow = threadId * rowsPerThread;
                int endRow = (threadId == numThreads - 1) ? rowsA : startRow + rowsPerThread;


                for (int i = startRow; i < endRow; i++)
                {
                    for (int j = 0; j < colsB; j++)
                    {
                        T sum = default;
                        for (int k = 0; k < colsA; k++)
                        {
                            sum = arithmetic.Sum(sum, arithmetic.Multiply(A[i, k], B[k, j]));
                        }
                        result[i, j] = sum;
                    }
                }
            });

            return result;
        }


        public T[,] CannonMultiply(T[,] A, T[,] B, int gridSize)
        {
            int rowsA = A.GetLength(0);
            int colsA = A.GetLength(1);
            int colsB = B.GetLength(1);

            if (colsA != B.GetLength(0))
                throw new ArgumentException("Несовместимые размеры матриц для умножения");

            if (rowsA % gridSize != 0 || colsA % gridSize != 0 || colsB % gridSize != 0)
                throw new ArgumentException("Размеры матриц должны быть кратны размеру решетки");

            int blockSize = rowsA / gridSize;
            T[,] result = InitializeMatrix(rowsA, colsB, arithmetic.Zero);

            Parallel.For(0, gridSize * gridSize, new ParallelOptions { MaxDegreeOfParallelism = gridSize * gridSize }, linearIndex =>
            {
                int i = linearIndex / gridSize;
                int j = linearIndex % gridSize;

                // Инициализируем нулями для обычного умножения
                T[,] localC = InitializeMatrix(blockSize, blockSize, arithmetic.Zero);

                int currentACol = (j + i) % gridSize;
                int currentBRow = (i + j) % gridSize;

                T[,] localA = GetBlock(A, i, currentACol, blockSize, gridSize);
                T[,] localB = GetBlock(B, currentBRow, j, blockSize, gridSize);

                for (int k = 0; k < gridSize; k++)
                {
                    MultiplyBlocks(localA, localB, localC, blockSize);

                    if (k < gridSize - 1)
                    {
                        currentACol = (currentACol + 1) % gridSize;
                        localA = GetBlock(A, i, currentACol, blockSize, gridSize);

                        currentBRow = (currentBRow + 1) % gridSize;
                        localB = GetBlock(B, currentBRow, j, blockSize, gridSize);
                    }
                }

                SetBlock(result, localC, i, j, blockSize, gridSize);
            });

            return result;
        }

        private T[,] InitializeMatrix(int rows, int cols, T value)
        {
            T[,] matrix = new T[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    matrix[i, j] = value;
            return matrix;
        }

        private T[,] GetBlock(T[,] matrix, int blockRow, int blockCol, int blockSize, int gridSize)
        {
            T[,] block = new T[blockSize, blockSize];
            int startRow = blockRow * blockSize;
            int startCol = blockCol * blockSize;

            for (int i = 0; i < blockSize; i++)
                for (int j = 0; j < blockSize; j++)
                    block[i, j] = matrix[startRow + i, startCol + j];

            return block;
        }

        private void SetBlock(T[,] matrix, T[,] block, int blockRow, int blockCol, int blockSize, int gridSize)
        {
            int startRow = blockRow * blockSize;
            int startCol = blockCol * blockSize;

            for (int i = 0; i < blockSize; i++)
                for (int j = 0; j < blockSize; j++)
                    matrix[startRow + i, startCol + j] = block[i, j];
        }

        private void MultiplyBlocks(T[,] A, T[,] B, T[,] C, int blockSize)
        {
            for (int i = 0; i < blockSize; i++)
            {
                for (int j = 0; j < blockSize; j++)
                {
                    T current = C[i, j];
                    for (int k = 0; k < blockSize; k++)
                    {
                        T product = arithmetic.Multiply(A[i, k], B[k, j]);
                        current = arithmetic.Sum(current, product);
                    }
                    C[i, j] = current;
                }
            }
        }
    }
}