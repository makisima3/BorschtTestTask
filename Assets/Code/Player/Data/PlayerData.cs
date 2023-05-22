using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Code.Player.Collectables.Enums;
using Code.Player.Enums;
using Code.Player.Shooting.Configs;
using Code.Player.Utils;
using UnityEngine;

namespace Code.Player.Data
{
    [Serializable]
    public class WeaponData
    {
        public WeaponType Type;
        public int Ammo;
        public int AmmoReserve;
        public bool Unlocked;
    }

    [Serializable]
    public class CollectableData
    {
        public CollectableType Type;
        public int Count;
    }

    [Serializable]
    public class StorageData
    {
        public Guid Guid;
        public List<CollectableData> Collectables;
    }

    [Serializable]
    public class PlayerData
    {
        public int CellsCount;

        public List<StorageData> StorageData;
        public List<Guid> UpgradePoints;

        public WeaponType CurrentWeapon;
        public List<WeaponData> WeaponData;
        public List<WeaponType> UnlockedWeapons;
        public List<CollectableData> CollectablesInBag;
        public List<CollectableData> CollectablesInBase;

        public List<WeaponType> EquipedWeapons;
        public int unlockedWeaponCellsCount = 1;

        public float CurrentHP = 2f;
        public float MaxHP = 2f;
        
        public float CurrentArmor = 2f;
        public float MaxArmor = 2f;
        public float ArmorStartRegenDelay = 1f;
        public float ArmorRegenPerTick = 0.1f;
        public float ArmorTickRate = 1f;

        public float BaseDamage;
        public float Speed = 3f;
        
        public WeaponData GetWeaponData(WeaponConfig weaponConfig)
        {
            var weaponData = WeaponData.FirstOrDefault(wd => wd.Type == weaponConfig.Type);
            if (weaponData != default)
                return weaponData;

            weaponData = new WeaponData()
            {
                Type = weaponConfig.Type,
                Ammo = weaponConfig.Ammo,
                AmmoReserve = weaponConfig.AmmoReserve,
                Unlocked = UnlockedWeapons.Contains(weaponConfig.Type),
            };
            WeaponData.Add(weaponData);

            return weaponData;
        }

        public bool IsWeaponUnlocked(WeaponType weaponType)
        {
            return UnlockedWeapons.Contains(weaponType);
        }

        public bool TryEquipWeapon(WeaponType weaponType)
        {
            if (EquipedWeapons.Count + 1 > unlockedWeaponCellsCount)
                return false;
            
            EquipedWeapons.Add(weaponType);
            return true;
        }
        
        public bool UnequipWeapon(WeaponType weaponType)
        {
            EquipedWeapons.Remove(weaponType);
            return true;
        }
        
        public bool IsEnoughResources(List<CollectableData> collectableDatas)
        {
            var result = true;

            foreach (var collectableData in collectableDatas)
            {
                var data = CollectablesInBase.FirstOrDefault(c => c.Type == collectableData.Type);
                if(data != null && data.Count >= collectableData.Count)
                    continue;

                result = false;
            }

            return result;
        }

        public void RemoveResources(List<CollectableData> collectableDatas)
        {
            foreach (var collectableData in collectableDatas)
            {
                var data = CollectablesInBase.FirstOrDefault(c => c.Type == collectableData.Type);
                if (data != null && data.Count >= collectableData.Count)
                {
                    data.Count -= collectableData.Count;
                    
                    if(data.Count < 0)
                        Debug.LogError("You remove to much resources");
                    data.Count = 0;

                    if (data.Count <= 0)
                        CollectablesInBase.Remove(data);
                }
            }
        }

        public bool UnlockAndTryEquipWeapon(WeaponConfig weaponConfig)
        {
            UnlockedWeapons.Add(weaponConfig.Type);
            var data = GetWeaponData(weaponConfig);
            data.Unlocked = true;

            if (!TryEquipWeapon(weaponConfig.Type))
            {
                if (!EnumConvertor.TryGetValue<WeaponType, CollectableType>(weaponConfig.Type, out var collectableType))
                {
                    Debug.LogError($"Cant convert weapon type ({weaponConfig.Type}) to collectable type!");
                    return false;
                }
                
                AddCollectableInBase(collectableType, 1);
                return false;
            }

            return true;
        }
        
        public void ResetAllWeaponsAmmo(WeaponsConfig weaponsConfig)
        {
            foreach (var weaponData in WeaponData)
            {
                var config = weaponsConfig.GetWeaponConfig(weaponData.Type);
                weaponData.Ammo = config.Ammo;
                weaponData.AmmoReserve = config.AmmoReserve;
            }
        }

        public void AddCollectableInBag(CollectableType type, int count)
        {
            CollectablesInBag ??= new List<CollectableData>();
            if (CollectablesInBag.Any(c => c.Type == type))
            {
                CollectablesInBag.First(c => c.Type == type).Count += count;
            }
            else
            {
                CollectablesInBag.Add(new CollectableData() { Type = type, Count = count });
            }
        }
        
        public CollectableData RemoveCollectableFromBag(CollectableType type)
        {
            CollectablesInBag ??= new List<CollectableData>();

            var collectable = CollectablesInBag.FirstOrDefault(c => c.Type == type);
            CollectablesInBag.Remove(collectable);

            return collectable;
        }
        
        public void RemoveCollectablesFromBag(List<CollectableType> types)
        {
            foreach (var type in types)
            {
                RemoveCollectableFromBag(type);
            }
        }

        public int GetCollectablesInBagCount(CollectableType type)
        {
            CollectablesInBag ??= new List<CollectableData>();
            if (CollectablesInBag.Any(c => c.Type == type))
            {
                return CollectablesInBag.First(c => c.Type == type).Count;
            }

            return 0;
        }

        public void AddCollectableInBase(CollectableType type, int count)
        {
            CollectablesInBase ??= new List<CollectableData>();
            if (CollectablesInBase.Any(c => c.Type == type))
            {
                CollectablesInBase.First(c => c.Type == type).Count += count;
            }
            else
            {
                CollectablesInBase.Add(new CollectableData() { Type = type, Count = count });
            }
        }
        
        public void RemoveAddCollectableFromBase(CollectableType type)
        {
            CollectablesInBase ??= new List<CollectableData>();
            if (CollectablesInBase.Any(c => c.Type == type))
            {
                CollectablesInBase.Remove(CollectablesInBase.First(c => c.Type == type));
            }
            
        }

        public int GetCollectablesInBaseCount(CollectableType type)
        {
            CollectablesInBase ??= new List<CollectableData>();
            if (CollectablesInBase.Any(c => c.Type == type))
            {
                return CollectablesInBase.First(c => c.Type == type).Count;
            }

            return 0;
        }

        public StorageData TryGetStorageData(Guid guid, List<CollectableData> defaultData )
        {

            var storageData = StorageData.FirstOrDefault(sd => sd.Guid == guid);

            if (storageData == default)
            {
                storageData = new StorageData()
                {
                    Collectables = new List<CollectableData>(defaultData),
                    Guid = guid
                };

                StorageData.Add(storageData);
            }


            return storageData;
        }

        public void SetStorageData(Guid guid, List<CollectableData> data)
        {
            var storageData = StorageData.FirstOrDefault(sd => sd.Guid == guid);

            if (storageData != default)
            {
                foreach (var collectableData in data)
                {
                    var savedData = storageData.Collectables.FirstOrDefault(d => d.Type == collectableData.Type);

                    if (savedData != null)
                        savedData.Count += collectableData.Count;
                    else
                        storageData.Collectables.Add(collectableData);
                }
                return;
            }
            
            var newStorageData = new StorageData()
            {
                Collectables = data,
                Guid = guid
            };
            StorageData.Add(newStorageData);
        }

        public void AddUpgradePoint(Guid guid)
        {
            UpgradePoints ??= new List<Guid>();

            UpgradePoints.Add(guid);
        }
        
        public bool IsUpgradePointCollected(Guid guid)
        {
            UpgradePoints ??= new List<Guid>();

            return UpgradePoints.Contains(guid);
        }
    }
}