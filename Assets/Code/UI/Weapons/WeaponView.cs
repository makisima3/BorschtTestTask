using Code.Player;
using Code.Player.Shooting.Configs;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Weapons
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Button button;

        private WeaponConfig _weaponConfig;
        private WeaponChangerView _weaponChangerView;
        
        public void Init(WeaponConfig config, WeaponChangerView weaponChangerView )
        {
            _weaponConfig = config;
            _weaponChangerView = weaponChangerView;

            icon.sprite = config.Icon;
            
            button.onClick.AddListener(OnCLick);
        }

        private void OnCLick()
        {
            _weaponChangerView.SetWeapon(_weaponConfig.Type);
        }
    }
}