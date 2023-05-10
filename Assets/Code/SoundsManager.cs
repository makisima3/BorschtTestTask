using System;
using System.Collections;
using System.Collections.Generic;
using Code.Player.Enums;
using DG.Tweening;
using Plugins.RobyyUtils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Player
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundsManager : MonoBehaviour
    {
        [SerializeField] private AudioClip[] akClips;
        [SerializeField] private AudioClip[] famasClips;
        [SerializeField] private AudioClip[] deathClips;
        [SerializeField] private AudioClip[] menuBackClips;
        [SerializeField] private AudioClip[] gameBackClips;
        [SerializeField] private AudioClip[] winBackClips;
        [SerializeField] private AudioClip[] loseBackClips;
        [SerializeField] private float maxVolume = 0.5f;
        [SerializeField] private float soundTransactionTime = 0.3f;

        private AudioSource _audioSource;
        private Coroutine _backMusicAwaiter;
        private Tween _volumeTween;
        private bool _isMenuBackPlaying;

        private Dictionary<WeaponType, AudioClip[]> _typeToShootSounds;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            _typeToShootSounds = new Dictionary<WeaponType, AudioClip[]>()
            {
                { WeaponType.AK, akClips },
                { WeaponType.Famas, famasClips },
            };
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            _audioSource.mute = !hasFocus;
        }

        public void PlayShootSound(WeaponType type)
        {
            _audioSource.PlayOneShot(_typeToShootSounds[type].ChooseOne());
        }

        public void PlayDeathSound()
        {
            _audioSource.PlayOneShot(deathClips.ChooseOne());
        }

        public void PlayWinSound()
        {
            StopPlayMusic().OnComplete(() =>
            {
                _audioSource.Stop();
                _volumeTween = DOTween
                    .To(SoundVolumeGetter, SoundVolumeSetter, maxVolume, soundTransactionTime);
                
                //_audioSource.PlayOneShot(winBackClips.ChooseOne());
                _audioSource.clip = winBackClips.ChooseOne();
                _audioSource.Play();
            });
        }
        
        public void PlayLoseSound()
        {
            StopPlayMusic().OnComplete(() =>
            {
                _audioSource.Stop();
                _volumeTween = DOTween
                    .To(SoundVolumeGetter, SoundVolumeSetter, maxVolume, soundTransactionTime);
                
                //_audioSource.PlayOneShot(loseBackClips.ChooseOne());
                _audioSource.clip = loseBackClips.ChooseOne();
                _audioSource.Play();
            });
        }
        
        public void StartPlayGameBackMusic()
        {
            _isMenuBackPlaying = false;
            StopPlayMusic().OnComplete(() =>
            {
                _volumeTween = DOTween
                    .To(SoundVolumeGetter, SoundVolumeSetter, maxVolume, soundTransactionTime);
                PlayGameBackMusic();
            });
        }

        public void StartPlayMenuBackMusic()
        {
            _isMenuBackPlaying = true;
            StopPlayMusic().OnComplete(() =>
            {
                _volumeTween = DOTween
                    .To(SoundVolumeGetter, SoundVolumeSetter, maxVolume, soundTransactionTime);
                PlayMenuBackMusic();
            });;
        }

        private void PlayGameBackMusic()
        {
            if(_isMenuBackPlaying)
                return;
            
            var clip = gameBackClips.ChooseOne();

            _audioSource.clip = clip;
            _audioSource.Play();
            _backMusicAwaiter = StartCoroutine(BackMusicAwaiter(clip.length, PlayGameBackMusic));
        }

        private void PlayMenuBackMusic()
        {
            if(!_isMenuBackPlaying)
                return;
            
            var clip = menuBackClips.ChooseOne();

            _audioSource.clip = clip;
            _audioSource.Play();
            _backMusicAwaiter = StartCoroutine(BackMusicAwaiter(clip.length, PlayMenuBackMusic));
        }

        private Tween StopPlayMusic()
        {
            _volumeTween.Kill();
            if (_backMusicAwaiter != null)
            {
                StopCoroutine(_backMusicAwaiter);
                _backMusicAwaiter = null;
            }

            return _volumeTween = DOTween
                .To(SoundVolumeGetter, SoundVolumeSetter, 0f, soundTransactionTime);
        }

        private IEnumerator BackMusicAwaiter(float delay, Action onMusicEnd)
        {
            yield return new WaitForSeconds(delay);
            onMusicEnd?.Invoke();
        }

        private void SoundVolumeSetter(float value) => _audioSource.volume = value;
        private float SoundVolumeGetter() => _audioSource.volume;
    }
}