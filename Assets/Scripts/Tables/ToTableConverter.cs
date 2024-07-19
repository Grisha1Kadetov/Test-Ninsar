using System;
using System.Collections.Generic;

namespace TestNinsar.Tables
{
    /// <summary>
    /// Переводит двухмерный массив TKey в двухмерный массив TValue
    /// </summary>
    /// <typeparam name="TKey">Ключ Dictionary</typeparam>
    /// <typeparam name="TValue">Значение Dictionary</typeparam>
    public class ToTableConverter<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _dictionary;

        public IReadOnlyDictionary<TKey, TValue> Dictionary => _dictionary;

        public ToTableConverter(Dictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
        }

        public virtual TValue[,] GetResult(TKey[,] s)
        {
            var res = new TValue[s.GetLength(0), s.GetLength(1)];
            for(int y = 0; y < s.GetLength(1); y++)
            {
                for (int x = 0; x < s.GetLength(0); x++)
                {
                    if(!Dictionary.ContainsKey(s[x, y]))
                        throw new Exception($"Dictionary doesn't contains {s[x, y]} key");
                    res[x, y] = Dictionary[s[x, y]];
                }
            }
            return res;
        }
    }
}