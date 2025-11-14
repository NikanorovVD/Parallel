namespace MatrixAlgorithms
{
    public class FloydWarshallAlgorithm<T>
    {
        private readonly MatrixMultiplyAlgorithms<T> matrixAlgorithms;
        private readonly IArithmetic<T> arithmetic;

        public FloydWarshallAlgorithm(IArithmetic<T> arithmetic)
        {
            this.arithmetic = arithmetic;
            this.matrixAlgorithms = new MatrixMultiplyAlgorithms<T>(arithmetic);
        }

        public T[,] FloydWarshall(T[,] adjacencyMatrix, int gridSize = 2)
        {
            int n = adjacencyMatrix.GetLength(0);

            T[,] result = CreateIdentityMatrix(n);
            T[,] currentPower = adjacencyMatrix;


            int power = n - 1; 
            while (power > 0)
            {
                if ((power & 1) == 1)
                {
                    result = matrixAlgorithms.CannonMultiply(result, currentPower, gridSize);
                }
                currentPower = matrixAlgorithms.CannonMultiply(currentPower, currentPower, gridSize);
                power >>= 1;
            }

            return result;
        }

        private T[,] CreateIdentityMatrix(int n)
        {
            T[,] identity = new T[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    identity[i, j] = (i == j) ? arithmetic.One : arithmetic.Zero;
                }
            }
            return identity;
        }
    }
}
