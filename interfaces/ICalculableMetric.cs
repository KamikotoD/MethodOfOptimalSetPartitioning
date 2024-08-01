
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodOfOptimalSetPartitioning.interfaces
{
    public interface ICalculableMetric
    {
        public double CalculeteFunction((double X, double Y) tau, (double X, double Y) point);
        public double CalculeteNorm(Vector<double> vector);
    }
}
