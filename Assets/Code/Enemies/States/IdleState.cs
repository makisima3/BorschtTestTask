using Code.Enemies.Enums;
using Code.StateMachine;
using UnityEngine;

namespace Code.Enemies.States
{
    public class IdleState : MonoBehaviour, IState<EnemyStates>
    {
        [SerializeField] private Animator animator;
        
        private Enemy _enemy;
        
        public EnemyStates Type => EnemyStates.Idle;
        
        public void OnEnter()
        {
            if (_enemy == null)
                _enemy = GetComponent<Enemy>();
            
            animator.SetTrigger(_enemy.EnemyActionConfig.GetAnimation(Type));
        }

        public void OnExit()
        {
          
        }

        public void Loop()
        {
           
        }
    }
}