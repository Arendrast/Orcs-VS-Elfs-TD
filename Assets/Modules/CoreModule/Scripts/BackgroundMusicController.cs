using System.Collections;
using Modules.SharedModule.Scripts.Audio;
using UnityEngine;

namespace Modules.CoreModule.Scripts
{
    public class BackgroundMusicController
    {
        private readonly AudioService _audioService;
        private readonly MonoBehaviour _coroutineRunner;

        public BackgroundMusicController(AudioService audioService, MonoBehaviour coroutineRunner)
        {
            _audioService = audioService;
            _coroutineRunner = coroutineRunner;
        }

        public void PlaySounds()
        {
            _coroutineRunner.StartCoroutine(StartPlaySoundsCoroutine());
        }

        private IEnumerator StartPlaySoundsCoroutine()
        {
            _audioService.TryPlayOneShotForMainAudioSource(AudioId.BackgroundMusic, out var clip);

            if (clip == null)
            {
                yield break;
            }

            yield return new WaitForSeconds(clip.length);
            
            PlaySounds();
        }
    }
}