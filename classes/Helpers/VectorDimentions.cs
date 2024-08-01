using MathNet.Numerics.LinearAlgebra;

namespace MethodOfOptimalSetPartitioning.classes.Helpers
{
    public class VectorDimentions
    {
        private const int _dimentions = 2;
        private Vector<double> _vector;
        public Vector<double> Vector { get { return _vector; } }
        public int Count { get; private set; }
        public VectorDimentions(Vector<double> vector)
        {
            _vector = vector;
            Count = _vector.Count/ _dimentions;
        }
        public VectorDimentions(int count)
        {
            _vector = Vector<double>.Build.DenseOfArray(new double[count* _dimentions]);
            Count = _vector.Count/ _dimentions;
        }
        public (double x, double y) this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException("Index out of range.");
                }
                return (_vector[index*_dimentions], _vector[index * _dimentions+1]);
            }
            set
            {
                if (index < 0 || index  >= Count)
                {
                    throw new IndexOutOfRangeException("Index out of range.");
                }
                _vector[index * _dimentions] = value.x;
                _vector[index * _dimentions + 1] = value.y;
            }
        }
    }
}
