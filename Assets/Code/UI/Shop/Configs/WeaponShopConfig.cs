using System;
using System.Collections.Generic;
using System.Linq;
using Code.Player.Collectables.Enums;
using Code.Player.Enums;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.UI.CollectablesViews.Configs
{
    [Serializable]
    public class Price
    {
        public CollectableType Type;
        public int Count;
    }
    
    [Serializable]
    public class TypeToPrice
    {
        public WeaponType Type;
        public bool IsADS;
        [ConditionalHide("IsADS", false)]
        public List<Price> Price;
    }

    [CreateAssetMenu(fileName = "ShopConfig", menuName = "ScriptableObjects/UI/ShopConfig", order = 0)]
    public class WeaponShopConfig : ScriptableObject
    {
        [SerializeField] private List<TypeToPrice> typeToPrice;

        public TypeToPrice GetPrice(WeaponType type)
        {
            return typeToPrice.First(ttp => ttp.Type == type);
        }
    }
}