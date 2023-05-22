using System;
using System.Collections.Generic;
using Code.Player.Configs;
using Code.Player.Enums;
using Code.UI.CollectablesViews.Configs;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.Shop
{
    public class WeaponShopView : ViewBase
    {
        [Inject] private WeaponShopConfig weaponShopConfig;
        [Inject] private DiContainer container;
        
        [SerializeField] private Transform holder;
        [SerializeField] private WeaponShopItemView weaponShopItemViewPrefab;
        [SerializeField] private Button closeButton;

        private List<WeaponShopItemView> _weaponShopItemViews;

        protected override void Awake()
        {
            base.Awake();
            closeButton.onClick.AddListener(() => Hide());
            _weaponShopItemViews = new List<WeaponShopItemView>();

            foreach (WeaponType value in Enum.GetValues(typeof(WeaponType)))
            {
                if(value == WeaponType.None)
                    continue;
                
                CreateWeaponShopItems(value);
            }
        }

        private void CreateWeaponShopItems(WeaponType type)
        {
            var itemView = container.InstantiatePrefabForComponent<WeaponShopItemView>(weaponShopItemViewPrefab, holder);
            itemView.Init(weaponShopConfig.GetPrice(type));

            _weaponShopItemViews.Add(itemView);
        }

        protected override void OnShowBegin()
        {
            base.OnShowBegin();
            
            _weaponShopItemViews.ForEach(w => w.UpdateView());
        }

    }
}