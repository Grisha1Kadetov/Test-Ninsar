using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestNinsar
{
    //Этого не было в тз, поэтому сделал попроще.
    //Конечно тут можно было применить MV*  паттерн.
    public class CellOfMap : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _selectionImage;
        [SerializeField] private TMP_Text _text;
        
        public void Init(char с, Color color)
        {
            _image.color = color;
            _text.text = с.ToString();
            _selectionImage.enabled = false;
        }

        public void UpdateView(bool isSelected)
        {
            _selectionImage.enabled = isSelected;
        }
    }
}