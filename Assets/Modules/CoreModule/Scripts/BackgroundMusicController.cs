using System.Collections;
using Modules.SharedModule.Scripts.Audio;
using UnityEngine;

namespace Modules.CoreModule.Scripts
{
    public class BackgroundMusicController
    {
        private Coroutine _coroutine;
        
        private readonly AudioService _audioService;
        private readonly MonoBehaviour _coroutineRunner;

        public BackgroundMusicController(AudioService audioService, MonoBehaviour coroutineRunner)
        {
            _audioService = audioService;
            _coroutineRunner = coroutineRunner;
        }

        public void PlaySounds()
        {
            if (_coroutine != null)
            {
                _coroutineRunner.StopCoroutine(_coroutine);
            }
            
            _coroutine = _coroutineRunner.StartCoroutine(StartPlaySoundsCoroutine());
        }

        private IEnumerator StartPlaySoundsCoroutine()
        {
            _audioService.StopBackgroundMusicAudioSource();
            
            if (!_audioService.TryPlayOneShotForBackgroundMusicAudioSource(AudioId.BackgroundMusic, out var clip))
            {
                yield break;
            }
            
            yield return new WaitForSecondsRealtime(clip.length);
            
            PlaySounds();
        }
    }
}