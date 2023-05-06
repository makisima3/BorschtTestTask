using Code.Enemies.Enums;
using Code.Enemies.States;
using Code.StateMachine;
using UnityEngine;

namespace Code.Enemies
{
    [RequireComponent(typeof(IdleState))]
    [RequireComponent(typeof(AttackState))]
    [RequireComponent(typeof(DeathState))]
    [RequireComponent(typeof(GoingBackState))]
    [RequireComponent(typeof(HitState))]
    public class EnemyStateMachine : StateMachine<EnemyStates>
    {
        
    }
}