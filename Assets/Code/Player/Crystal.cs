using System;
using UnityEngine;
using Zenject;

namespace Code.Player
{
    public class Crystal : MonoBehaviour
    {
        [Inject] private Collector collector;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<PlayerController>(out _))
                return;

            collector.AddCrystalInBag();
            Destroy(gameObject);
        }
    }
}