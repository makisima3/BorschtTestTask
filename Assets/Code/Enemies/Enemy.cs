using System;
using Code.Collectables;
using Code.Enemies.Enums;
using Code.Player;
using Code.Player.Configs;
using Code.UI;
using Code.Zone;
using Code.Zone.Impls;
using Code.Zone.Interfaces;
using Plugins.RobyyUtils;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Random = UnityEngine.Random;

namespace Code.Enemies
{
    [RequireComponent(typeof(EnemyStateMachine))]
    public class Enemy : MonoBehaviour,IZoneChecker
    {
        [Inject] private PlayerHpController playerHpController;
        [Inject] private DiContainer container;
        
        [SerializeField] private Transform aimPoint;
        [SerializeField] private EnemyActionConfig enemyActionConfig;
        [SerializeField] private EnemyAnimationEvents enemyAnimationEvents;
        [SerializeField] private HPBar hpBar;

        private EnemyStateMachine _enemyStateMachine;
        private float hp;
        
        public bool IsAlive => hp > 0;

        public EnemyActionConfig EnemyActionConfig => enemyActionConfig;
        public Transform AimPoint => aimPoint;
        public Vector3 StartPoint { get; private set; }
        public UnityEvent<Enemy> OnDead { get; private set; }

        public void Init(Vector3 spawnPosition)
        {
            OnDead = new UnityEvent<Enemy>();
            _enemyStateMachine = GetComponent<EnemyStateMachine>();
            
            hp = enemyActionConfig.Hp;
            transform.position = spawnPosition;
            StartPoint = spawnPosition;
        }

        private void Start()
        {
            enemyAnimationEvents.OnHitEvent.AddListener(HitPlayer);
            enemyAnimationEvents.OnHitAnimationOverEvent.AddListener(OnHitAnimationEventOver);
            enemyAnimationEvents.OnDeathAnimationOverEvent.AddListener(OnDeathAnimationEventOver);
            
            playerHpController.OnDead.AddListener(BackToStartPosition);            
        }

        public void AttackPlayer()
        {
            _enemyStateMachine.CurrentState = EnemyStates.Attack;
        }
        
        public void BackToStartPosition()
        {
            _enemyStateMachine.CurrentState = EnemyStates.GoingBack;
        }

        public void Damage(float damage)
        {
            if (!IsAlive)
                return;
            
            hp -= damage;
            hpBar.ShowHP(enemyActionConfig.Hp, hp);
            
            if (!IsAlive)
                Death();
        }

        private void HitPlayer()
        {
            if (Vector3.Distance(playerHpController.transform.position, transform.position) <= enemyActionConfig.AttackDistance)
                playerHpController.Damage(enemyActionConfig.Damage);
            else
                _enemyStateMachine.CurrentState = EnemyStates.Attack;
        }

        private void OnHitAnimationEventOver()
        {
            _enemyStateMachine.CurrentState = EnemyStates.Attack;
        }
        private void OnDeathAnimationEventOver()
        {
            Destroy(gameObject);
        }
        
        private void Death()
        {
            OnDead.Invoke(this);
            _enemyStateMachine.CurrentState = EnemyStates.Death;

            SpawnCollectableWithChance();
        }

        private void SpawnCollectableWithChance()
        {
            if ((Random.Range(0f, 1f) > enemyActionConfig.DropCrystalChance))
                return;

            var prefab = enemyActionConfig.CollectablesPrefab.ChooseOne();
            var collectable = container.InstantiatePrefabForComponent<Collectable>(prefab);
            collectable.transform.position = transform.position;
        }

        public void OnEnterInZone(ActionZone zone)
        {
            
        }

        public void OnExitInZone(ActionZone zone)
        {
            if (zone is EnemiesZone)
            {
                BackToStartPosition();
            }
        }
    }
}