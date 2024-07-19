using System;
using System.Collections.Generic;

namespace TestNinsar.Tables
{
    /// <summary>
    /// Удобный тип для получения сразу группы объектов.
    /// Размер считаеться X влево и Y внизиз от Pivot
    /// Pivot по умалчанию (0, 0)
    /// </summary>
    public class AreaOfNonBoundariesTable<T>
    {
        public NonBoundariesTable<T> Table { get; }
        
        private int _areaSizeX;
        public int AreaSizeX 
        { 
            get => _areaSizeX;
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentException("AreaSizeX can't be less then 1");
                }
                _areaSizeX = value;
            }
        }

        private int _areaSizeY;
        public int AreaSizeY
        {
            get => _areaSizeY;
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentException("AreaSizeY can't be less then 1");
                }
                _areaSizeY = value;
            }
        }

        /// <summary>
        /// Формат (int Х, int Y)
        /// </summary>
        public (int, int) Pivot
        {
            get => (PivotXPosition, PivotYPosition);
            set
            {
                (int x, int y) = value;
                PivotXPosition = x;
                PivotYPosition = y;
            }
        }

        private int _pivotXPosition;
        public int PivotXPosition
        {
            get => _pivotXPosition;
            set => _pivotXPosition = Table.GetNormalizeIndexX(value);
        }

        private int _pivotYPosition;
        public int PivotYPosition
        {
            get => _pivotYPosition;
            set => _pivotYPosition = Table.GetNormalizeIndexY(value);
        }

        public AreaOfNonBoundariesTable(NonBoundariesTable<T> table, int sizeX, int sizeY)
        {
            Table = table;
            AreaSizeX = sizeX;
            AreaSizeY = sizeY;
        }
        
        public AreaOfNonBoundariesTable(NonBoundariesTable<T> table, int sizeX, int sizeY, int pivotX, int pivotY) 
            : this(table, sizeX, sizeY)
        {
            Pivot = (pivotX, pivotY);
        }

        public void SetSize(int sizeX, int sizeY)
        {
            AreaSizeX = sizeX;
            AreaSizeY = sizeY;
        }
        
        public T[,] GetArea(int pivotX, int pivotY)
        {
            Pivot = (pivotX, pivotY);
            return GetArea();
        }
        
        public T[,] GetArea()
        {
            T[,] r = new T[AreaSizeX, AreaSizeY];
            (int x, int y) = Pivot;
            for (int j = 0; j < AreaSizeY; j++)
                for (int i = 0; i < AreaSizeX; i++)
                    r[i, j] = Table[x + i, y + j];
            return r;
        }
        
        public (int, int)[] GetAreaNormalizedIndexes()
        {
            var r = new List<(int, int)>();
            (int x, int y) = Pivot;
            for (int j = 0; j < AreaSizeY; j++)
                for (int i = 0; i < AreaSizeX; i++)
                    r.Add(Table.GetNormalizeIndex(x + i, y + j));
            return r.ToArray();
        }
    }
}