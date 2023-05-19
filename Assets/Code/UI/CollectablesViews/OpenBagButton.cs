using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.CollectablesViews
{
    public class OpenBagButton : MonoBehaviour
    {
        [Inject] private BagView bagView;
        
        [SerializeField] private Button button;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            bagView.Show();
        }
    }
}