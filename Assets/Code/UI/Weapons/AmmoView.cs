using Code.Player.Data;
using Code.Player.Shooting;
using TMPro;
using UnityEngine;
using Zenject;

namespace Code.UI.Weapons
{
    public class AmmoView : MonoBehaviour
    {
        [Inject] private ShootController shootController;
        
        [SerializeField] private TMP_Text ammoText;
        [SerializeField] private TMP_Text ammoReserveText;

        private void Start()
        {
            shootController.OnBulletsCountChanged.AddListener(UpdateTexts);
        }

        private void UpdateTexts(WeaponData weaponData)
        {
            ammoText.text = weaponData.Ammo.ToString();
            ammoReserveText.text = weaponData.AmmoReserve.ToString();
        }
        
    }
}