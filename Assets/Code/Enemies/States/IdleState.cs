using Code.Enemies.Enums;
using Code.Player;
using Code.StateMachine;
using UnityEngine;
using Zenject;

namespace Code.Enemies.States
{
    public class IdleState : MonoBehaviour, IState<EnemyStates>
    {
        [Inject] private PlayerController playerController;
        
        [SerializeField] private Animator animator;
        [SerializeField] private ZombieSoundManager zombieSoundManager;
        
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
            var distance = Vector3.Distance(playerController.transform.position, transform.position); 
            
           if(distance <= _enemy.EnemyActionConfig.DistanceToWalkSound)
               zombieSoundManager.PlayWalkSound();
           
           if(distance <= _enemy.EnemyActionConfig.DistanceToAggressive)
               _enemy.AttackPlayer();
        }
    }
}