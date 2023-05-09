using System;
using Code.Player;
using Code.Player.Shooting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI
{
    [RequireComponent(typeof(Button))]
    public class StopShootButton : MonoBehaviour
    {
        [Inject] private ShootController shootController;

        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(shootController.DisableShoot);
        }
    }
}