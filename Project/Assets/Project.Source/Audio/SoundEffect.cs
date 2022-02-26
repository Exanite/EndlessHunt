using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioSource source;

    private void Start()
    {
        source.Play();
    }

    private void Update()
    {
        if (!source.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}