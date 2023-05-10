using System;
using System.Collections.Generic;
using System.Linq;
using Code.Player.Data;
using Code.Player.Enums;
using Plugins.RobyyUtils;
using UnityEngine;

namespace Code.Player.Configs
{
    [Serializable]
    public class TypeToPlayerAnimation
    {
        public PlayerAnimationType Type;
        public string Key;
    }
    
    [CreateAssetMenu(fileName = "PlayerAnimationsConfig", menuName = "ScriptableObjects/Player/PlayerAnimationsConfig", order = 1)]
    public class PlayerAnimationsConfig : ScriptableObject
    {
        [SerializeField] private List<TypeToPlayerAnimation> typeToPlayerAnimations;

        public string GetAnimationKey(PlayerAnimationType type)
        {
            return typeToPlayerAnimations.Where(ttpa => ttpa.Type == type).ChooseOne().Key;
        }
    }
}