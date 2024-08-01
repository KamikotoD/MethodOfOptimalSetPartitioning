using MathNet.Numerics.LinearAlgebra;
using MethodOfOptimalSetPartitioning.classes.FunctionalsOfProject;
using MethodOfOptimalSetPartitioning.classes.Helpers;
using MethodOfOptimalSetPartitioning.classes.Shapes;
using MethodOfOptimalSetPartitioning.interfaces;

namespace MethodOfOptimalSetPartitioning.classes.Functionals
{
    public class FunctionalTaskA3 : FunctionalContainer
    {
        public (double X, double Y)[] CenterTaus {  get; set; }
        public double[] Limitations { get; set; }
        public bool[] LimitationsBool { get; set; }
        public FunctionalTaskA3(ShapeRectangle rectangle, double[] significanceFactors, bool[] limitationsBool, (double X, double Y)[] centerTaus, double[] limitations, ICalculableMetric calculableMetric, Func<(double x, double y), double> probabilityDensityFunction):base(rectangle, significanceFactors,calculableMetric,probabilityDensityFunction)
        {
            CenterTaus = centerTaus;
            Limitations = limitations;
            LimitationsBool = limitationsBool;
        }
        public override int[,] GetFunctionalPartitional(Vector<double> vector)
        {
            var arrayIndexes = new int[Rectangle.Count.XGrid, Rectangle.Count.YGrid];
            var centerTaus = new VectorDimentions(vector);
            for (int x = 0; x < Rectangle.Count.XGrid; x++)
            {
                for (int y = 0; y < Rectangle.Count.YGrid; y++)
                {
                    var point = Rectangle.GetPointOnGridForIndex(x, y);
                    arrayIndexes[x, y] = _calculateIndexCenterForMinDistance(vector, point);
                }
            }
            return arrayIndexes;
        }
        public override Vector<double> CalculateTheAreaOfSubsets(Vector<double> vector)
        {
            var valueFunctionaleInCenters = new double[vector.Count];
            for (int x = 0; x < Rectangle.Count.XGrid; x++)
            {
                for (int y = 0; y < Rectangle.Count.YGrid; y++)
                {
                    var point = Rectangle.GetPointOnGridForIndex(x, y);
                    int index = _calculateIndexCenterForMinDistance(vector, point);
                    valueFunctionaleInCenters[index] += Rectangle.GridStep.X * Rectangle.GridStep.Y;
                }
            }
            return Vector<double>.Build.DenseOfArray(valueFunctionaleInCenters);
        }
        public override Vector<double> CalculateFunctionaleOfSubsets(Vector<double> vector)
        {
            var valueFunctionaleInCenters = new double[vector.Count];
            for (int x = 0; x < Rectangle.Count.XGrid; x++)
            {
                for (int y = 0; y < Rectangle.Count.YGrid; y++)
                {
                    var point = Rectangle.GetPointOnGridForIndex(x, y);
                    int index = _calculateIndexCenterForMinDistance(vector, point);
                    valueFunctionaleInCenters[index] += _calculateIntegral(vector[index], point, index);
                }
            }
            for (int i = 0; i < valueFunctionaleInCenters.Length; i++)
            {
                valueFunctionaleInCenters[i] -= vector[i] * Limitations[i];
            }
            return Vector<double>.Build.DenseOfArray(valueFunctionaleInCenters);
        }
        public Vector<double> CalculateFunctionaleOfSubsetsTwo(Vector<double> vector)
        {
            var valueFunctionaleInCenters = new double[vector.Count];
            for (int x = 0; x < Rectangle.Count.XGrid; x++)
            {
                for (int y = 0; y < Rectangle.Count.YGrid; y++)
                {
                    var point = Rectangle.GetPointOnGridForIndex(x, y);
                    int index = _calculateIndexCenterForMinDistance(vector, point);
                    valueFunctionaleInCenters[index] += _calculateIntegralTwo(vector[index], point, index);
                }
            }
            return Vector<double>.Build.DenseOfArray(valueFunctionaleInCenters);
        }
        public override double CalculateFunctionale(Vector<double> vector)
        {
            return CalculateFunctionaleOfSubsets(vector).Sum(x => x);
        }
        public double CalculateFunctionaleTwo(Vector<double> vector)
        {
            return CalculateFunctionaleOfSubsetsTwo(vector).Sum(x => x);
        }
        public override Vector<double> CalculateSubgradientFunctionale(Vector<double> vector)
        {
            var areaOfSubset = CalculateTheAreaOfSubsets(vector);
            var sub = new double[vector.Count];
            for (int i = 0; i < vector.Count; i++)
            {
                sub[i] = areaOfSubset[i]-Limitations[i];
                // if <= and ==
                if (LimitationsBool[i])
                {
                    sub[i] += 10000 * Math.Max(0, Math.Sign(-vector[i]));
                }
                
            }
            return Vector<double>.Build.DenseOfArray(sub);
        }
        //private 
        private int _calculateIndexCenterForMinDistance(Vector<double> lambdas, (double x, double y) point)
        {
            int indexMin = 0;
            for (int i = 1; i < lambdas.Count; i++)
            {
                if (_calculationFormulaOfFunction(lambdas[i], point, i) <= _calculationFormulaOfFunction(lambdas[indexMin], point, indexMin))
                {
                    indexMin = i;
                }
            }
            return indexMin;
        }
        private double _calculationFormulaOfFunction(double lambda, (double x, double y) point, int indexTau)
        {
            return (CalculableMetric.CalculeteFunction(CenterTaus[indexTau], point) + SignificanceFactors[indexTau] + lambda) * ProbabilityDensityFunction(point);
        }
        private double _calculateIntegral(double lambda, (double x, double y) point, int indexTau)
        {
            (double x, double y) indent = (Rectangle.GridStep.X/2, Rectangle.GridStep.Y/2);
            Func<double, double, double> fxy = (x, y) => _calculationFormulaOfFunction(lambda, (x, y), indexTau);

            return MathNet.Numerics.Integrate.OnRectangle(fxy, point.x - indent.x, point.x + indent.x, point.y - indent.y, point.y + indent.y, 2);
        }
        private double _calculateIntegralTwo(double lambda, (double x, double y) point, int indexTau)
        {
            (double x, double y) indent = (Rectangle.GridStep.X / 2, Rectangle.GridStep.Y / 2);
            Func<double, double, double> fxy = (x, y) => _calculationFormulaOfFunctionTwo(lambda, (x, y), indexTau);

            return MathNet.Numerics.Integrate.OnRectangle(fxy, point.x - indent.x, point.x + indent.x, point.y - indent.y, point.y + indent.y, 2);
        }
        private double _calculationFormulaOfFunctionTwo(double lambda, (double x, double y) point, int indexTau)
        {
            return (CalculableMetric.CalculeteFunction(CenterTaus[indexTau], point) + SignificanceFactors[indexTau]) * ProbabilityDensityFunction(point);
        }
    }
}
