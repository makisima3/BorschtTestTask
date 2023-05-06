using System;
using Code.Zone.Enums;
using Code.Zone.Interfaces;
using UnityEngine;

namespace Code.Zone.Impls
{
    public class PlayerBaseZone : ActionZone
    {
        private void Awake()
        {
            Type = ZoneType.PlayerBase;
        }

        protected override void OnCheckerEnter(IZoneChecker checker)
        {
            
        }

        protected override void OnCheckerExit(IZoneChecker checker)
        {
           
        }
    }
}