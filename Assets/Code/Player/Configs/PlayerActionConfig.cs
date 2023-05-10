using UnityEngine;

namespace Code.Player.Configs
{
    [CreateAssetMenu(fileName = "PlayerActionConfig", menuName = "ScriptableObjects/Player/PlayerActionConfig", order = 1)]
    public class PlayerActionConfig : ScriptableObject
    {
        [SerializeField] private float speed = 2f;
        [SerializeField] private int startHP = 2;
        [SerializeField] private int bulletsPoolSize;
        [SerializeField] private float viewRotateSpeed = 10;

        public float Speed => speed;

        public int StartHp => startHP;

        public int BulletsPoolSize => bulletsPoolSize;

        public float ViewRotateSpeed => viewRotateSpeed;
    }
}