using System;
using UnityEngine;

namespace Modules.SharedModule.Scripts.Audio
{
    [CreateAssetMenu(fileName = nameof(AudioConfig), menuName = "Configs/" + nameof(AudioConfig))]
    public class AudioConfig : ScriptableObject
    {
        [Serializable]
        public class AudioClipsByAudioId
        {
            [field: SerializeField] public AudioId Id { get; private set; }
            [field: SerializeField] public AudioClipByVolume[] Clips { get; private set; }
        }

        [Serializable]
        public class AudioClipByVolume
        {
            [field: SerializeField] public AudioClip Clip { get; private set; }
            [field: SerializeField] [field: Range(0, 1f)] public float Volume { get; private set; } = 1f;
        }
        
        [field: SerializeField] public AudioClipsByAudioId[] ClipsById { get; private set; }
    }
}