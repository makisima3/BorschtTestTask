using System.Collections.Generic;
using System.Linq;
using Code.Collectables;
using Code.Collectables.Interfaces;
using Code.Player;
using Code.Player.Collectables.Enums;
using Code.Player.Data;
using Code.UI.CollectablesViews.Configs;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.CollectablesViews
{
    public class LootBagView : ViewBase
    {
        [Inject] private Collector collector;
        [Inject] private CollectablesUIConfig collectablesUIConfig;
        [Inject] private PlayerDataHolder playerDataHolder;
        [Inject] private DiContainer container;
        [Inject] private StorageView storageView;

        
        [SerializeField] private Button dropAllButton;

        private IStorage _storage;
       
        
        [SerializeField] private Transform cellsHolder;

        private List<Cell> _cells;

        protected override void Awake()
        {
            base.Awake();
            
            dropAllButton.onClick.AddListener(DropAll);
        }

        protected override void Start()
        {
            base.Start();
            
            Hide();
            CreateCells();
            
            UpdateBag();
        }

        protected override void OnShowBegin()
        {
            base.OnShowBegin();

            UpdateBag();
        }

        private void CreateCells()
        {
            ClearCells();
            
            for (int i = 0; i < playerDataHolder.PlayerData.CellsCount; i++)
            {
                var newCell = container.InstantiatePrefabForComponent<Cell>(collectablesUIConfig.CellPrefab, cellsHolder);
                newCell.SetAction(CellTapAction);
                newCell.SetType(CollectableType.None, 0);
                _cells.Add(newCell);
            }
        }
        
        private void ClearCells()
        {
            _cells ??= new List<Cell>();
            
            foreach (var cell in _cells)
            {
                Destroy(cell.gameObject);
            }
            
            _cells.Clear();
        }
        
        public void UpdateBag()
        {
            CreateCells();
            
            var counter = 0;
            foreach (var collectableData in playerDataHolder.PlayerData.CollectablesInBag.OrderBy(c => collectablesUIConfig.GetOrder(c.Type)))
            {
                _cells[counter].SetType(collectableData.Type, collectableData.Count);
                counter++;
            }
        }

        private void DropAll()
        {
            var types = collector.CollectablesInBag.Select(c => c.Type).ToList();

            _storage.AddCollectables(collector.CollectablesInBag);
            collector.RemoveCollectablesFromBag(types);
            
            UpdateBag();
            storageView.UpdateBag();
        }
        
        public void SetStorage(IStorage iStorage) => _storage = iStorage;
        
        private void CellTapAction(CollectableData collectableData)
        {
            _storage.AddCollectable(collectableData);
            collector.RemoveCollectableFromBag(collectableData.Type);
            
            UpdateBag();
            storageView.UpdateBag();
        }
    }
}