using Code.Enemies.Enums;
using Code.StateMachine;
using UnityEngine;

namespace Code.Enemies.States
{
    public class DeathState : MonoBehaviour, IState<EnemyStates>
    {
        [SerializeField] private Animator animator;

        private Enemy _enemy;
        private CharacterController _characterController;
        
        public EnemyStates Type => EnemyStates.Death;
        
        public void OnEnter()
        {
            if (_enemy == null)
                _enemy = GetComponent<Enemy>();
            if (_characterController == null)
                _characterController = GetComponent<CharacterController>();
            
            animator.SetTrigger(_enemy.EnemyActionConfig.GetAnimation(Type));
            _characterController.enabled = false;
        }

        public void OnExit()
        {
            
        }

        public void Loop()
        {
            
        }
    }
}