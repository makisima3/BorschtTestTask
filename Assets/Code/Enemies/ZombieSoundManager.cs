using System;
using System.Collections;
using Code.Player.Configs;
using Plugins.RobyyUtils;
using UnityEngine;

namespace Code.Enemies
{
    [RequireComponent(typeof(AudioSource))]
    public class ZombieSoundManager : MonoBehaviour
    {
        private EnemyActionConfig _config;
        private AudioSource _audioSource;

        private bool _isWalkSoundReload;

        public void Init(EnemyActionConfig config)
        {
            _audioSource = GetComponent<AudioSource>();
            _config = config;
        }

        public void PlayAngrySound()
        {
            _audioSource.PlayOneShot(_config.AngrySounds.ChooseOne());
        }
        
        public void PlayWalkSound()
        {
            if(_isWalkSoundReload)
                return;
            
            _audioSource.PlayOneShot(_config.WalkingSounds.ChooseOne());
            StartCoroutine(Reload(_config.WalkSoundsRate, () => _isWalkSoundReload = false));

            _isWalkSoundReload = true;
        }
        
        public void PlayAttackSound()
        {
            _audioSource.PlayOneShot(_config.AttackSounds.ChooseOne());
        }
        
        public void PlayDamagedSound()
        {
            _audioSource.PlayOneShot(_config.DamagedSounds.ChooseOne());
        }

        private IEnumerator Reload(float time, Action onReload)
        {
            yield return new WaitForSeconds(time);
            
            onReload?.Invoke();
        }
    }
}