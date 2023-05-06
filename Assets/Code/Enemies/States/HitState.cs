using System.Collections;
using System.Collections.Generic;
using Code.Enemies.Enums;
using Code.Player;
using Code.StateMachine;
using Plugins.MyUtils;
using UnityEngine;
using Zenject;

namespace Code.Enemies.States
{
    [RequireComponent(typeof(CharacterController))]
    public class HitState : MonoBehaviour,IState<EnemyStates>
    {
        [Inject] private PlayerHpController hpController;
        
        [SerializeField] private Animator animator;
        
        private Enemy _enemy;
        private CharacterController _characterController;
        private EnemyStateMachine _enemyStateMachine;
            
        public EnemyStates Type => EnemyStates.Hit;

        public void OnEnter()
        {
            if (_enemy == null)
                _enemy = GetComponent<Enemy>();
            if (_characterController == null)
                _characterController = GetComponent<CharacterController>();
            if (_enemyStateMachine == null)
                _enemyStateMachine = GetComponent<EnemyStateMachine>();

            animator.SetTrigger(_enemy.EnemyActionConfig.GetAnimation(Type));
        }

        public void OnExit()
        {
            
        }

        public void Loop()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, 
                ExtraMathf.GetRotation(transform.position, hpController.transform.position,Vector3.up), _enemy.EnemyActionConfig.RotateSpeed);
        }
    }
}