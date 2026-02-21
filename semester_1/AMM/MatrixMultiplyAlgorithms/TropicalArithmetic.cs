using System;

namespace MatrixAlgorithms
{
    public class TropicalArithmetic : IArithmetic<double>
    {
        public double Multiply(double a, double b) => a + b;
        public double Sum(double a, double b) => Math.Min(a, b);
        public double Zero => double.PositiveInfinity;
        public double One => 0;
    }
}
