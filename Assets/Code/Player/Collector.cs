using System;
using Code.Zone;
using Code.Zone.Impls;
using Code.Zone.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Player
{
    public class Collector : MonoBehaviour, IZoneChecker
    {
        private int _crystalCountInBag;
        private int _crystalCountGlobal;

        public UnityEvent<int> OnCrystalCountInBagChanged { get; private set; }
        public UnityEvent<int> OnCrystalCountGlobalChanged { get; private set; }

        private void Awake()
        {
            OnCrystalCountInBagChanged = new UnityEvent<int>();
            OnCrystalCountGlobalChanged = new UnityEvent<int>();
        }

        public void AddCrystalInBag()
        {
            _crystalCountInBag++;
            OnCrystalCountInBagChanged.Invoke(_crystalCountInBag);
        }

        public void OnEnterInZone(ActionZone zone)
        {
            if (zone is not PlayerBaseZone)
                return;
            
            _crystalCountGlobal += _crystalCountInBag;
            _crystalCountInBag = 0;
                
            OnCrystalCountInBagChanged.Invoke(_crystalCountInBag);
            OnCrystalCountGlobalChanged.Invoke(_crystalCountGlobal);
        }

        public void OnExitInZone(ActionZone zone)
        {

        }
    }
}