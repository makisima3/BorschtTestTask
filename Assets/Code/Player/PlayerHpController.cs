using System;
using System.Collections;
using Code.Player.Configs;
using Code.Player.Data;
using Code.UI;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Code.Player
{
    public class PlayerHpController : MonoBehaviour
    {
        [Inject] private PlayerDataHolder playerDataHolder;
        [Inject] private RestartView restartView;

        [SerializeField] private HPBar hpBar;

        private Coroutine _armorRegenCoroutine;
        
        public bool IsAlive => playerDataHolder.PlayerData.CurrentHP > 0;
        public UnityEvent OnDead { get; private set; }

        private void Awake()
        {
            OnDead = new UnityEvent();
           
        }

        private void Start()
        {
            playerDataHolder.OnMaxHpChanged.AddListener(OnMaxHpCHanged);
            restartView.OnRestart.AddListener(() =>
            {
                playerDataHolder.PlayerData.CurrentHP = playerDataHolder.PlayerData.MaxHP;
                playerDataHolder.PlayerData.CurrentArmor = playerDataHolder.PlayerData.MaxArmor;
            });
        }

        private void OnMaxHpCHanged()
        {
            if (Math.Abs(playerDataHolder.PlayerData.CurrentHP - playerDataHolder.PlayerData.MaxHP) < 0.1)
            {
                playerDataHolder.PlayerData.CurrentHP = playerDataHolder.PlayerData.MaxHP;
            }

            UpdateHpBar();
        }

        private void UpdateHpBar()
        {
            hpBar.ShowHP(playerDataHolder.PlayerData.MaxHP, playerDataHolder.PlayerData.CurrentHP);
            hpBar.ShowArmor(playerDataHolder.PlayerData.MaxArmor, playerDataHolder.PlayerData.CurrentArmor);
        }
        

        public void Damage(float damage)
        {
            if (!IsAlive)
                return;

            playerDataHolder.PlayerData.CurrentArmor -= damage;

            if (playerDataHolder.PlayerData.CurrentArmor < 0)
            {
                playerDataHolder.PlayerData.CurrentHP -= Mathf.Abs(playerDataHolder.PlayerData.CurrentArmor);
                playerDataHolder.PlayerData.CurrentArmor = 0f;
            }

            UpdateHpBar();
            
            if (!IsAlive)
                Death();

            if (_armorRegenCoroutine != null)
            {
                StopCoroutine(_armorRegenCoroutine);
                _armorRegenCoroutine = null;
            }

            _armorRegenCoroutine = StartCoroutine(ArmorRegen());
        }

        public void KillMe()
        {
            Damage(playerDataHolder.PlayerData.CurrentHP + playerDataHolder.PlayerData.CurrentArmor);
        }
        
        private void Death()
        {
            OnDead.Invoke();
        }

        private IEnumerator ArmorRegen()
        {
            yield return new WaitForSeconds(playerDataHolder.PlayerData.ArmorStartRegenDelay);
            
            while (playerDataHolder.PlayerData.CurrentArmor < playerDataHolder.PlayerData.MaxArmor)
            {
                playerDataHolder.PlayerData.CurrentArmor += playerDataHolder.PlayerData.ArmorRegenPerTick;
                playerDataHolder.PlayerData.CurrentArmor = Mathf.Clamp(playerDataHolder.PlayerData.CurrentArmor, 0, playerDataHolder.PlayerData.MaxArmor);

                yield return new WaitForSeconds(1f / playerDataHolder.PlayerData.ArmorTickRate);
            }

            _armorRegenCoroutine = null;
        }
    }
}