using System;
using System.Collections.Generic;
using Code.Player;
using Code.Player.Data;
using Code.Player.Enums;
using Code.Player.Shooting.Configs;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.Weapons
{
    public class WeaponView : MonoBehaviour
    {
        [Inject] private PlayerDataHolder playerDataHolder;
        [Inject] private WeaponChangerView weaponChangerView;
        
        [SerializeField] private Image icon;
        [SerializeField] private Button button;

        private WeaponConfig _weaponConfig;
        private WeaponChangerView _weaponChangerView;
        private Dictionary<int, KeyCode> _indexToKeyCode;
        private int _index;

        public void Init(WeaponConfig config, WeaponChangerView weaponChangerView)
        {
            _weaponConfig = config;
            _weaponChangerView = weaponChangerView;
            _index = transform.GetSiblingIndex() + 1;

            icon.sprite = _weaponConfig.Icon;
            gameObject.SetActive(playerDataHolder.PlayerData.EquipedWeapons.Contains(_weaponConfig.Type));
            
            playerDataHolder.OnEquipedWeaponChanged.AddListener(OnWeaponEquiped);
            button.onClick.AddListener(OnCLick);

            _indexToKeyCode = new Dictionary<int, KeyCode>()
            {
                {1,KeyCode.Alpha1},
                {2,KeyCode.Alpha2},
                {3,KeyCode.Alpha3},
                {4,KeyCode.Alpha4},
                {5,KeyCode.Alpha5},
                {6,KeyCode.Alpha6},
                {7,KeyCode.Alpha7},
                {8,KeyCode.Alpha8},
                {9,KeyCode.Alpha9},
            };
        }

        private void OnWeaponEquiped()
        {
            gameObject.SetActive(playerDataHolder.PlayerData.EquipedWeapons.Contains(_weaponConfig.Type));
            weaponChangerView.UnityFix();
            _index = transform.GetSiblingIndex() + 1;
        }

        private void OnCLick()
        {
            _weaponChangerView.SetWeapon(_weaponConfig.Type);
        }

        private void Update()
        {
            if (Input.GetKeyDown(_indexToKeyCode[_index]))
            {
                OnCLick();
            }
        }
    }
}