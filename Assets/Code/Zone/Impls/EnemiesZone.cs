using System;
using System.Collections;
using System.Collections.Generic;
using Code.Enemies;
using Code.Enemies.Enums;
using Code.Player;
using Code.Player.Configs;
using Code.Player.Shooting;
using Code.UI;
using Code.Zone.Enums;
using Code.Zone.Interfaces;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

namespace Code.Zone.Impls
{
    [RequireComponent(typeof(BoxCollider))]
    public class EnemiesZone : ActionZone
    {
        [Inject] private DiContainer container;
        [Inject] private PlayerHpController playerHpController;
        
        [SerializeField] private ZoneActionConfig actionConfig;

        private BoxCollider _collider;
        private List<Enemy> _enemies;
        private Coroutine _spawnCoroutine;
        private bool _isPlayerInside;
        
        public List<Enemy> Enemies => _enemies;

        private void Awake()
        {
            _enemies = new List<Enemy>();
            _collider = GetComponent<BoxCollider>();
            Type = ZoneType.EnemyZone;
        }

        private void Start()
        {
            //_spawnCoroutine = StartCoroutine(SpawnCoroutine());
            for (int i = 0; i < actionConfig.MAXEnemies; i++)
            {
                SpawnEnemy();
            }
            playerHpController.OnDead.AddListener(() => _isPlayerInside = false);
        }

        private Enemy SpawnEnemy()
        {
            var point = GetRandomPoint();
            var enemy = container.InstantiatePrefabForComponent<Enemy>(actionConfig.EnemyPrefab,point,Quaternion.identity,null);
            enemy.Init(point);
            enemy.OnDead.AddListener(OnEnemyDead);
            _enemies.Add(enemy);

            return enemy;
        }

        private Vector3 GetRandomPoint()
        {
            var halfX = transform.localScale.x / 2f;
            var halfZ = transform.localScale.z / 2f;

            var point = new Vector3
            {
                x = transform.position.x + Random.Range(-halfX, halfX),
                y = 0,
                z = transform.position.z + Random.Range(-halfZ, halfZ),
            };

            if(NavMesh.SamplePosition(point, out var myNavHit, 1000 , -1))
            {
                point = myNavHit.position;
            }

            
            return point;
        }

        private void OnEnemyDead(Enemy deadEnemy)
        {
            _enemies.Remove(deadEnemy);
            
            if(_spawnCoroutine != null)
                return;

            _spawnCoroutine = StartCoroutine(SpawnCoroutine());
        }
        
        protected override void OnCheckerEnter(IZoneChecker checker)
        {
            if (checker is not ShootController)
                return;
            
            _isPlayerInside = true;
            _enemies.ForEach(e => e.AttackPlayer());
        }

        protected override void OnCheckerExit(IZoneChecker checker)
        {
            if (checker is not ShootController)
                return;
            
            _isPlayerInside = false;
        }
        
        private IEnumerator SpawnCoroutine()
        {
            while (_enemies.Count < actionConfig.MAXEnemies)
            {
                var enemy = SpawnEnemy();

                if (_isPlayerInside)
                {
                    yield return new WaitForEndOfFrame();
                    yield return new WaitForEndOfFrame();
                    yield return new WaitForEndOfFrame();
                    enemy.AttackPlayer();
                }

                yield return new WaitForSeconds(1 / actionConfig.SpawnRate);
            }

            _spawnCoroutine = null;
        }
    }
}