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
        [Header("Params")]
        [SerializeField] private float hp = 2;
        [SerializeField] private float armor = 1;
        [SerializeField] private float speed = 2f;
        [SerializeField] private float rotateSpeed = 2f;
        [SerializeField] private float attackRate = 2f;
        [SerializeField] private float attackDistance = 2f;
        [SerializeField] private int damage = 1;
        [SerializeField, Range(0f, 1f)] private float dropCrystalChance = 0.1f;
        
        [Space, Header("Animations")]
        [SerializeField] private List<TypeToAnimation> typeToAnimations;
        [SerializeField] private List<Collectable> collectablesPrefab;

        [Space, Header("Sounds")]
        [SerializeField] private float walkSoundsRate = 10f;
        [SerializeField] private float distanceToWalkSound = 10f;
        [SerializeField] private List<AudioClip> attackSounds;
        [SerializeField] private List<AudioClip> angrySounds;
        [SerializeField] private List<AudioClip> damagedSounds;
        [SerializeField] private List<AudioClip> walkingSounds;

        #region Params
        
        public float Hp => hp;

        public float Armor => armor;

        public float Speed => speed;

        public float RotateSpeed => rotateSpeed;

        public float AttackRate => attackRate;
        
        public float AttackDistance => attackDistance;

        public int Damage => damage;

        public List<Collectable> CollectablesPrefab => collectablesPrefab;

        public float DropCrystalChance => dropCrystalChance;
        
        #endregion
        
        #region Sounds
        
        public List<AudioClip> AttackSounds => attackSounds;

        public List<AudioClip> AngrySounds => angrySounds;

        public List<AudioClip> DamagedSounds => damagedSounds;

        public List<AudioClip> WalkingSounds => walkingSounds;

        public float WalkSoundsRate => walkSoundsRate;

        public float DistanceToWalkSound => distanceToWalkSound;
        
        #endregion

        public string GetAnimation(EnemyStates state)
        {
            return typeToAnimations.First(tta => tta.EnemyState == state).AnimName;
        }
    }
}