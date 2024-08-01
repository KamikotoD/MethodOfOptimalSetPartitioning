using MathNet.Numerics.LinearAlgebra;
using MethodOfOptimalSetPartitioning.classes.FunctionalsOfProject;
using MethodOfOptimalSetPartitioning.classes.Shapes;
using System.Drawing;

namespace MethodOfOptimalSetPartitioning.classes.Helpers
{
    public enum TypesOfRendering
    {
        ColourRendering,
        BoundaryDrawing,
        ColourDrawingWithBorders
    }
    public enum WritingCenterPoint
    {
        None,
        OnlyPoint,
        OnlyNumber,
        AllWriting
    }
    public class CreateImageOfPartitional
    {
        private Brush[] brushes;

        public TypesOfRendering typesOfRendering { get; set; }
        public WritingCenterPoint writingCenterPoint { get; set; }
        public Brush[] Brushes { get => brushes; set => brushes = value; }
        public CreateImageOfPartitional(int countBrushes, TypesOfRendering typesOfRendering = TypesOfRendering.ColourDrawingWithBorders, WritingCenterPoint writingCenterPoint = WritingCenterPoint.OnlyNumber)
        {
            Brushes = GetBrushes(countBrushes);
            this.typesOfRendering = typesOfRendering;
            this.writingCenterPoint = writingCenterPoint;
        }
        private static Brush[] GetBrushes(int count)
        {
            Random random = new Random();
            var brushes = new Brush[count];
            for (int i = 0; i < count; i++)
            {
                brushes[i] = new SolidBrush(Color.FromArgb(255, random.Next(255), random.Next(255), random.Next(255)));
            }
            return brushes;
        }
        
        public Bitmap GetBitmapOfPartitional(FunctionalContainer container, Vector<double> vector, double size, Vector<double> centers)
        {
            var tempSaveRectangle = container.Rectangle;
            var boundsWidth = container.Rectangle.DimationShape[0].Max - container.Rectangle.DimationShape[0].Min;
            var boundsHeight = container.Rectangle.DimationShape[1].Max - container.Rectangle.DimationShape[1].Min;
            var AreaAspectRatio = boundsHeight / boundsWidth;
            var initialSize = new Size((int)size, (int)(size * AreaAspectRatio));
            var newRectangle = new ShapeRectangle(
                (container.Rectangle.DimationShape[0].Min, container.Rectangle.DimationShape[0].Max),
                (container.Rectangle.DimationShape[1].Min, container.Rectangle.DimationShape[1].Max),
                initialSize.Width, initialSize.Height
                );
            container.Rectangle = newRectangle;
            var bitmap = new Bitmap(initialSize.Width, initialSize.Height);
            var area = container.GetFunctionalPartitional(vector);
            container.Rectangle = tempSaveRectangle;
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {

                Brush blackBrush = new SolidBrush(Color.Black);
                Brush whiteBrush = new SolidBrush(Color.White);
                var ColourRendering = (int x, int y) =>
                {
                    graphics.FillRectangle(Brushes[area[x, y]], x, initialSize.Height - y, 1, 1);
                };
                var BoundaryDrawing = (int x, int y) =>
                {
                    var flag = false;
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int l = -1; l <= 1; l++)
                        {
                            if (x + k >= 0 && y + l >= 0 && x + k < initialSize.Width && y + l < initialSize.Height)
                            {
                                if (area[x, y] != area[x + k, y + l])
                                {
                                    graphics.FillRectangle(blackBrush, x, initialSize.Height - y, 1, 1);
                                }
                            }
                        }
                    }
                };
                for (int i = 0; i < initialSize.Width; ++i)
                {
                    for (int j = initialSize.Height - 1; j > 0; --j)
                    {
                        switch (typesOfRendering)
                        {
                            case TypesOfRendering.BoundaryDrawing:
                                graphics.FillRectangle(whiteBrush, i,j, 1, 1);
                                BoundaryDrawing(i, j);
                                break;
                            case TypesOfRendering.ColourRendering:
                                ColourRendering(i, j);
                                break;
                            case TypesOfRendering.ColourDrawingWithBorders:
                                ColourRendering(i, j);
                                BoundaryDrawing(i, j);
                                break;
                             default:
                                break;

                        }

                    }
                }
                //Draw point
                float PointRadius = (float)(size * 5f) / 1000f;
                float countPoint = centers.Count / 2.0f, k = -20/99f, b = 30+ 20/99f;
                float numberSize = countPoint * k + b;//((float)(size * 30f) / 1000f);
                for (int center = 0; center < centers.Count; center+=2)
                {
                    var x = (float)((centers[center] - newRectangle.DimationShape[0].Min) / (newRectangle.DimationShape[0].Max - newRectangle.DimationShape[0].Min) * initialSize.Width);
                    var y = (float)((newRectangle.DimationShape[1].Max - centers[center + 1]) / (newRectangle.DimationShape[1].Max - newRectangle.DimationShape[1].Min) * initialSize.Height);
                    switch (writingCenterPoint)
                    {
                        case WritingCenterPoint.None: break;
                        case WritingCenterPoint.OnlyPoint:
                            graphics.FillEllipse(System.Drawing.Brushes.Black, x - PointRadius, y - PointRadius, 2 * PointRadius, 2 * PointRadius);
                            break;
                        case WritingCenterPoint.OnlyNumber:
                            graphics.DrawString((center / 2 + 1).ToString(), new Font(FontFamily.GenericMonospace, numberSize), System.Drawing.Brushes.Black, x- numberSize/2, y- numberSize/2);
                            break;
                        case WritingCenterPoint.AllWriting:
                            graphics.FillEllipse(System.Drawing.Brushes.Black, x - PointRadius, y - PointRadius, 2 * PointRadius, 2 * PointRadius);
                            graphics.DrawString((center / 2 + 1).ToString(), new Font(FontFamily.GenericMonospace, numberSize), System.Drawing.Brushes.Black, x + PointRadius * 0.71f, y + PointRadius * 0.71f);
                            break;
                    }
                    
                    
                }

            }
            return bitmap;
        }
        public static void SaveImageUseBitmap(Bitmap bitmap, string name, string directoryPath = @"Fotos")
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryPath);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            string filePath = Path.Combine(fullPath, $"{name}.jpeg");
            bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}
