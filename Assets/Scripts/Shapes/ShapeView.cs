using UnityEngine;

namespace TestNinsar.Shapes
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ShapeView : MonoBehaviour
    {
        [SerializeField] private Shape _shape;

        private MeshRenderer _renderer;
        
        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
        }

        private void OnEnable()
        {
            if (_shape.IsInited)
            {
                UpdateView(_shape);
            }
            _shape.ShapeChangedEvent += UpdateView;
        }
        
        private void OnDisable()
        {
            _shape.ShapeChangedEvent -= UpdateView;
        }

        public void UpdateView(Shape shape)
        {
            _renderer.material.color = shape.ShapeData.Color;
        }
    }
}