using MathNet.Numerics.LinearAlgebra;

namespace MethodOfOptimalSetPartitioning.classes.MinimizationsAlgoritms
{
    public class StepData
    {
        public double Step { get; }
        public Vector<double> Vector { get; }
        public Vector<double> Subgradient { get; }
        public StepData(double step, Vector<double> vector, Vector<double> subgradient)
        {
            Step = step;
            Vector = vector.Clone();
            Subgradient = subgradient.Clone();
        }
    }
    public class RData
    {
        public int Iteration { get; }
        public Vector<double> Vector { get; }
        public Vector<double> Subgradient{ get; }
        public StepData[] Step { get; }
        public TimeSpan StepTotal { get; }
        public TimeSpan MethodTotals { get; }

        public RData(Vector<double> vectors, Vector<double> subgradients, StepData[] step, int iteration, TimeSpan stepTotal, TimeSpan methodTotals)
        {
            Vector = (vectors).Clone();
            Subgradient = (subgradients).Clone();
            Step = step;
            Iteration = iteration;
            StepTotal = stepTotal;
            MethodTotals = methodTotals;
        }
    }
}
