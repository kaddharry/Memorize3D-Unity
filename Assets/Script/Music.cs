using UnityEngine;

public class Music : MonoBehaviour
{
    public static Music Instance { get; private set; }

    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _audioSource = GetComponent<AudioSource>();
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }

    public void SetVolume(float value)
    {
        _audioSource.volume = Mathf.Clamp01(value);
    }
}
