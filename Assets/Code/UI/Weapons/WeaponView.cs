using Code.Player;
using Code.Player.Data;
using Code.Player.Enums;
using Code.Player.Shooting.Configs;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.Weapons
{
    public class WeaponView : MonoBehaviour
    {
        [Inject] private PlayerDataHolder playerDataHolder;
        [Inject] private WeaponChangerView weaponChangerView;
        
        [SerializeField] private Image icon;
        [SerializeField] private Button button;

        private WeaponConfig _weaponConfig;
        private WeaponChangerView _weaponChangerView;

        public void Init(WeaponConfig config, WeaponChangerView weaponChangerView )
        {
            _weaponConfig = config;
            _weaponChangerView = weaponChangerView;

            icon.sprite = _weaponConfig.Icon;
            gameObject.SetActive(playerDataHolder.PlayerData.EquipedWeapons.Contains(_weaponConfig.Type));
            
            playerDataHolder.OnEquipedWeaponChanged.AddListener(OnWeaponEquiped);
            button.onClick.AddListener(OnCLick);
        }

        private void OnWeaponEquiped()
        {
            gameObject.SetActive(playerDataHolder.PlayerData.EquipedWeapons.Contains(_weaponConfig.Type));
            weaponChangerView.UnityFix();
        }

        private void OnCLick()
        {
            _weaponChangerView.SetWeapon(_weaponConfig.Type);
        }
    }
}