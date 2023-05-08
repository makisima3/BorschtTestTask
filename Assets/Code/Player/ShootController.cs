using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Enemies;
using Code.Player.Configs;
using Code.Zone;
using Code.Zone.Enums;
using Code.Zone.Impls;
using Code.Zone.Interfaces;
using Plugins.MyUtils;
using UnityEngine;
using Zenject;

namespace Code.Player
{
    [RequireComponent(typeof(PlayerAnimationEvents))]
    public class ShootController : MonoBehaviour, IZoneChecker
    {
        [Inject] private DiContainer container;
        [Inject] private PlayerActionConfig actionConfig;
        [Inject] private PlayerHpController playerHpController;

        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform view;
        [SerializeField] private Animator animator;

        private PlayerAnimationEvents _playerAnimationEvents;
        
        private Coroutine _shootCoroutine;
        private EnemiesZone _enemiesZone;
        private BulletsPool _bulletsPool;
        private bool _isShootingDisabled;//only for debug and test task

        public bool IsShooting { get; private set; }

        private void Awake()
        {
            _bulletsPool = new BulletsPool(actionConfig.BulletPrefab, actionConfig.BulletsPoolSize,container);
            _playerAnimationEvents = GetComponent<PlayerAnimationEvents>();
        }

        private void Start()
        {
            playerHpController.OnDead.AddListener(StopShoot);
            _playerAnimationEvents.OnShoot.AddListener(Shoot);
        }

        public void DisableShoot() => _isShootingDisabled = !_isShootingDisabled;
        
        public void OnEnterInZone(ActionZone zone)
        {
            if (zone is EnemiesZone enemiesZone)
            {
                _enemiesZone = enemiesZone;

                StartShoot();
               
            }
        }

        public void OnExitInZone(ActionZone type)
        {
            StopShoot();
        }

        private void StartShoot()
        {
            StopShoot();

            _shootCoroutine = StartCoroutine(ShootCoroutine());
            IsShooting = true;
        }

        private void StopShoot()
        {
            if (_shootCoroutine != null)
            {
                StopCoroutine(_shootCoroutine);
                _shootCoroutine = null;
            }

            IsShooting = false;
        }

        private void Shoot()
        {
            if(_isShootingDisabled)
                return;
            
            var bullet = _bulletsPool.GetObject();
            bullet.Init(_newarestEnemy.AimPoint,_bulletsPool,spawnPoint.position);
        }

        private bool GetNearestEnemy(out Enemy enemy)
        {
            enemy = null;
            
            if (!_enemiesZone.Enemies.Any())
                return false;
            
            enemy = _enemiesZone.Enemies.OrderBy(e => Vector3.Distance(e.transform.position, transform.position)).First();
            return enemy != null && enemy.IsAlive;
        }

        private void Update()
        {
            if(!IsShooting)
                return;
            
            if(!GetNearestEnemy(out var enemy))
                return;

            view.rotation = Quaternion.Lerp(view.rotation,
                ExtraMathf.GetRotation(transform.position, enemy.transform.position, Vector3.up), actionConfig.ViewRotateSpeed * Time.deltaTime);
        }

        private Enemy _newarestEnemy;
        
        private IEnumerator ShootCoroutine()
        {
            while (true)
            {
                if (!GetNearestEnemy(out var enemy))
                {
                    yield return new WaitForEndOfFrame();
                    continue;
                }

                _newarestEnemy = enemy;
                animator.SetTrigger("Shoot");
                
                yield return new WaitForSeconds(1f / actionConfig.ShootRate);
            }
        }
    }
}