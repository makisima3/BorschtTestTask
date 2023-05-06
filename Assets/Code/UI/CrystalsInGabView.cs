using System;
using Code.Player;
using TMPro;
using UnityEngine;
using Zenject;

namespace Code.UI
{
    public class CrystalsInGabView : MonoBehaviour
    {
        [Inject] private Collector collector;

        [SerializeField] private TMP_Text countPlace;

        private void Start()
        {
            collector.OnCrystalCountInBagChanged.AddListener(SetCount);
            countPlace.text = "Crystals: 0";
        }

        private void SetCount(int count)
        {
            countPlace.text = $"Crystals: {count}";
        }
    }
}