using System;
using System.Collections.Generic;
using System.Linq;
using Code.Player.Collectables.Enums;
using UnityEngine;

namespace Code.UI.CollectablesViews.Configs
{
    [Serializable]
    public class TypeToIcon
    {
        public CollectableType Type;
        public Sprite Icon;
        public int Order;
    }

    [CreateAssetMenu(fileName = "CollectablesUIConfig", menuName = "ScriptableObjects/UI/CollectablesUIConfig", order = 0)]
    public class CollectablesUIConfig : ScriptableObject
    {
        [SerializeField] private List<TypeToIcon> typeToIcons;
        [SerializeField] private Cell cellPrefab;
        [SerializeField] private WeaponCell weaponCellPrefab;

        public Cell CellPrefab => cellPrefab;
        public WeaponCell WeaponCellPrefab => weaponCellPrefab;

        public Sprite GetIcon(CollectableType type)
        {
            return typeToIcons.First(tti => tti.Type == type).Icon;
        }
        
        public int GetOrder(CollectableType type)
        {
            return typeToIcons.First(tti => tti.Type == type).Order;
        }
    }
}