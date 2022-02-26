using UnityEngine;

namespace Project.Source
{
    public class SoundManager : SingletonBehaviour<SoundManager>
    {
        public Camera mainCamera;
        public SoundEffect soundPrefab;

        public SoundEffect PlaySound(AudioClip clip, Vector3 position, float volume = 1)
        {
            Debug.Log($"Playing {clip}");

            var sound = Instantiate(soundPrefab);
            sound.source.clip = clip;
            sound.source.volume = volume;
            
            sound.transform.position = position;

            return sound;
        }
    }
}