using MathNet.Numerics.LinearAlgebra;
using MethodOfOptimalSetPartitioning.classes.Helpers;
using MethodOfOptimalSetPartitioning.classes.Shapes;
using MethodOfOptimalSetPartitioning.interfaces;

namespace MethodOfOptimalSetPartitioning.classes.FunctionalsOfProject
{
    public class FunctionalContainer : AbstractFunctionalContainer
    {
        public ShapeRectangle Rectangle { get; set; }
        public double[] SignificanceFactors { get; set; }
        public ICalculableMetric CalculableMetric { get; set; }
        public Func<(double x, double y), double> ProbabilityDensityFunction { get; set; }
        public FunctionalContainer(ShapeRectangle rectangle, double[] significanceFactors, ICalculableMetric calculableMetric, Func<(double x, double y), double> probabilityDensityFunction)
        {
            Rectangle = rectangle;
            SignificanceFactors = significanceFactors;
            CalculableMetric = calculableMetric;
            ProbabilityDensityFunction = probabilityDensityFunction;
        }
        public virtual int[,] GetFunctionalPartitional(Vector<double> vector)
        {
            var arrayIndexes = new int[Rectangle.Count.XGrid, Rectangle.Count.YGrid];
            var centerTaus = new VectorDimentions(vector);
            for (int x = 0; x < Rectangle.Count.XGrid; x++)
            {
                for (int y = 0; y < Rectangle.Count.YGrid; y++)
                {
                    var point = Rectangle.GetPointOnGridForIndex(x, y);
                    arrayIndexes[x,y] = _calculateIndexCenterForMinDistance(centerTaus, point);
                }
            }
            return arrayIndexes;
        }
        public virtual Vector<double> CalculateTheAreaOfSubsets(Vector<double> vector)
        {
            var centerTaus = new VectorDimentions(vector);
            var valueFunctionaleInCenters = new double[centerTaus.Count];
            for (int x = 0; x < Rectangle.Count.XGrid; x++)
            {
                for (int y = 0; y < Rectangle.Count.YGrid; y++)
                {
                    var point = Rectangle.GetPointOnGridForIndex(x, y);
                    int index = _calculateIndexCenterForMinDistance(centerTaus, point);
                    valueFunctionaleInCenters[index] += Rectangle.GridStep.X* Rectangle.GridStep.Y;
                }
            }
            return Vector<double>.Build.DenseOfArray(valueFunctionaleInCenters);
        }
        public virtual Vector<double> CalculateFunctionaleOfSubsets(Vector<double> vector)
        {
            var centerTaus = new VectorDimentions(vector);
            var valueFunctionaleInCenters = new double[centerTaus.Count];
            for (int x = 0; x < Rectangle.Count.XGrid; x++)
            {
                for (int y = 0; y < Rectangle.Count.YGrid; y++)
                {
                    var point = Rectangle.GetPointOnGridForIndex(x, y);
                    int index = _calculateIndexCenterForMinDistance(centerTaus, point);
                    valueFunctionaleInCenters[index] += _calculateIntegral(centerTaus[index], point, index);
                }
            }
            return Vector<double>.Build.DenseOfArray(valueFunctionaleInCenters);
        }
        public override double CalculateFunctionale(Vector<double> vector)
        {
            return CalculateFunctionaleOfSubsets(vector).Sum(x => x);
        }

        public override Vector<double> CalculateSubgradientFunctionale(Vector<double> vector)
        {
            var centerTaus = new VectorDimentions(vector);
            var sub_vector = new VectorDimentions(centerTaus.Count);
            for (int x = 0; x < Rectangle.Count.XGrid; x++)
            {
                for (int y = 0; y < Rectangle.Count.YGrid; y++)
                {
                    var point = Rectangle.GetPointOnGridForIndex(x, y);
                    int index = _calculateIndexCenterForMinDistance(centerTaus, point);
                    var tempValue = _calculateSubgradient(centerTaus[index], point);
                    sub_vector[index] = (sub_vector[index].x + tempValue.x, sub_vector[index].y + tempValue.y);
                }
            }
            for (int i = 0; i < sub_vector.Count; i++)
            {
                var tempValue = _giveAFine(centerTaus[i]);
                sub_vector[i] = (sub_vector[i].x + tempValue.x, sub_vector[i].y + tempValue.y);
            }
            return sub_vector.Vector;
        }
        //private 
        private int _calculateIndexCenterForMinDistance(VectorDimentions centerTaus, (double x, double y) point)
        {
            int indexMin = 0;
            for (int i = 1; i < centerTaus.Count; i++)
            {
                if (_calculationFormulaOfFunction(centerTaus[i],point,i) <= _calculationFormulaOfFunction(centerTaus[indexMin], point, indexMin))
                {
                    indexMin = i;
                }
            }
            return indexMin;
        }
        private double _calculationFormulaOfFunction((double x, double y) centerTau, (double x, double y) point, int indexTau)
        {
            return (CalculableMetric.CalculeteFunction(centerTau, point) + SignificanceFactors[indexTau]) * ProbabilityDensityFunction(point);
        }
        private (double x, double y) _calculateSubgradient((double x, double y) tau, (double x, double y) point)
        {
            var normVector = CalculableMetric.CalculeteFunction(tau, point);
            var gridArea = Rectangle.GridStep.X * Rectangle.GridStep.Y;
            return ((tau.x-point.x) * gridArea / normVector, (tau.y - point.y) * gridArea / normVector);
        }
        private (double x, double y) _giveAFine((double x, double y) tau)
        {
            double x = 0, y = 0;
            x -= 10000 * Math.Max(0, Math.Sign(Rectangle.DimationShape[0].Min - tau.x));
            x += 10000 * Math.Max(0, Math.Sign(tau.x - Rectangle.DimationShape[0].Max));

            y -= 10000 * Math.Max(0, Math.Sign(Rectangle.DimationShape[1].Min - tau.y));
            y += 10000 * Math.Max(0, Math.Sign(tau.y - Rectangle.DimationShape[1].Max));
            return (x, y);
        }
        private double _calculateIntegral((double x, double y) tau, (double x, double y) point, int indexTau)
        {
            (double x, double y) indent = (Rectangle.GridStep.X/2, Rectangle.GridStep.Y/2);
            Func<double, double, double> fxy = (x, y) => _calculationFormulaOfFunction(tau, (x,y),indexTau);
            return MathNet.Numerics.Integrate.OnRectangle(fxy, point.x - indent.x, point.x + indent.x, point.y - indent.y, point.y + indent.y, 2);
        }

        public override double CalculateNormForMetrics(Vector<double> vector)
        {
            return CalculableMetric.CalculeteNorm(vector);
        }
    }
    /*
     public class NewFunctionalContainer
{
    public ShapeRectangle rectangle {  get; set; }
    public double[] significance_factors { get; set; }
    public NewFunctionalContainer(ShapeRectangle rectangle)
    {
        this.rectangle = rectangle;
    }
    public double CalculateFunctional(Vector<double> vectors)
    {
        double sum = 0;
        significance_factors = new double[vectors.Count/2];
        for (int x = 0; x < rectangle.Count.XGrid; x++)
        {
            for (int y = 0; y < rectangle.Count.YGrid; y++)
            {
                var point = rectangle.GetPointOnGridForIndex(x, y);
                int index = MinDistation(point, vectors, significance_factors);
                sum += CalculationFormulaOfFunction((vectors[index], vectors[index + 1]), (point[0], point[1]), significance_factors[index]) * (rectangle.GridStep.X * rectangle.GridStep.Y);
            }
        }
        return 0;
    }
    public Vector<double> CalculateSubgradientOfFunctional(Vector<double> vectors)
    {
        significance_factors = new double[vectors.Count];
        var sub_vector = new double[vectors.Count];
        for (int x = 0; x < rectangle.Count.XGrid; x++)
        {
            for (int y = 0; y < rectangle.Count.YGrid; y++)
            {
                var point = rectangle.GetPointOnGridForIndex(x, y);
                int index = MinDistation(vectors, point, significance_factors);
                var sub = _calculeteIntegral((vectors[index], vectors[index+1]), (point[0], point[1]));
                sub_vector[index] += sub.Item1;
                sub_vector[index+1] += sub.Item2;
            }
        }
        for (int i = 0; i <= vectors.Count / 2; i += 2)
        {
            sub_vector[i] -= 10000 * Math.Max(0, Math.Sign(rectangle.DimationShape[0].Min - vectors[i]));
            sub_vector[i] += 10000 * Math.Max(0, Math.Sign(vectors[i] - rectangle.DimationShape[0].Max));

            sub_vector[i + 1] -= 10000 * Math.Max(0, Math.Sign(rectangle.DimationShape[1].Min - vectors[i+1]));
            sub_vector[i + 1] += 10000 * Math.Max(0, Math.Sign(vectors[i+1] - rectangle.DimationShape[1].Max));
        }
        return Vector<double>.Build.DenseOfArray(sub_vector);

    }
    private (double,double) _calculeteIntegral((double x, double y) tau, (double x, double y) point)
    {
        var sq = rectangle.GridStep.X * rectangle.GridStep.Y;
        var norm = _Norm(( tau.x - point.x,  tau.y - point.y));
        return (norm.x * sq, norm.y * sq);
    }
    private (double x,double y) _Norm((double x, double y) tau)
    {
        var norm_value = Math.Sqrt(tau.x * tau.x + tau.y * tau.y);
        return (tau.x / norm_value, tau.y / norm_value);
    }
    private double CalculationFormulaOfFunction((double x,double y) tau, (double x, double y) point, double significance_factor) 
    {
        return Math.Sqrt(Math.Pow(point.x - tau.x, 2) + Math.Pow(point.y - tau.y, 2)) + significance_factor;//p(x,y)!!!
    }
    public int MinDistation(Vector<double> taus, Vector<double> point, double[] significance_factors)
    {
        int min_index = 0;
        for (int i = 2; i < taus.Count; i+=2)
        {
            if (CalculationFormulaOfFunction((taus[i], taus[i+1]), (point[0], point[1]), significance_factors[i]) <= CalculationFormulaOfFunction((taus[min_index], taus[min_index + 1]), (point[0], point[1]), significance_factors[min_index]))
            {
                min_index = i;
            }
        }
        return min_index;
    }
     */
}
