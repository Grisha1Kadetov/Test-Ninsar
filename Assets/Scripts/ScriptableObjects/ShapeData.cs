using UnityEngine;

namespace TestNinsar.ScriptableObjects
{
    [CreateAssetMenu(menuName = "ShapeData", fileName = "New ShapeData", order = -1)]
    public class ShapeData : ScriptableObject
    {
        [SerializeField, ColorUsageAttribute(false)] private Color _color;
        
        public Color Color 
        {
            get
            {
                var c = _color;
                c.a = 1f;
                return c;
            }
        }
    }
}