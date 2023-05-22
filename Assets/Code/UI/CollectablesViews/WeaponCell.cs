using System;
using Code.Player.Collectables.Enums;
using Code.Player.Data;
using Code.Player.Enums;
using Code.Player.Shooting.Configs;
using Code.UI.CollectablesViews.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.CollectablesViews
{
    public class WeaponCell : MonoBehaviour,IPointerClickHandler
    {
        [Inject] private WeaponsConfig weaponsConfig;
        
        [SerializeField] private Image icon;
        [SerializeField] private Image lockIcon;
        
        private WeaponType _type;
        private Action<WeaponType> _tapAction;
        private int _tapCounter;
        
        public bool IsUnlocked { get; private set; }
        
        public void SetType(WeaponType type,bool isUnlocked)
        {
            _type = type;
            IsUnlocked = isUnlocked;

            lockIcon.gameObject.SetActive(!isUnlocked);
            icon.gameObject.SetActive(type != WeaponType.None && isUnlocked);
            
            if(type == WeaponType.None)
                return;

           
            icon.sprite = weaponsConfig.GetWeaponConfig(_type).Icon;
        }
        
        public void SetAction(Action<WeaponType> tapAction) =>  _tapAction = tapAction;
        
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if(!IsUnlocked)
                return;
            
            _tapCounter++;

            if (_tapCounter >= 2)
            {
                _tapAction?.Invoke(_type);
                _tapCounter = 0;
            }
        }
    }
}