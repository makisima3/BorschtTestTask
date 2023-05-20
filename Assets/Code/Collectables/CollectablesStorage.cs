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
using UnityEngine.Events;
using Zenject;

namespace Code.Collectables
{

    public class CollectablesStorage : MonoBehaviour, IStorage
    {
        [Inject] private PlayerDataHolder playerDataHolder;
        [Inject] private StorageView storageView;
        [Inject] private LootBagView lootBagView;

        [SerializeField] private CollectablesStorageConfig config;
        [SerializeField] private string guid;

        private Guid _guid;
        private StorageData _storageData;
        private UnityEvent _onValidate;

        public List<CollectableData> Collectables => _storageData.Collectables;

        private void Awake()
        {
            if (Guid.TryParse(guid, out var result))
            {
                _guid = result;
            }
            else
            {
                Debug.LogError($"Storage: {gameObject.name} have invalid guid string");
            }
        }

        private void Start()
        {
            _storageData = playerDataHolder.PlayerData.TryGetStorageData(_guid, config.Collectables);
        }

        private void OnValidate()
        {
            if (_onValidate == null)
            {
                _onValidate = new UnityEvent();
                _onValidate.AddListener(FillGuid);
            }
            
            if(string.IsNullOrEmpty(guid))
                _onValidate.Invoke();
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
            var storageData = _storageData.Collectables.FirstOrDefault(c => c.Type == data.Type);

            if (storageData == default)
                _storageData.Collectables.Add(new CollectableData() { Type = data.Type, Count = data.Count });
            else
                storageData.Count += data.Count;
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
            var storageData = _storageData.Collectables.FirstOrDefault(c => c.Type == data.Type);

            /*var returnedData = new CollectableData()
            {
                Type = storageData.Type,
                Count = storageData.Count,
            };*/

            _storageData.Collectables.Remove(storageData);

            //return returnedData;
        }

        public void RemoveCollectables(List<CollectableData> data)
        {
            foreach (var collectableData in data)
            {
                RemoveCollectable(collectableData);
            }
        }

        [ContextMenu("FillGuid")]
        private void FillGuid()
        {
            guid = Guid.NewGuid().ToString();
            _onValidate.RemoveListener(FillGuid);
        }
    }
}