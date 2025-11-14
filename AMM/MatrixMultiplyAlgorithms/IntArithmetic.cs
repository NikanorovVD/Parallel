
namespace MatrixAlgorithms
{
    public class IntArithmetic: IArithmetic<int>
    {
        public int Multiply(int a, int b) => a * b;
        public int Sum(int a, int b) => a + b;
        public int Zero => 0;
        public int One => 1;
    }
}
