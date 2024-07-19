using System;
using TestNinsar.ScriptableObjects;
using UnityEngine;

namespace TestNinsar.Shapes
{
    public class Shape : MonoBehaviour
    {
        [SerializeField] private ShapeData _shapeData;

        public ShapeData ShapeData
        {
            get => _shapeData;
            set
            {
                _shapeData = value; 
                ShapeChangedEvent?.Invoke(this);
            }
        }

        public bool IsInited => _shapeData != null;

        public event Action<Shape> ShapeChangedEvent;
        
        //Я практикую создание методов Init для начала работы объекта
        //Хоть сейчас это выглядит и странно тк класс слишком простой.
        public void Init(ShapeData shapeData)
        {
            ShapeData = shapeData;
        }
    }
}