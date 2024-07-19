using System;

namespace TestNinsar.Tables
{
    ///<summary>
    /// Тип данных, который представляет массив как бесконечную карту, не позволяет выйти за пределы двухмерного массива 
    /// </summary>
    public class NonBoundariesTable<T>
    {
        protected readonly T[,] Table;
        
        public T this[int i, int j]
        {
            get
            {
                (i, j) = GetNormalizeIndex(i, j);
                return Table[i, j];
            }
            set
            {
                (i, j) = GetNormalizeIndex(i, j); 
                Table[i, j] = value;
            }
        }

        public int LengthX => Table.GetLength(0);
        public int LengthY => Table.GetLength(1);
        
        public NonBoundariesTable(int xSize, int ySize)
        {
            Table = new T[xSize, ySize];
        }

        public NonBoundariesTable(T[,] table) : this(table.GetLength(0), table.GetLength(1))
        {
            for (var i = 0; i < table.GetLength(0); i++)
                for (var j = 0; j < table.GetLength(1); j++)
                    Table[i, j] = table[i, j];
        }
        
        public NonBoundariesTable(NonBoundariesTable<T> table) : this(table.LengthX, table.LengthY)
        {
            for (var i = 0; i < table.LengthX; i++)
                for (var j = 0; j < table.LengthY; j++)
                    Table[i, j] = table[i, j];
        }
        
        public (int, int) GetNormalizeIndex(int i, int j) => (GetNormalizeIndexX(i), GetNormalizeIndexY(j));
        public int GetNormalizeIndexX(int i) => NormalizeBy(i, LengthX);
        public int GetNormalizeIndexY(int j) => NormalizeBy(j, LengthY);
        
        protected int NormalizeBy(int a, int b)
        {
            if (a >= 0) a %= b;
            else a = a % b != 0 ? b - Math.Abs(a) % b : 0;
            return a;
        }
    }
}