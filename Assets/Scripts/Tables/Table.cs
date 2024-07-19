namespace TestNinsar.Tables
{
    public static class Table
    {
        public static void CloneTable<T> (T[,] from, ref T[,]to)
        {
            to = new T[from.GetLength(0), from.GetLength(1)];
            for (int i = 0; i < from.GetLength(0); i++)
            {
                for (int j = 0; j < from.GetLength(1); j++)
                {
                    to[i, j] = from[i, j];
                }  
            }
        }
    }
}