using System;
using System.Collections;
using System.Collections.Generic;
using Code.Player.Data;
using Code.Player.Enums;
using Code.Player.Shooting.Configs;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.Weapons
{
    public class WeaponChangerView : MonoBehaviour
    {
        [Inject] private PlayerDataHolder playerDataHolder;
        [Inject] private WeaponsConfig weaponsConfig;
        [Inject] private DiContainer container;

        [SerializeField] private WeaponView weaponViewPrefab;
        [SerializeField] private Transform holder;

        private void Awake()
        {
            foreach (var typeToWeapon in weaponsConfig.TypeToWeapons)
            {
                var weaponView = container.InstantiatePrefabForComponent<WeaponView>(weaponViewPrefab, holder);
                weaponView.Init(typeToWeapon.Config, this);
            }

            StartCoroutine(UnityUIFix());
        }

        public void SetWeapon(WeaponType type)
        {
            playerDataHolder.SetWeapon(type);
        }

        private IEnumerator UnityUIFix()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            
            holder.GetComponent<HorizontalLayoutGroup>().spacing += 0.1f;
        }
    }
}