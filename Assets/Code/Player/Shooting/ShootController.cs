using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Enemies;
using Code.Player.Configs;
using Code.Player.Data;
using Code.Player.Enums;
using Code.Player.Shooting.Configs;
using Code.Zone;
using Code.Zone.Impls;
using Code.Zone.Interfaces;
using Plugins.MyUtils;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Zenject.SpaceFighter;

namespace Code.Player.Shooting
{
    [RequireComponent(typeof(PlayerAnimationEvents))]
    public class ShootController : MonoBehaviour, IZoneChecker
    {
        [Inject] private DiContainer container;
        [Inject] private PlayerActionConfig actionConfig;
        [Inject] private PlayerHpController playerHpController;
        [Inject] private SoundsManager soundsManager;
        [Inject] private PlayerDataHolder playerDataHolder;
        [Inject] private WeaponsConfig weaponsConfig;
        [Inject] private PlayerAnimationsConfig playerAnimationsConfig;
        [Inject] private WeaponViewChanger weaponViewChanger;

        [SerializeField] private Transform view;
        [SerializeField] private Transform bulletsHolder;
        [SerializeField] private Animator animator;

        private PlayerAnimationEvents _playerAnimationEvents;
        
        private Coroutine _shootCoroutine;
        private EnemiesZone _enemiesZone;
        private Enemy _newarestEnemy;
        private bool _isShootingDisabled;//only for debug and test task
        private WeaponConfig _currentWeapon;
        private WeaponData _currentWeaponData;
        private bool _isOnReload;

        private Dictionary<WeaponType, BulletsPool> _typeToPool;

        public bool IsShooting { get; private set; }
        public UnityEvent<WeaponData> OnBulletsCountChanged { get; private set; }

        private void Awake()
        {
            OnBulletsCountChanged = new UnityEvent<WeaponData>();
            
            _typeToPool = new Dictionary<WeaponType, BulletsPool>();

            foreach (WeaponType weaponType in Enum.GetValues(typeof(WeaponType)))
            {
                var holder = new GameObject();
                holder.transform.SetParent(bulletsHolder);
                holder.name = $"{weaponType}BulletsHolder";
                _typeToPool.Add(weaponType,new BulletsPool(weaponsConfig.GetWeaponConfig(weaponType).BulletPrefab, actionConfig.BulletsPoolSize,container, holder.transform));
            }
            
            _playerAnimationEvents = GetComponent<PlayerAnimationEvents>();
        }

        private void Start()
        {
            playerHpController.OnDead.AddListener(StopShoot);
            _playerAnimationEvents.OnShoot.AddListener(Shoot);
            _playerAnimationEvents.OnReload.AddListener(Reload);
            playerDataHolder.OnWeaponChanged.AddListener(ChangeWeapon);
            
            ChangeWeapon(playerDataHolder.PlayerData.CurrentWeapon);
            OnBulletsCountChanged.Invoke(_currentWeaponData);
        }

        public void DisableShoot() => _isShootingDisabled = !_isShootingDisabled;
        
        public void OnEnterInZone(ActionZone zone)
        {
            if (zone is EnemiesZone enemiesZone)
            {
                _enemiesZone = enemiesZone;

                StartShoot();
               return;
            }

            if (zone is PlayerBaseZone)
            {
               playerDataHolder.PlayerData.ResetAllWeaponsAmmo(weaponsConfig);
               OnBulletsCountChanged.Invoke(_currentWeaponData);
            }
        }

        public void OnExitInZone(ActionZone type)
        {
            StopShoot();
        }

        private void ChangeWeapon(WeaponType weaponType)
        {
            _currentWeapon = weaponsConfig.GetWeaponConfig(weaponType);
            _currentWeaponData = playerDataHolder.PlayerData.GetWeaponData(_currentWeapon);
            
            OnBulletsCountChanged.Invoke(_currentWeaponData);
            
            if(IsShooting)
                StartCoroutine(RestartShoot());
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
        
        

        private void Reload()
        {
            var ammoCount = _currentWeapon.Ammo - _currentWeaponData.Ammo;
            if (_currentWeaponData.AmmoReserve < ammoCount)
                ammoCount = _currentWeaponData.AmmoReserve;

            _currentWeaponData.AmmoReserve -= ammoCount;
            _currentWeaponData.Ammo = ammoCount;
            _isOnReload = false;
            
            OnBulletsCountChanged.Invoke(_currentWeaponData);
        }
        
        private void Shoot()
        {
            if(_isShootingDisabled)
                return;

            if(_currentWeaponData.Ammo <= 0)
                return;
            
            _currentWeaponData.Ammo--;
            soundsManager.PlayShootSound(_currentWeaponData.Type);
            var pool = _typeToPool[_currentWeaponData.Type];
            var bullet = pool.GetObject();
            bullet.Init(_newarestEnemy.AimPoint,pool,weaponViewChanger.SpawnPoint.position, _currentWeapon.Damage + playerDataHolder.PlayerData.BaseDamage);
            OnBulletsCountChanged.Invoke(_currentWeaponData);
        }

        private bool GetNearestEnemy(out Enemy enemy)
        {
            enemy = null;
            
            if (!_enemiesZone.Enemies.Any())
                return false;
            
            enemy = _enemiesZone.Enemies.OrderBy(e => Vector3.Distance(e.transform.position, transform.position)).First();
            var distance = Vector3.Distance(enemy.transform.position, transform.position);
            return enemy != null && enemy.IsAlive && distance <= _currentWeapon.Distance;
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

        private IEnumerator RestartShoot()
        {
            StopShoot();

            yield return new WaitForEndOfFrame();
            
            StartShoot();
        }
        
        private IEnumerator ShootCoroutine()
        {
            var burstCounter = 0;
            
            while (true)
            {
                #region Reload
                    if (_currentWeaponData.Ammo <= 0 && _currentWeaponData.AmmoReserve > 0)
                    {
                        _isOnReload = true;
                        animator.SetTrigger(playerAnimationsConfig.GetAnimationKey(_currentWeapon.ReloadAnimation));
                        yield return new WaitWhile(() => _isOnReload);
                    }

                    if (_currentWeaponData.Ammo <= 0 && _currentWeaponData.AmmoReserve <= 0)
                    {
                        yield return new WaitWhile(() => _currentWeaponData.AmmoReserve <= 0);
                        continue;
                    }

                    if (_currentWeaponData.Ammo <= 0)
                    {
                        Debug.Log("_currentWeaponData.Ammo == 0");
                        continue;
                    }
                #endregion

                if (!GetNearestEnemy(out var enemy))
                {
                    yield return new WaitForEndOfFrame();
                    continue;
                }
                
                _newarestEnemy = enemy;
                
                animator.SetTrigger(playerAnimationsConfig.GetAnimationKey(_currentWeapon.ShootAnimation));

                #region Burst

                if (_currentWeapon.IsBurst)
                {
                    burstCounter++;

                    if (burstCounter >= _currentWeapon.BurstCount)
                    {
                        burstCounter = 0;

                        yield return new WaitForSeconds(_currentWeapon.BurstDelay);
                    }
                }

                #endregion
                
                yield return new WaitForSeconds(1f / _currentWeapon.ShootRate);
            }
        }
    }
}