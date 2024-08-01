
using MathNet.Numerics.LinearAlgebra;
using MethodOfOptimalSetPartitioning.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodOfOptimalSetPartitioning.classes.CalculableMetric
{
    public class ManhattanDistanceCalculator : ICalculableMetric
    {
        public double CalculeteFunction((double X, double Y) tau, (double X, double Y) point)
        {
            double deltaX = Math.Abs(tau.X - point.X);
            double deltaY = Math.Abs(tau.Y - point.Y);
            return deltaX + deltaY;
        }
        public double CalculeteNorm(Vector<double> vector)
        {
            return vector.L1Norm();
        }

    }
}
