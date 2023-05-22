using System;
using System.Collections.Generic;
using Code.Player.Data;
using Code.Player.Upgrades.Enums;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Code.Player.Upgrades
{
    public class UpgradePoint : MonoBehaviour
    {
        [Inject] private PlayerDataHolder playerDataHolder;

        [SerializeField] private UpgradeType type;
        [SerializeField] private float value;
        [SerializeField] private string guid;
        
        private Guid _guid;
        private Dictionary<UpgradeType, Action> _typeToAction;
        private UnityEvent _onValidate;

        private void Awake()
        {
            _typeToAction = new Dictionary<UpgradeType, Action>()
            {
                { UpgradeType.HP, UpgradeHP },
                { UpgradeType.Armor, UpgradeArmor },
                { UpgradeType.Bag, UpgradeBag },
                { UpgradeType.Speed, UpgradeSpeed },
                { UpgradeType.BaseDamage, UpgradeBaseDamage },
                { UpgradeType.WeaponCell, UpgradeWeaponCell },
            };
            
            if (Guid.TryParse(guid, out var result))
            {
                _guid = result;
            }
            else
            {
                Debug.LogError($"Upgrade point: {gameObject.name} have invalid guid string");
            }
        }

        private void Start()
        {
            if (playerDataHolder.PlayerData.IsUpgradePointCollected(_guid))
                Destroy(gameObject);
        }

        private void OnValidate()
        {
            if (_onValidate == null)
            {
                _onValidate = new UnityEvent();
                _onValidate.AddListener(FillGuid);
            }
            
            if(string.IsNullOrEmpty(guid))
                _onValidate.Invoke();
        }
        
        private void UpgradeWeaponCell()
        {
            playerDataHolder.PlayerData.unlockedWeaponCellsCount += (int) value;
        }
        
        private void UpgradeHP()
        {
            playerDataHolder.UpgradeMaxHp(value);
        }
        
        private void UpgradeArmor()
        {
            playerDataHolder.UpgradeMaxArmor(value);
        }
        private void UpgradeBag()
        {
            playerDataHolder.PlayerData.CellsCount += Mathf.RoundToInt(value);
        }

        private void UpgradeSpeed()
        {
            playerDataHolder.PlayerData.Speed += value;
        }
        
        private void UpgradeBaseDamage()
        {
            playerDataHolder.PlayerData.BaseDamage += value;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<PlayerController>(out _))
                return;
            
            _typeToAction[type]?.Invoke();
            playerDataHolder.PlayerData.AddUpgradePoint(_guid);
            
            Destroy(gameObject);
        }
        
        [ContextMenu("FillGuid")]
        private void FillGuid()
        {
            guid = Guid.NewGuid().ToString();
            _onValidate.RemoveListener(FillGuid);
        }
    }
}
