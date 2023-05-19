using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public enum ShowType
    {
        Activate, Scale,Slide,Collapse
    }
    
    public class ViewBase : MonoBehaviour
    {
        private class Actions
        {
            public Action<float> Show { get; }
            public Action<float> Hide { get; }

            public Actions(Action<float> show, Action<float> hide)
            {
                Show = show;
                Hide = hide;
            }
        }
        
        [SerializeField] private Transform container;
        [SerializeField] private Transform[] collapsedTransforms;
        [SerializeField] private Transform hidePoint;
        [SerializeField] private ShowType type;
        [SerializeField] protected Ease showEase = Ease.OutBack;
        [SerializeField] protected Ease hideEase = Ease.InBack;
        [SerializeField] protected float animTime;
        [SerializeField] protected bool hideOnAwake = true;

        private Vector3 showPosition;
        private Tween _stateTween;

        private Dictionary<ShowType, Actions> _typeToAction;
        private List<Button> _buttons;
        
        protected virtual void Awake()
        {
            _buttons = new List<Button>();
            GetButtons(transform,_buttons);
            
            _typeToAction = new Dictionary<ShowType, Actions>()
            {
                {ShowType.Scale, new Actions(ScaleShow,ScaleHide)},
                {ShowType.Slide, new Actions(SlideShow,SlideHide)},
                {ShowType.Activate, new Actions(ActivateShow,ActivateHide)},
                {ShowType.Collapse, new Actions(CollapseShow,CollapseHide)},
            };
        }

        protected virtual void Start()
        {
            showPosition = transform.position;
            
            if(hideOnAwake)
                Hide(true);
        }

        public void Show(bool isPermanent = false)
        {
            OnShowBegin();
            
            _stateTween.Kill();
            var time = isPermanent ? 0 : animTime;

            _typeToAction[type].Show(time);
            _stateTween.OnComplete(OnShowEnd);
        }
        
        public void Hide(bool isPermanent = false)
        {
            OnHideBegin();
            
            _stateTween.Kill();
            var time = isPermanent ? 0 : animTime;
            
            _typeToAction[type].Hide(time);
            
            if(!isPermanent)
                _stateTween.OnComplete(OnHideEnd);
            else
                OnHideEnd();
        }

        protected virtual void OnShowBegin()
        {
            if (type == ShowType.Activate)
                return;
            foreach (var button in _buttons)
            {
                button.interactable = false;
            }
        }
        
        protected virtual void OnShowEnd()
        {
            if (type == ShowType.Activate)
                return;
            foreach (var button in _buttons)
            {
                button.interactable = true;
            }
        }
        
        protected virtual void OnHideBegin()
        {
            if (type == ShowType.Activate)
                return;
            foreach (var button in _buttons)
            {
                button.interactable = false;
            }
        }
        
        protected virtual void OnHideEnd()
        {
            if (type == ShowType.Activate)
                return;
            foreach (var button in _buttons)
            {
                button.interactable = true;
            }
        }

        private void GetButtons(Transform transform, List<Button> buttons)
        {
            if(transform.TryGetComponent<Button>(out var button))
                buttons.Add(button);

            foreach (Transform child in transform)
            {
                GetButtons(child, buttons);
            }
        }
        
        
        private void SlideShow(float time)
        {
            _stateTween = container.DOMove(showPosition, time).SetEase(showEase);
        }
        
        private void ScaleHide(float time)
        {
            _stateTween = container.DOScale(Vector3.zero, time).SetEase(hideEase);
        }
        
        private void ScaleShow(float time)
        {
            _stateTween = container.DOScale(Vector3.one, time).SetEase(showEase);
        }
        
        private void SlideHide(float time)
        {
            _stateTween = container.DOMove(hidePoint.position, time).SetEase(hideEase);
        } 
        
        private void ActivateShow(float time)
        {
            container.gameObject.SetActive(true);
        }
        
        private void ActivateHide(float time)
        {
            container.gameObject.SetActive(false);
        }

        private void CollapseShow(float time)
        {
            var seq = DOTween.Sequence();
            seq.Append(container.DOScale(Vector3.one, 0f));
            foreach (var collapsedTransform in collapsedTransforms)
            {
                seq.Join(collapsedTransform.DOScale(Vector3.one, time).SetEase(showEase));
            }

            _stateTween = seq;
        }
        
        private void CollapseHide(float time)
        {
            var seq = DOTween.Sequence();

            foreach (var collapsedTransform in collapsedTransforms)
            {
                seq.Join(collapsedTransform.DOScale(Vector3.zero, time).SetEase(hideEase));
            }
            seq.Append(container.DOScale(Vector3.zero, 0f));
            _stateTween = seq;
        }
    }
}