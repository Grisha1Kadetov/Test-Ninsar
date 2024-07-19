using System;
using UnityEngine;

namespace TestNinsar.ScriptableObjects
{
    [Serializable]
    public class CharToShapeDataAssociation
    {
        [SerializeField] private char _char;
        [SerializeField] private ShapeData _shape;

        public char Char => _char;

        public ShapeData ShapeData => _shape;
    }
}