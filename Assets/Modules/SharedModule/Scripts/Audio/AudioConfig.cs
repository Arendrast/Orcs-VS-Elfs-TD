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
            [field: SerializeField] public AudioClip[] Clips { get; private set; }
        }
        
        [field: SerializeField] public AudioClipsByAudioId[] ClipsById { get; private set; }
    }
}