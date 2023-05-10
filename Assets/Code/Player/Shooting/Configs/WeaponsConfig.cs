using System;
using System.Collections.Generic;
using System.Linq;
using Code.Player.Enums;
using UnityEditor;
using UnityEngine;

namespace Code.Player.Shooting.Configs
{
    [Serializable]
    public class TypeToWeapon
    {
        public WeaponType Type;
        public WeaponConfig Config;
    }
    
    [CreateAssetMenu(fileName = "WeaponsConfig", menuName = "ScriptableObjects/Player/Weapons/WeaponsConfig", order = 1)]
    public class WeaponsConfig : ScriptableObject
    {
        [SerializeField] private List<TypeToWeapon> typeToWeapons;

        public List<TypeToWeapon> TypeToWeapons => typeToWeapons;

        public WeaponConfig GetWeaponConfig(WeaponType type)
        {
            return typeToWeapons.First(ttw => ttw.Type == type).Config;
        }

    }
}