using System;
using TestNinsar.Tables;
using UnityEngine;

namespace TestNinsar.Shapes
{
    /// <summary>
    /// Строит сетку из Shape, с определенным размером, центром и поворотом (как у объекат на котором висит скрипт).
    /// </summary>
    public class ShapeGridFactory : MonoBehaviour
    {

        [SerializeField, Min(0)] private float _space;
        [Space] [SerializeField, Min(1)] private int _sizeX;
        [SerializeField, Min(1)] private int _sizeY;
        [Header("Links")] [SerializeField] private Shape _prefab;
        [SerializeField] private Transform _center;
        [SerializeField] private bool _buildAwake;
        private Shape[,] _shapes;
        private Vector3 _start;

        public int SizeX
        {
            get => _sizeX;
            set
            {
                if (value < 1)
                {
                    throw new AggregateException($"{nameof(SizeX)} can't be less then 1. But you tried {value}.");
                }

                _sizeX = value;
            }
        }

        public int SizeY
        {
            get => _sizeY;
            set
            {
                if (value < 1)
                {
                    throw new AggregateException($"{nameof(SizeY)} can't be less then 1. But you tried {value}.");
                }

                _sizeY = value;
            }
        }

        public Vector3 Center => _center.position;

        private void Awake()
        {
            if (_buildAwake)
            {
                Build();
            }
        }

        private void SetStarts()
        {
            _start = Center;
            var xSpaceCount = _sizeX % 2 == 0
                ? _sizeX * 0.5f - 0.5f
                : (_sizeX - 1) * 0.5f;
            var ySpaceCount = _sizeY % 2 == 0
                ? _sizeY * 0.5f - 0.5f
                : (_sizeY - 1) * 0.5f;
            _start -= transform.right * (_space * xSpaceCount);
            _start += transform.forward * (_space * ySpaceCount);

        }

        private Vector3[,] GetPositions()
        {
            var res = new Vector3[_sizeX, _sizeY];
            for (int y = 0; y < _sizeY; y++)
            {
                for (int x = 0; x < _sizeX; x++)
                {
                    var position = _start + transform.right * (_space * x)
                                   - transform.forward * (_space * y);
                    res[x, y] = position;
                }
            }

            return res;
        }

        public Shape[,] Build()
        {
            DestroyAllShapes();
            SetStarts();

            Shape[,] res = new Shape[_sizeX, _sizeY];

            var vectors = GetPositions();
            for (var x = 0; x < vectors.GetLength(0); x++)
            for (var y = 0; y < vectors.GetLength(1); y++)
            {
                var position = vectors[x, y];
                res[x, y] = Instantiate(_prefab, position, transform.rotation, _center);
            }

            Table.CloneTable(res, ref _shapes);

            return res;
        }

        public void DestroyAllShapes()
        {
            if (_shapes == null)
                return;

            foreach (var shape in _shapes)
            {
                Destroy(shape.gameObject);
            }
        }

        void OnDrawGizmosSelected()
        {
            if (_prefab == null || _center == null)
                return;

            SetStarts();

            Gizmos.color = new Color(1, 0, 0, 1f);
            Gizmos.DrawSphere(_start, 0.5f);
            Gizmos.color = new Color(0.5f, 1, 1, 0.5f);
            foreach (var position in GetPositions())
            {
                Gizmos.DrawSphere(position, 0.5f);
            }
        }
    }

}