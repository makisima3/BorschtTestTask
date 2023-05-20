
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
        [SerializeField] private GameObject armorHolder;
        [SerializeField] private Image armorImage;
        [SerializeField] private Image armorDeltaImage;

        public void ShowHP(float maxHP, float currentHP)
        {
            var fillAmount = 1f / maxHP * currentHP;
            hpImage.fillAmount = fillAmount;
            hpDeltaImage.DOFillAmount(fillAmount, animTime).SetEase(Ease.Linear);
        }
        
        public void ShowArmor(float maxArmor, float currentArmor)
        {
            if (maxArmor <= 0)
            {
                armorHolder.SetActive(false);
            }
            
            var fillAmount = 1f / maxArmor * currentArmor;
            armorImage.fillAmount = fillAmount;
            armorDeltaImage.DOFillAmount(fillAmount, animTime).SetEase(Ease.Linear);
        }
    }
}