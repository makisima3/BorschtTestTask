using System;
using Code.Player.Collectables.Enums;
using Code.Player.Data;
using Code.Player.Enums;
using Code.Player.Shooting.Configs;
using Code.Player.Utils;
using Code.UI.CollectablesViews.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.CollectablesViews
{
    public class Cell : MonoBehaviour, IPointerClickHandler
    {
        [Inject] private CollectablesUIConfig typeToIconConfig;
        [Inject] private WeaponsConfig weaponsConfig;

        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text countPlace;

        private CollectableType _type;
        private int _count;
        private Action<CollectableData> _tapAction;
        private int _tapCounter;

        public void SetType(CollectableType type, int count)
        {
            _type = type;
            _count = count;

            icon.gameObject.SetActive(type != CollectableType.None);
            countPlace.gameObject.SetActive(type != CollectableType.None && _count > 1);

            if (type == CollectableType.None)
                return;

            var sprite = GetSprite();

            icon.sprite = sprite;
            countPlace.text = count.ToString();
        }

        private Sprite GetSprite()
        {
            Sprite sprite;

            if (EnumConvertor.TryGetValue<CollectableType, WeaponType>(_type, out var weaponType))
            {
                sprite = weaponsConfig.GetWeaponConfig(weaponType).Icon;
            }
            else
            {
                sprite = typeToIconConfig.GetIcon(_type);
            }

            return sprite;
        }


        public void SetAction(Action<CollectableData> tapAction) => _tapAction = tapAction;


        public void OnPointerClick(PointerEventData eventData)
        {
            _tapCounter++;

            if (_tapCounter >= 2)
            {
                _tapAction?.Invoke(new CollectableData() { Type = _type, Count = _count });
                _tapCounter = 0;
            }
        }
    }
}