using System.Linq;
using UnityEngine;

namespace Modules.SharedModule.Scripts.Audio
{
    public class AudioService
    {
        private readonly AudioConfig _audioConfig;
        private readonly AudioSource _mainAudioSource;
        private readonly AudioSource _musicAudioSource;

        public AudioService(AudioConfig audioConfig, AudioSource mainAudioSource, AudioSource musicAudioSource)
        {
            _audioConfig = audioConfig;
            _mainAudioSource = mainAudioSource;
            _musicAudioSource = musicAudioSource;
        }

        public bool TryPlayOneShotForMainAudioSource(AudioId audioId)
        {
            return TryPlayOneShot(_mainAudioSource, audioId);
        }
        
        public void StopBackgroundMusicAudioSource()
        {
            _musicAudioSource.Stop();
        }
        
        public bool TryPlayOneShotForBackgroundMusicAudioSource(AudioId audioId, out AudioClip audioClip)
        {
            return TryPlayOneShot(_musicAudioSource, audioId, out audioClip);
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

            var audioClipByVolume = clips.Clips[Random.Range(0, clips.Clips.Length)];
            audioClip = audioClipByVolume.Clip;
            audioSource.PlayOneShot(audioClipByVolume.Clip, audioClipByVolume.Volume);
            return true;
        }
    }
}