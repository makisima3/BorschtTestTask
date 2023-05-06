using System;
using System.Linq;
using Code.Zone.Enums;
using Code.Zone.Interfaces;
using UnityEngine;

namespace Code.Zone
{
    public abstract class ActionZone : MonoBehaviour
    {
        
        public ZoneType Type { get; protected set; }

        private void OnTriggerEnter(Collider other)
        {
            var components = other.GetComponents<IZoneChecker>();

            foreach (var checker in components)
            {
                OnCheckerEnter(checker);
                checker.OnEnterInZone(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var components = other.GetComponents<IZoneChecker>();

            foreach (var checker in components)
            {
                OnCheckerExit(checker);
                checker.OnExitInZone(this);
            }
        }

        protected abstract void OnCheckerEnter(IZoneChecker checker);
        protected abstract void OnCheckerExit(IZoneChecker checker);
    }
}