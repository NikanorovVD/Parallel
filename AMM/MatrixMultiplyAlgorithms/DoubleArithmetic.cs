namespace MatrixAlgorithms
{
    public class DoubleArithmetic : IArithmetic<double>
    {
        public double Multiply(double a, double b) => a * b;
        public double Sum(double a, double b) => a + b;
        public double Zero => 0;
        public double One => 1;
    }
}
