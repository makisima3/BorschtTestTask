using Code.Player.Data;
using UnityEngine;

namespace Code.Player.Configs
{
    [CreateAssetMenu(fileName = "PlayerDataConfig", menuName = "ScriptableObjects/Player/PlayerDataConfig", order = 1)]
    public class PlayerDataConfig : ScriptableObject
    {
        [SerializeField] private PlayerData playerData;

        public PlayerData PlayerData => playerData;
    }
}