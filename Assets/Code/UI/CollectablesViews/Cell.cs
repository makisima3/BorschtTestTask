using System;
using Code.Player.Collectables.Enums;
using Code.Player.Data;
using Code.UI.CollectablesViews.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.CollectablesViews
{
    public class Cell : MonoBehaviour,IPointerClickHandler
    {
        [Inject] private CollectablesUIConfig typeToIconConfig;
        
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
            countPlace.gameObject.SetActive(type != CollectableType.None && _count > 0);
            
            if(type == CollectableType.None)
                return;

           
            icon.sprite = typeToIconConfig.GetIcon(_type);
            countPlace.text = count.ToString();
        }
        
        public void SetAction(Action<CollectableData> tapAction) =>  _tapAction = tapAction;
        
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _tapCounter++;
            
            if(_tapCounter >= 2)
                _tapAction?.Invoke(new CollectableData(){Type = _type, Count = _count});
        }
    }
}