using System;
using System.Collections.Generic;
using System.Linq;
using Code.Collectables.Interfaces;
using Code.Player;
using Code.Player.Collectables.Enums;
using Code.Player.Data;
using Code.UI.CollectablesViews;
using Code.UI.CollectablesViews.Configs;
using UnityEngine;
using Zenject;

namespace Code.Collectables
{
    
    public class PlayerBaseCollectablesStorage : MonoBehaviour,IStorage
    {
        [Inject] private PlayerDataHolder playerDataHolder;
        [Inject] private StorageView storageView;
        [Inject] private LootBagView lootBagView;
        

        public List<CollectableData> Collectables => playerDataHolder.PlayerData.CollectablesInBase;

        private void Awake()
        {
        }

        private void Start()
        {
           
        }

        public void Open()
        {
            storageView.SetStorage(this);
            lootBagView.SetStorage(this);
            
            storageView.UpdateBag();
            
            storageView.Show();
            lootBagView.Show();
        }

        public void AddCollectable(CollectableData data)
        {
           playerDataHolder.PlayerData.AddCollectableInBase(data.Type,data.Count);
        }

        public void AddCollectables(List<CollectableData> data)
        {
            foreach (var collectableData in data)
            {
                AddCollectable(collectableData);
            }
        }
        
        public void RemoveCollectable(CollectableData data)
        {
            playerDataHolder.PlayerData.RemoveAddCollectableFromBase(data.Type);
        }

        public void RemoveCollectables(List<CollectableData> data)
        {
            foreach (var collectableData in data)
            {
                RemoveCollectable(collectableData);
            }
        }
    }
}