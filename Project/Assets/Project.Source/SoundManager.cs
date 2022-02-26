using UnityEngine;

namespace Project.Source
{
    public class SoundManager : SingletonBehaviour<SoundManager>
    {
        public Camera mainCamera;
        public AudioSource soundPrefab;

        public AudioSource PlaySound(AudioClip clip, Vector3 position, float volume = 1)
        {
            Debug.Log($"Playing {clip}");

            var sound = Instantiate(soundPrefab);
            sound.clip = clip;
            sound.transform.position = position;
            sound.volume = volume;

            return sound;
        }
    }
}