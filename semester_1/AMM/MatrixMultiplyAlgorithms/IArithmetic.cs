namespace MatrixAlgorithms
{
    public interface IArithmetic<T>
    {
        T Multiply(T a, T b);
        T Sum(T a, T b);
        T Zero { get; }
        T One { get; }
    }
}
