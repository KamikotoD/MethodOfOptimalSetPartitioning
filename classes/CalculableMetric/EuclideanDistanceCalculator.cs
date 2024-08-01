
using MathNet.Numerics.LinearAlgebra;
using MethodOfOptimalSetPartitioning.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodOfOptimalSetPartitioning.classes.CalculableMetric
{
    public class EuclideanDistanceCalculator : ICalculableMetric
    {
        public double CalculeteFunction((double X, double Y) tau, (double X, double Y) point)
        {
            return Math.Sqrt(Math.Pow(tau.X - point.X, 2) + Math.Pow(tau.Y - point.Y, 2));
        }
        public double CalculeteNorm(Vector<double> vector)
        {
            return vector.L2Norm();
        }

    }

}
