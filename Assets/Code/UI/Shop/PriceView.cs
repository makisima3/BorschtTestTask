using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Shop
{
    public class PriceView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text countPlace;

        public void Init(Sprite sprite, int count)
        {
            icon.sprite = sprite;
            countPlace.text = $"X{count}";
        }
    }
}