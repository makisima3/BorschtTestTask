using System;
using System.Collections.Generic;
using System.Linq;
using Code.Collectables;
using Code.Enemies.Enums;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Player.Configs
{
    [Serializable]
    public class TypeToAnimation
    {
        public EnemyStates EnemyState;
        public string AnimName;

    }
    
    [CreateAssetMenu(fileName = "EnemyActionConfig", menuName = "ScriptableObjects/Enemies/EnemyActionConfig", order = 1)]
    public class EnemyActionConfig : ScriptableObject
    {
        [SerializeField] private float hp = 2;
        [SerializeField] private float armor = 1;
        [SerializeField] private float speed = 2f;
        [SerializeField] private float rotateSpeed = 2f;
        [SerializeField] private float attackRate = 2f;
        [SerializeField] private float attackDistance = 2f;
        [SerializeField] private int damage = 1;
        [SerializeField] private List<TypeToAnimation> typeToAnimations;
        [SerializeField] private List<Collectable> collectablesPrefab;
        [SerializeField, Range(0f, 1f)] private float dropCrystalChance = 0.1f;

        public float Hp => hp;

        public float Armor => armor;

        public float Speed => speed;

        public float RotateSpeed => rotateSpeed;

        public float AttackRate => attackRate;
        
        public float AttackDistance => attackDistance;

        public int Damage => damage;

        public List<Collectable> CollectablesPrefab => collectablesPrefab;

        public float DropCrystalChance => dropCrystalChance;

        public string GetAnimation(EnemyStates state)
        {
            return typeToAnimations.First(tta => tta.EnemyState == state).AnimName;
        }
    }
}