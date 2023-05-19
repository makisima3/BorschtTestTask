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
    public class StorageView : ViewBase
    {
        [Inject] private CollectablesUIConfig collectablesUIConfig;
        [Inject] private DiContainer container;
        [Inject] private Collector collector;
        [Inject] private LootBagView lootBagView;

        [SerializeField] private Transform cellsHolder;
        [SerializeField] private Button GetAllButton;
        [SerializeField] private Button closeButton;

        private List<Cell> _cells;
        private IStorage _storage;


        protected override void Awake()
        {
            base.Awake();
            
            GetAllButton.onClick.AddListener(GetAll);
            closeButton.onClick.AddListener(() => Hide());
        }

        protected override void OnHideBegin()
        {
            base.OnHideBegin();

            lootBagView.Hide();
        }
        
        private void CreateCells(int count)
        {
            ClearCells();

           for (int i = 0; i < count; i++)
            {
                var newCell = container.InstantiatePrefabForComponent<Cell>(collectablesUIConfig.CellPrefab,cellsHolder);
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
            if(_storage == null)
                return;
            
            CreateCells(_storage.Collectables.Count);
            
            var counter = 0;
            foreach (var collectableData in _storage.Collectables.OrderBy(c => collectablesUIConfig.GetOrder(c.Type)))
            {
                _cells[counter].SetType(collectableData.Type, collectableData.Count);
                counter++;
            }
        }
        
        private void GetAll()
        {
            var collectableDataToRemove = new List<CollectableData>();
            foreach (var collectableData in _storage.Collectables)
            {
                if(!collector.HasFreeCells())
                    break;
                
                collectableDataToRemove.Add(collectableData);
                collector.AddCollectableInBag(collectableData.Type, collectableData.Count);
            }

            _storage.RemoveCollectables(collectableDataToRemove);
            
            UpdateBag();
            lootBagView.UpdateBag();
        }

        public void SetStorage(IStorage iStorage) => _storage = iStorage;
        
        private void CellTapAction(CollectableData collectableData)
        {
            if(!collector.HasFreeCells())
                return;
            
            _storage.RemoveCollectable(collectableData);
            collector.AddCollectableInBag(collectableData.Type, collectableData.Count);
            UpdateBag();
            lootBagView.UpdateBag();
        }
        
        
    }
}