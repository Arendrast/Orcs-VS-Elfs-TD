using System.Linq;
using UnityEngine;

namespace Modules.SharedModule.Scripts.Audio
{
    public class AudioService
    {
        private readonly AudioConfig _audioConfig;
        private readonly AudioSource _mainAudioSource;

        public AudioService(AudioConfig audioConfig, AudioSource mainAudioSource)
        {
            _audioConfig = audioConfig;
            _mainAudioSource = mainAudioSource;
        }

        public bool TryPlayOneShotForMainAudioSource(AudioId audioId)
        {
            return TryPlayOneShotForMainAudioSource(audioId, out var audioClip);
        }
        
        public bool TryPlayOneShotForMainAudioSource(AudioId audioId, out AudioClip audioClip)
        {
            return TryPlayOneShot(_mainAudioSource, audioId, out audioClip);
        }

        public bool TryPlayOneShot(AudioSource audioSource, AudioId audioId)
        {
            return TryPlayOneShot(audioSource, audioId, out var audioClip);
        }

        public bool TryPlayOneShot(AudioSource audioSource, AudioId audioId, out AudioClip audioClip)
        {
            audioClip = null;
            var clips = _audioConfig.ClipsById.FirstOrDefault(clips => clips.Id == audioId);

            if (clips == null || clips.Clips.Length == 0)
            {
                return false;
            }

            audioClip = clips.Clips[Random.Range(0, clips.Clips.Length)];
            audioSource.PlayOneShot(clips.Clips[Random.Range(0, clips.Clips.Length)]);
            return true;
        }
    }
}