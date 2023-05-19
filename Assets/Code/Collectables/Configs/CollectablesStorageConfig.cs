using System;
using System.Collections.Generic;
using System.Linq;
using Code.Player.Collectables.Enums;
using Code.Player.Data;
using UnityEngine;

namespace Code.UI.CollectablesViews.Configs
{

    [CreateAssetMenu(fileName = "CollectablesStorageConfig", menuName = "ScriptableObjects/Collectables/CollectablesStorageConfig", order = 0)]
    public class CollectablesStorageConfig : ScriptableObject
    {
        [SerializeField] private List<CollectableData> collectables;

        public List<CollectableData> Collectables => collectables;
    }
}