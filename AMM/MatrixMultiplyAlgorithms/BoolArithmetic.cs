namespace MatrixAlgorithms
{
    public class BoolArithmetic : IArithmetic<bool>
    {
        public bool Multiply(bool a, bool b) => a & b;
        public bool Sum(bool a, bool b) => a | b;
        public bool Zero => false;
        public bool One => true;
    }
}
