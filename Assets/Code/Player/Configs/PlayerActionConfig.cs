using UnityEngine;

namespace Code.Player.Configs
{
    [CreateAssetMenu(fileName = "PlayerActionConfig", menuName = "ScriptableObjects/Player/PlayerActionConfig", order = 1)]
    public class PlayerActionConfig : ScriptableObject
    {
        [SerializeField] private float speed = 2f;
        [SerializeField] private float shootRate = 2f;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private int startHP = 2;
        [SerializeField] private int bulletsPoolSize;
        [SerializeField] private float viewRotateSpeed = 10;

        public float Speed => speed;
        
        public float ShootRate => shootRate;

        public Bullet BulletPrefab => bulletPrefab;

        public int StartHp => startHP;

        public int BulletsPoolSize => bulletsPoolSize;

        public float ViewRotateSpeed => viewRotateSpeed;
    }
}