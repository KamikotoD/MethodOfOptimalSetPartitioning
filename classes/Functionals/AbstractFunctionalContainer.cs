using MathNet.Numerics.LinearAlgebra;

namespace MethodOfOptimalSetPartitioning.classes.FunctionalsOfProject
{
    public abstract class AbstractFunctionalContainer
    {
        public abstract double CalculateNormForMetrics(Vector<double> vector);
        public abstract double CalculateFunctionale(Vector<double> vector);
        public abstract Vector<double> CalculateSubgradientFunctionale(Vector<double> vector);
    }
}
