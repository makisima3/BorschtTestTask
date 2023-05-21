using System;
using System.Collections.Generic;
using Code.Player.Data;
using Code.Player.Enums;
using Code.Player.Shooting.Configs;
using Code.UI.CollectablesViews.Configs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.Shop
{
    public class WeaponShopItemView : MonoBehaviour,IPointerClickHandler
    {
        [Inject] private PlayerDataHolder playerDataHolder;
        [Inject] private CollectablesUIConfig collectablesUIConfig;
        [Inject] private DiContainer container;
        [Inject] private WeaponsConfig weaponsConfig;
        
        [SerializeField] private Image icon;
        [SerializeField] private Color boughtColor;
        [SerializeField] private Transform priceHolder;
        [SerializeField] private PriceView priceViewPrefab;
        
        private bool _isBought;
        private Color baseColor;
        private TypeToPrice _typeToPrice;
        private List<PriceView> _priceViews;

        public void Init(TypeToPrice typeToPrice)
        {
            _typeToPrice = typeToPrice;
            baseColor = icon.color;
            icon.sprite = weaponsConfig.GetWeaponConfig(_typeToPrice.Type).Icon;
            _priceViews = new List<PriceView>();
            foreach (var price in _typeToPrice.Price)
            {
                CreatePriceView(price);
            }
        }

        private void CreatePriceView(Price price)
        {
            var priceView = container.InstantiatePrefabForComponent<PriceView>(priceViewPrefab,priceHolder);
            priceView.Init(collectablesUIConfig.GetIcon(price.Type), price.Count);
            _priceViews.Add(priceView);
        }

        public void UpdateView()
        {
           _isBought = playerDataHolder.PlayerData.IsWeaponUnlocked(_typeToPrice.Type);
           priceHolder.gameObject.SetActive(!_isBought);
           icon.color = _isBought || playerDataHolder.PlayerData.IsEnoughResources(GetCollectableDatasFromPrice()) ? baseColor : boughtColor;
        }
        

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_isBought)
                return;

            OnBuy();
        }

        private void OnBuy()
        {
            if(!playerDataHolder.PlayerData.IsEnoughResources(GetCollectableDatasFromPrice()))
                return;
            
            playerDataHolder.PlayerData.RemoveResources(GetCollectableDatasFromPrice());
            playerDataHolder.UnlockWeapon(weaponsConfig.GetWeaponConfig(_typeToPrice.Type));
            UpdateView();
        }

        private List<CollectableData> GetCollectableDatasFromPrice()
        {
            var resourcesToCheck = new List<CollectableData>();

            foreach (var price in _typeToPrice.Price)
            {
                resourcesToCheck.Add(new CollectableData()
                {
                    Type = price.Type,
                    Count = price.Count,
                });
            }

            return resourcesToCheck;
        }
        
    }
}