using System;
using System.Collections;
using System.Collections.Generic;
using Code.Enemies;
using Plugins.MyUtils;
using UnityEngine;

namespace Code.Player
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 3f;
        
        private float _damage;
        private float _distance;
        private float _maxDistance;
        private BulletsPool _bulletsPool ;
        

        public void Init(Vector3 target,BulletsPool pool,Vector3 spawnPosition,float damage,float maxDistance)
        {
            _damage = damage;
            _bulletsPool = pool;
            _maxDistance = maxDistance;

            transform.position = spawnPosition;
            transform.rotation = ExtraMathf.GetRotation(transform.position, target, Vector3.up);
            
            StartCoroutine(ReturnToPoolWithDelay(5f));
        }

        private void Update()
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            _distance += (transform.forward * speed * Time.deltaTime).magnitude;
            
            if(_distance >= _maxDistance)
                StartCoroutine(ReturnToPoolWithDelay(0f));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.Damage(_damage);
            }

            StartCoroutine(ReturnToPoolWithDelay(0f));
        }

        private IEnumerator ReturnToPoolWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _distance = 0;
            _bulletsPool.ReturnObject(this);
        }
    }
}