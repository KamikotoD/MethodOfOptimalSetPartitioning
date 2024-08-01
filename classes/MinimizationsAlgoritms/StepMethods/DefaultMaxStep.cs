using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodOfOptimalSetPartitioning.classes.MinimizationsAlgoritms.StepMethods
{
    public class DefaultMaxStep : AbstractStepMethod
    {
        protected override Vector<double> _stepInDirectional(double step, Vector<double> subVectors)
        {
            return step * subVectors;
        }
    }
}
