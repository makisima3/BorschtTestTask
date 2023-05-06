
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class HPBar : MonoBehaviour
    {
        [SerializeField] private float animTime = 0.1f;
        [SerializeField] private Image hpImage;
        [SerializeField] private Image hpDeltaImage;

        public void ShowHP(int maxHP, int currentHP)
        {
            var fillAmount = 1f / maxHP * currentHP;
            hpImage.fillAmount = fillAmount;
            hpDeltaImage.DOFillAmount(fillAmount, animTime).SetEase(Ease.Linear);
        }
        

    }
}