using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestNinsar.ScriptableObjects
{
    [CreateAssetMenu(menuName = "TextToShapeDictionaryDictionary", fileName = "New TextToShapeDictionaryDictionary", order = -1)]

    public class CharToShapeDataDictionary : ScriptableObject
    {
        [SerializeField] private List<CharToShapeDataAssociation> _associations;

        private IReadOnlyDictionary<char, ShapeData> _shapeDictionary;
        public IReadOnlyDictionary<char, ShapeData> ShapeDictionary
        {
            get
            {
                _shapeDictionary ??= _associations.ToDictionary(x => x.Char, x => x.ShapeData);
                return _shapeDictionary;
            }
        }
    }
}