using Code.Enemies.Enums;
using Code.StateMachine;
using Plugins.MyUtils;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Enemies.States
{
    public class GoingBackState : MonoBehaviour, IState<EnemyStates>
    {
        private Enemy _enemy;
        private CharacterController _characterController;
        private EnemyStateMachine _enemyStateMachine;
        private NavMeshAgent _navMeshAgent;

        [SerializeField] private Animator animator;
        
        public EnemyStates Type => EnemyStates.GoingBack;
        
        public void OnEnter()
        {
            if (_enemy == null)
                _enemy = GetComponent<Enemy>();
            
            if (_characterController == null)
                _characterController = GetComponent<CharacterController>();
            
            if (_enemyStateMachine == null)
                _enemyStateMachine = GetComponent<EnemyStateMachine>();
            
            if (_navMeshAgent == null)
                _navMeshAgent = GetComponent<NavMeshAgent>();

            if (Vector3.Distance(transform.position, _enemy.StartPoint) <= _enemy.EnemyActionConfig.AttackDistance)
                _enemyStateMachine.CurrentState = EnemyStates.Idle;
            else
                animator.SetTrigger(_enemy.EnemyActionConfig.GetAnimation(Type));

            _navMeshAgent.isStopped = false;
        }

        public void OnExit()
        {
            _navMeshAgent.isStopped = true;
        }

        public void Loop()
        {
            //Rotate();
            Move();

            if (Vector3.Distance(transform.position, _enemy.StartPoint) <= _enemy.EnemyActionConfig.AttackDistance)
                _enemyStateMachine.CurrentState = EnemyStates.Idle;
        }

        private void Rotate()
        {
            var direction = ExtraMathf.GetDirection(transform.position, _enemy.StartPoint);
            
            transform.rotation = Quaternion.Lerp(transform.rotation, 
                ExtraMathf.GetRotation(direction, Vector3.up), 
                _enemy.EnemyActionConfig.RotateSpeed * Time.deltaTime);
        }

        private void Move()
        {
            //_characterController.Move(transform.forward * _enemy.EnemyActionConfig.Speed * Time.deltaTime);
            _navMeshAgent.SetDestination(_enemy.StartPoint);
        }
    }
}