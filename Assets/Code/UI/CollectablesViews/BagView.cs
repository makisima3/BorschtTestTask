using System.Collections.Generic;
using System.Linq;
using Code.Player;
using Code.Player.Collectables.Enums;
using Code.Player.Data;
using Code.UI.CollectablesViews.Configs;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.CollectablesViews
{
    public class BagView : ViewBase
    {
        [Inject] private CollectablesUIConfig collectablesUIConfig;
        [Inject] private PlayerDataHolder playerDataHolder;
        [Inject] private DiContainer container;

        [SerializeField] private Transform cellsHolder;
        [SerializeField] private Button closeButton;

        private List<Cell> _cells;

        protected override void Awake()
        {
            base.Awake();

            if(closeButton != null)
                closeButton.onClick.AddListener(() => Hide());
        }

        protected override void Start()
        {
            base.Start();
            
            CreateCells();
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
        
        private void UpdateBag()
        {
            CreateCells();
            
            var counter = 0;
            foreach (var collectableData in playerDataHolder.PlayerData.CollectablesInBag.OrderBy(c => collectablesUIConfig.GetOrder(c.Type)))
            {
                _cells[counter].SetType(collectableData.Type, collectableData.Count);
                counter++;
            }
        }
    }
}