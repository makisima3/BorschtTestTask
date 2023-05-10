using System;
using System.Collections.Generic;
using System.Linq;
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
    }
    
    [Serializable]
    public class PlayerData
    {
        public WeaponType CurrentWeapon;
        public List<WeaponData> WeaponData;

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
    }
}