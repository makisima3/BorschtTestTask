using System;
using Code.Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI
{
    [RequireComponent(typeof(Button))]
    public class KillPlayerButton : MonoBehaviour
    {
        [Inject] private PlayerHpController playerHpController;
        [Inject] private RestartView restartView;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(playerHpController.KillMe);
        }

        private void Start()
        {
            playerHpController.OnDead.AddListener(() => gameObject.SetActive(false));
            restartView.OnRestart.AddListener(() => gameObject.SetActive(true));
        }
    }
}