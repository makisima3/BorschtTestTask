using System;
using Code.Player.Configs;
using Code.UI;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Code.Player
{
    public class PlayerHpController : MonoBehaviour
    {
        [Inject] private PlayerActionConfig playerActionConfig;
        [Inject] private RestartView restartView;

        [SerializeField] private HPBar hpBar;
        
        private int _hp;
        public bool IsAlive => _hp > 0;
        public UnityEvent OnDead { get; private set; }

        private void Awake()
        {
            OnDead = new UnityEvent();
            _hp = playerActionConfig.StartHp;
        }

        private void Start()
        {
            restartView.OnRestart.AddListener(() =>
            {
                _hp = playerActionConfig.StartHp;
                hpBar.ShowHP(playerActionConfig.StartHp, _hp);
            });
        }

        public void Damage(int damage)
        {
            if (!IsAlive)
                return;
            
            _hp-= damage;
            hpBar.ShowHP(playerActionConfig.StartHp, _hp);
            
            if (!IsAlive)
                Death();
        }

        public void KillMe()
        {
            Damage(_hp);
        }
        
        private void Death()
        {
            OnDead.Invoke();
        }
    }
}