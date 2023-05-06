using Code.Enemies;
using UnityEngine;

namespace Code.Player.Configs
{
    [CreateAssetMenu(fileName = "ZoneActionConfig", menuName = "ScriptableObjects/Zones/ZoneActionConfig", order = 1)]
    public class ZoneActionConfig : ScriptableObject
    {
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private int maxEnemies;
        [SerializeField] private float spawnRate;

        public Enemy EnemyPrefab => enemyPrefab;

        public int MAXEnemies => maxEnemies;

        public float SpawnRate => spawnRate;
    }
}