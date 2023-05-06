using System;
using System.Collections;
using System.Collections.Generic;
using Code.Enemies;
using UnityEngine;

namespace Code.Player
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 3f;
        [SerializeField] private int damage = 1;
        
        private Transform _target;
        private BulletsPool _bulletsPool ;
        

        public void Init(Transform target,BulletsPool pool,Vector3 spawnPosition)
        {
            _target = target;
            _bulletsPool = pool;

            transform.position = spawnPosition;
            transform.LookAt(target);
            
            StartCoroutine(ReturnToPoolWithDelay(5f));
        }

        private void Update()
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.Damage(damage);
            }

            StartCoroutine(ReturnToPoolWithDelay(0f));
        }

        private IEnumerator ReturnToPoolWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _bulletsPool.ReturnObject(this);
        }
    }
}