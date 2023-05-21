using System;
using System.Collections.Generic;
using System.Linq;
using Code.Collectables;
using Code.Collectables.Interfaces;
using Code.Player.Collectables.Enums;
using Code.Player.Configs;
using Code.Player.Data;
using Code.Player.World;
using Code.Zone;
using Code.Zone.Impls;
using Code.Zone.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Code.Player
{
    public class Collector : MonoBehaviour, IZoneChecker
    {
        [Inject] private PlayerDataHolder playerDataHolder;
        [Inject] private PlayerActionConfig playerActionConfig;
        [Inject] private PlayerController playerController;

        public List<CollectableData> CollectablesInBag => playerDataHolder.PlayerData.CollectablesInBag;


        private void Start()
        {
            playerController.OnStop.AddListener(GetStorage);
            playerController.OnStop.AddListener(GetWeaponShopStorage);
        }

        private void GetStorage()
        {
            var results = new Collider[999];
            if (Physics.OverlapSphereNonAlloc(playerController.transform.position, playerActionConfig.StorageRadius, results) > 0)
            {
                foreach (var collider in results.Where(c => c != null))
                {
                    if (collider.TryGetComponent<IStorage>(out var storage))
                    {
                        storage.Open();
                    }
                }
            }
        }
        
        private void GetWeaponShopStorage()
        {
            var results = new Collider[999];
            if (Physics.OverlapSphereNonAlloc(playerController.transform.position, playerActionConfig.StorageRadius, results) > 0)
            {
                foreach (var collider in results.Where(c => c != null))
                {
                    if (collider.TryGetComponent<ShopWorld>(out var shop))
                    {
                        shop.Show();
                    }
                }
            }
        }

        public bool HasFreeCells()
        {
            return playerDataHolder.PlayerData.CollectablesInBag.Count < playerDataHolder.PlayerData.CellsCount;
        }
        
        public void AddCollectableInBag(CollectableType type, int count)
        {
            playerDataHolder.PlayerData.AddCollectableInBag(type,count);
            playerDataHolder.Save();
        }
        
        public CollectableData RemoveCollectableFromBag(CollectableType type)
        {
            var data = playerDataHolder.PlayerData.RemoveCollectableFromBag(type);
            playerDataHolder.Save();

            return data;
        }
        
        public void RemoveCollectablesFromBag(List<CollectableType> types)
        {
            foreach (var collectableType in types)
            {
                RemoveCollectableFromBag(collectableType);
            }
        }

        public void OnEnterInZone(ActionZone zone)
        {
            if (zone is not PlayerBaseZone)
                return;

            var collectablesDataToRemove = new List<CollectableData>();
            foreach (var collectableData in playerDataHolder.PlayerData.CollectablesInBag)
            {
                playerDataHolder.PlayerData.AddCollectableInBase(collectableData.Type, collectableData.Count);
                collectablesDataToRemove.Add(collectableData);
            }

            playerDataHolder.PlayerData.RemoveCollectablesFromBag(collectablesDataToRemove.Select(d => d.Type).ToList());
        }

        public void OnExitInZone(ActionZone zone)
        {

        }
    }
}