using Code.Player.Configs;
using Code.Player.Enums;
using Code.StorageObjects;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Code.Player.Data
{
    public class PlayerDataHolder : MonoBehaviour
    {
        [Inject] private PlayerDataConfig playerDataConfig;

        private PlayerStorageObject _playerStorageObject;

        public PlayerData PlayerData => _playerStorageObject.Data;
        public UnityEvent<WeaponType> OnWeaponChanged { get; private set; }
        public UnityEvent OnMaxHpChanged { get; private set; }
        public UnityEvent OnMaxArmorChanged { get; private set; }

        public void Awake()
        {
            _playerStorageObject = new PlayerStorageObject(playerDataConfig.PlayerData).Load();
            
            OnWeaponChanged = new UnityEvent<WeaponType>();
            OnMaxHpChanged = new UnityEvent();
            OnMaxArmorChanged = new UnityEvent();
        }

        private void OnApplicationQuit()
        {
            Save();
            PlayerPrefs.Save();
        }

        [ContextMenu("Save")]
        public void Save()
        {
            _playerStorageObject.Save();
        }

        public void SetWeapon(WeaponType type, bool isSave = true)
        {
            if (type == PlayerData.CurrentWeapon)
                return;

            PlayerData.CurrentWeapon = type;
            OnWeaponChanged.Invoke(type);

            if (isSave)
                Save();
        }

        public void UpgradeMaxHp(float additionalValue, bool isSave = true)
        {
            PlayerData.MaxHP += additionalValue;
            
            OnMaxHpChanged.Invoke();
            
            if(isSave)
                Save();
        }
        
        public void UpgradeMaxArmor(float additionalValue, bool isSave = true)
        {
            PlayerData.MaxArmor += additionalValue;
            
            OnMaxArmorChanged.Invoke();
            
            if(isSave)
                Save();
        }
    }
}