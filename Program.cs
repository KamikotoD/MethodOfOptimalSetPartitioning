using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MethodOfOptimalSetPartitioning.classes.CalculableMetric;
using MethodOfOptimalSetPartitioning.classes.Functionals;
using MethodOfOptimalSetPartitioning.classes.Helpers;
using MethodOfOptimalSetPartitioning.classes.MinimizationsAlgoritms;
using MethodOfOptimalSetPartitioning.classes.Shapes;

namespace MethodOfOptimalSetPartitioning
{
    public class Program
    {
        static void Main(string[] args)
        {

            var createImage = new CreateImageOfPartitional(100);

            //Task 2

            //var rectangle = new ShapeRectangle((0, 1), (0, 1), 100, 100);
            //var points = new double[50];
            //{
            //    0.2,0.2, 0.3,0.1, 0.4,0.9, 0.1,0.0, 0.5,0.7,  0.2,0.2, 0.3,0, 0,0, 0,0, 0,0,  0.2,0.2, 0.3,0.1, 0.4,0.9, 0.1,0.0, 0.5,0.7, 0.5, 0.6
            //};
            //var vector = Vector<double>.Build.DenseOfArray(points);
            //var functional = new FunctionalContainer(rectangle, new double[vector.Count / 2], new EuclideanDistanceCalculator(), (points) => 1);
            //var res = RAlgoritm.SolveMinizationTask(functional, vector);

            //Task 3

            var rectangle = new ShapeRectangle((0, 1), (0, 1), 30, 30);
            var points = new (double, double)[] { 
                (0.25, 0.25), (0.25, 0.75), (0.75, 0.25), (0.75, 0.75)};
            var a = new double[points.Length];
            var bBool = new bool[] { true, false, false, true };
            var b = new double[] { 0.1, 0.3, 0.3, 0.4 };
            var lambda = new double[points.Length];
            var vector = Vector<double>.Build.DenseOfArray(lambda);
            var functional = new FunctionalTaskA3(
                rectangle,
                a,
                bBool,
                points,
                b,
                new EuclideanDistanceCalculator(),
                (p) => 1
                );
            var res = RAlgoritm.SolveMinizationTask(functional, vector);

            Console.WriteLine($"F One{functional.CalculateFunctionale(res)}");
            Console.WriteLine($"F Two{functional.CalculateFunctionaleTwo(res)}"); 
            Console.WriteLine($"F One {functional.CalculateFunctionaleOfSubsets(res)}");
            Console.WriteLine($"F Two {functional.CalculateFunctionaleOfSubsetsTwo(res)}");
            Console.WriteLine($"Sq {functional.CalculateTheAreaOfSubsets(res)}");
            Console.WriteLine($"Point {res}");
            CreateImageOfPartitional.SaveImageUseBitmap(createImage.GetBitmapOfPartitional(functional, res, 1000, GetVector(points)), "MA3");
        }
        private static Vector<double> GetVector((double x,double y)[] vectors)
        {
            var vector = new List<double>();
            for (int i = 0; i < vectors.Length; i++)
            {
                vector.Add(vectors[i].x);
                vector.Add(vectors[i].y);
            }
            return Vector<double>.Build.DenseOfArray(vector.ToArray());
        }
    }
}