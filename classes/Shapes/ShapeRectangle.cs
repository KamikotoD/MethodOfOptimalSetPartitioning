using MathNet.Numerics.LinearAlgebra;

namespace MethodOfOptimalSetPartitioning.classes.Shapes
{
    public class ShapeRectangle
    {
        public (int XGrid, int YGrid) Count { get; set; }
        public (double X, double Y) GridStep { get; set; }
        public (double Min, double Max)[] DimationShape { get; set; }
        public ShapeRectangle((double Min, double Max) x, (double Min, double Max) y, int countXGrid, int countYGrid)
        {
            DimationShape = new (double Min, double Max)[2]
            {
                x,
                y
            };
            Count = (countXGrid, countYGrid);
            GridStep = (Math.Abs(x.Max - x.Min) / countXGrid, Math.Abs(y.Max - y.Min) / countYGrid);
            Vector<double>.Build.DenseOfArray(new double[] { });
        }
        public (double x,double y) GetPointOnGridForIndex(int xIndex, int yIndex)
        {
            return ( xIndex * GridStep.X + DimationShape[0].Min + (GridStep.X / 2), yIndex * GridStep.Y + DimationShape[1].Min + (GridStep.Y / 2) );
        }
    }
}
