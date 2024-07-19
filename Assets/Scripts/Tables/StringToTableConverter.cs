using System;
using System.Collections.Generic;

namespace TestNinsar.Tables
{
    /// <summary>
    /// Конвертирует строку в тип T, с помощью славаря 
    /// </summary>
    /// <typeparam name="TValue">Тип данных, в который переводится строка</typeparam>
    public class StringToTableConverter<TValue> : ToTableConverter<char, TValue>
    {
        public StringToTableConverter(Dictionary<char, TValue> dictionary) : base(dictionary)
        {
        }

        public TValue[,] GetResult(string s)
        {
            var subs = s.Split('\n');
            var res = new TValue[subs[0].Length, subs.Length];
            for (int y = 0; y < subs.Length; y++)
            {
                for (int x = 0; x < subs[0].Length; x++)
                {
                    if(!Dictionary.ContainsKey(subs[y][x]))
                        throw new Exception($"Dictionary doesn't contains {subs[y][x]} key");
                    res[x, y] = Dictionary[subs[y][x]];
                }
            }
            return res;
        }
    }
}