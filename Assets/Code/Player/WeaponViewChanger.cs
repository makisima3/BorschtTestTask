using System;
using System.Collections.Generic;
using Code.Player.Data;
using Code.Player.Enums;
using UnityEngine;
using Zenject;

namespace Code.Player
{
    [Serializable]
    public class TypeToWeaponView
    {
        public WeaponType WeaponType;
        public GameObject WeaponView;
        public Transform SpawnPoint;
    }
    
    public class WeaponViewChanger : MonoBehaviour
    {
        [Inject] private PlayerDataHolder playerDataHolder;

        [SerializeField] private List<TypeToWeaponView> typeToWeaponViews;

        public Transform SpawnPoint { get; private set; }

        private void Start()
        {
            playerDataHolder.OnWeaponChanged.AddListener(ChangeWeaponView);
            ChangeWeaponView(playerDataHolder.PlayerData.CurrentWeapon);
        }

        private void ChangeWeaponView(WeaponType type)
        {
            foreach (var typeToWeaponView in typeToWeaponViews)
            {
                typeToWeaponView.WeaponView.SetActive(typeToWeaponView.WeaponType == type);
                if (typeToWeaponView.WeaponType == type)
                    SpawnPoint = typeToWeaponView.SpawnPoint;
            }
        }
    }
}