using System;
using System.Collections.Generic;
using System.Linq;
using Code.Player.Collectables.Enums;
using Code.Player.Enums;
using Code.Player.Shooting.Configs;

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

        public WeaponType CurrentWeapon;
        public List<WeaponData> WeaponData;
        public List<CollectableData> CollectablesInBag;
        public List<CollectableData> CollectablesInBase;

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
            };
            WeaponData.Add(weaponData);

            return weaponData;
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
    }
}