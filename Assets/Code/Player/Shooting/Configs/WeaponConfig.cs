using Code.Player.Enums;
using UnityEditor;
using UnityEngine;

namespace Code.Player.Shooting.Configs
{
    [CreateAssetMenu(fileName = "WeaponConfigConfig", menuName = "ScriptableObjects/Player/Weapons/WeaponConfigConfig", order = 1)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] private WeaponType type;
        [SerializeField] private Sprite icon;
        [SerializeField] private int ammoReserve = 30;
        [SerializeField] private int ammo = 10;
        [SerializeField] private int damage = 1;
        [SerializeField] private float shootRate = 1;
        [SerializeField] private PlayerAnimationType reloadAnimation;
        [SerializeField] private PlayerAnimationType shootAnimation;
        [SerializeField] private Bullet bulletPrefab;
        [Header("Burst")]
        [SerializeField] private bool isBurst;
        [ConditionalHide("isBurst", true)]
        [SerializeField] private int burstCount;
        [ConditionalHide("isBurst", true)]
        [SerializeField] private float burstDelay;

        public WeaponType Type => type;

        public Sprite Icon => icon;

        public int AmmoReserve => ammoReserve;

        public int Ammo => ammo;

        public int Damage => damage;

        public float ShootRate => shootRate;

        public PlayerAnimationType ReloadAnimation => reloadAnimation;

        public PlayerAnimationType ShootAnimation => shootAnimation;

        public Bullet BulletPrefab => bulletPrefab;

        public bool IsBurst => isBurst;

        public int BurstCount => burstCount;

        public float BurstDelay => burstDelay;

    }
}