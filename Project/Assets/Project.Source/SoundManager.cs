using UnityEngine;

namespace Project.Source
{
    public class SoundManager : SingletonBehaviour<SoundManager>
    {
        public AudioSource soundPrefab;

        public void PlaySound(Vector3 position, AudioClip clip)
        {
            Debug.Log($"Playing {clip} at {position}");
        }
    }
}