using System;
using Code.Player;
using Code.Player.Collectables.Enums;
using Code.Player.Data;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Code.Collectables
{
    public class Collectable : MonoBehaviour
    {
        [Inject] private Collector collector;

        [SerializeField] private CollectableType type;
        [SerializeField] private int minCount;
        [SerializeField] private int maxCount;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<PlayerController>(out _))
                return;

            collector.AddCollectableInBag(type, Random.Range(minCount, maxCount + 1));
            Destroy(gameObject);
        }
    }
}