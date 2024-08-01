using MathNet.Numerics.LinearAlgebra;
using MethodOfOptimalSetPartitioning.classes.FunctionalsOfProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodOfOptimalSetPartitioning.classes.MinimizationsAlgoritms.StepMethods
{
    public abstract class AbstractStepMethod
    {
        public int CountIterationForIncreasStep { get; set; } = 3;
        public double StepReductionFactor { get; set; } = 0.9;
        public double StepIncreaseFactor { get; set; } = 1.1;
        public virtual StepData[] CalculateStep(AbstractFunctionalContainer container, Vector<double> vectors, ref double step, Vector<double> direction)
        {
            var stepsCount = 0;
            var dataStep = new List<StepData>();
            do
            {
                ++stepsCount;
                vectors += _stepInDirectional(step,direction);
                dataStep.Add(new StepData(step, vectors, container.CalculateSubgradientFunctionale(vectors)));
                if (stepsCount > CountIterationForIncreasStep)
                    step *= StepIncreaseFactor;
            }
            while (direction * dataStep[stepsCount - 1].Subgradient > 0);
            if (stepsCount == 1)
                step *= StepReductionFactor;
            return dataStep.ToArray();
        }
        protected abstract Vector<double> _stepInDirectional(double Step, Vector<double> direction);
    }
}
