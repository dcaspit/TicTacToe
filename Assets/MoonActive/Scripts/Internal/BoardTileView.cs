using UnityEngine;
using UnityEngine.UI;

namespace MoonActive.Scripts
{
    public class BoardTileView : MonoBehaviour
    {
        public int Column;
        public int Row;
        
        [SerializeField]
        private Image _image;
        
        public void SetSprite(Sprite sprite)
        {
            var imageColor = _image.color;
            imageColor.a = sprite != null ? 1 : 0;
            _image.color = imageColor;
            
            _image.sprite = sprite;
        }
    }
}