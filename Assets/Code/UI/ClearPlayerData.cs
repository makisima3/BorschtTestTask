using System;
using Code.Player.Configs;
using Code.Player.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Code.UI
{
    [RequireComponent(typeof(Button))]
    public class ClearPlayerData : MonoBehaviour
    {
        [Inject] private PlayerDataHolder playerDataHolder;
        [Inject] private PlayerDataConfig playerDataConfig;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnCLick);
        }

        private void OnCLick()
        {
            playerDataHolder.Save();

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}