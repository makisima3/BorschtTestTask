using System;
using Code.Player;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Code.UI
{
    public class RestartView : MonoBehaviour
    {
        [Inject] private PlayerHpController playerHpController;
        
        [SerializeField] private Button restartButton;
        [SerializeField] private float animTime = 0.5f;

        public UnityEvent OnRestart { get; private set; }

        private void Awake()
        {
            OnRestart = new UnityEvent();
            restartButton.onClick.AddListener(Restart);
            
            Hide(true);
        }

        private void Start()
        {
            playerHpController.OnDead.AddListener(Show);
        }

        private void Restart()
        {
            Hide();
            OnRestart.Invoke();
        }

        public void Show()
        {
            transform.DOScale(Vector3.one, animTime).SetEase(Ease.OutBack);
        }

        public void Hide(bool isPermanent = false)
        {
            transform.DOScale(Vector3.zero, isPermanent ? 0 : animTime).SetEase(Ease.InBack);;
        }
    }
}