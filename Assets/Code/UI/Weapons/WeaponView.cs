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
            gameObject.SetActive(playerDataHolder.PlayerData.IsWeaponUnlocked(_weaponConfig.Type));
            
            playerDataHolder.OnWeaponUnlocked.AddListener(OnWeaponUnlocked);
            button.onClick.AddListener(OnCLick);
        }

        private void OnWeaponUnlocked(WeaponType type)
        {
            if (type != _weaponConfig.Type)
                return;
            
            gameObject.SetActive(true);
            weaponChangerView.UnityFix();
        }

        private void OnCLick()
        {
            _weaponChangerView.SetWeapon(_weaponConfig.Type);
        }
    }
}