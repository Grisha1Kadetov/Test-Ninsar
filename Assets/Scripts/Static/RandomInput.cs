using UnityEngine;

namespace TestNinsar
{
    public static class InputRandomizer
    {
        public static string GetRandomInput(char[] values, int xSize, int ySize)
        {
            var r = "";
            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    r += values[Random.Range(0, values.Length)];
                }

                r += '\n';
            }

            return r;
        }
    }
}