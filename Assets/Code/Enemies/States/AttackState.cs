using Code.Enemies.Enums;
using Code.Player;
using Code.StateMachine;
using Plugins.MyUtils;
using UnityEngine;
using Zenject;

namespace Code.Enemies.States
{
    [RequireComponent(typeof(CharacterController))]
    public class AttackState : MonoBehaviour,IState<EnemyStates>
    {
        [Inject] private PlayerController playerController;

        [SerializeField] private Animator animator;
        [SerializeField] private ZombieSoundManager zombieSoundManager;
        
        private Enemy _enemy;
        private CharacterController _characterController;
        private EnemyStateMachine _enemyStateMachine;
            
        public EnemyStates Type => EnemyStates.Attack;

        public void OnEnter()
        {
            if (_enemy == null)
                _enemy = GetComponent<Enemy>();
            if (_characterController == null)
                _characterController = GetComponent<CharacterController>();
            if (_enemyStateMachine == null)
                _enemyStateMachine = GetComponent<EnemyStateMachine>();
            
            animator.SetTrigger(_enemy.EnemyActionConfig.GetAnimation(Type));
            
            zombieSoundManager.PlayAngrySound();
        }

        public void OnExit()
        {
            
        }

        public void Loop()
        {
            Rotate();
            Move();

            if (Vector3.Distance(playerController.transform.position, transform.position) <= _enemy.EnemyActionConfig.AttackDistance)
                _enemyStateMachine.CurrentState = EnemyStates.Hit;
        }

        private void Rotate()
        {
            var direction = ExtraMathf.GetDirection(transform.position, playerController.transform.position);
            
            transform.rotation = Quaternion.Lerp(transform.rotation, 
                ExtraMathf.GetRotation(direction, Vector3.up), 
                _enemy.EnemyActionConfig.RotateSpeed * Time.deltaTime);
        }

        private void Move()
        {
            _characterController.Move(transform.forward * _enemy.EnemyActionConfig.Speed * Time.deltaTime);
        }
    }
}