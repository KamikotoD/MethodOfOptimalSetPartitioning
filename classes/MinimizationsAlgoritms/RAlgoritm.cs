using MathNet.Numerics.LinearAlgebra;
using MethodOfOptimalSetPartitioning.classes.FunctionalsOfProject;
using MethodOfOptimalSetPartitioning.classes.MinimizationsAlgoritms.StepMethods;
using System.Diagnostics;

namespace MethodOfOptimalSetPartitioning.classes.MinimizationsAlgoritms
{
    public static class RAlgoritm
    {
        public static double CoefA { get; set; } = 2;
        public static int MaxIteration { get; set; } = 300;
        public static AbstractStepMethod StepMethod { get; set; } = new DefaultMaxStep();
        public static Func<RData, Task> AsyncCallback { get; set; } = (Rdata) => { return Task.CompletedTask; };
        private static Vector<double> SolveAndNormalizeSLAR(Matrix<double> matrix, Vector<double> vector)
        {
            return matrix.Transpose() * vector / (matrix.Transpose() * vector).L2Norm();
        }
        private static Matrix<double> ComputeSpaceTensileMatrix(Matrix<double> eta, int size)
        {
            return Matrix<double>.Build.DenseIdentity(size) + (1 / CoefA - 1) * eta * eta.Transpose();
        }
        public static Vector<double> SolveMinizationTask(AbstractFunctionalContainer container, Vector<double> vector,double startStep = 1, double epsilon = 1e-5)
        {
            Task callbackTask = Task.CompletedTask;
            StepData [] dataStep;
            Stopwatch swMain = new(), swStep = new();
            var vectorValues = vector.Clone();
            var newVectorValues = vectorValues.Clone();
            var subgradient = container.CalculateSubgradientFunctionale(vector);
            var newSubgradient = subgradient.Clone();
            var B = Matrix<double>.Build.DenseIdentity(vector.Count);
            var eta = Vector<double>.Build.Dense(vector.Count, 0);
            var step = startStep;
            int iterationNumber = 0;
            do
            {
                swMain.Restart();
                vectorValues = newVectorValues.Clone();
                subgradient = newSubgradient.Clone();
                if (container.CalculateNormForMetrics(subgradient) < epsilon)
                    break;
                B = B * ComputeSpaceTensileMatrix(eta.ToColumnMatrix(), vector.Count);
                swStep.Restart();
                dataStep = StepMethod.CalculateStep(container, newVectorValues, ref step, B * SolveAndNormalizeSLAR(B, newSubgradient));
                swStep.Stop();
                newVectorValues = dataStep[dataStep.Length - 1].Vector;
                newSubgradient = dataStep[dataStep.Length - 1].Subgradient;
                eta = SolveAndNormalizeSLAR(B, newSubgradient - subgradient);
                swMain.Stop();
                callbackTask = AsyncCallback(new RData(vectorValues, subgradient, dataStep, iterationNumber, swStep.Elapsed, swMain.Elapsed));
                iterationNumber++;
            }
            while (container.CalculateNormForMetrics(newVectorValues-vectorValues) > epsilon && iterationNumber < MaxIteration);
            return newVectorValues;
        }
    }
}
