using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public float BGMVolume { get; private set; } = 1f;
    public float SFXVolume { get; private set; } = 1f;

    private const string PrefBGM = "vol_bgm";
    private const string PrefSFX = "vol_sfx";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        if (FindObjectOfType<SoundManager>() != null) return;
        var go = new GameObject("SoundManager");
        DontDestroyOnLoad(go);
        go.AddComponent<SoundManager>();
    }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        BGMVolume = PlayerPrefs.GetFloat(PrefBGM, 1f);
        SFXVolume = PlayerPrefs.GetFloat(PrefSFX, 1f);
        ApplyBGMVolume();
    }

    public void SetBGMVolume(float value)
    {
        BGMVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(PrefBGM, BGMVolume);
        PlayerPrefs.Save();
        ApplyBGMVolume();
    }

    public void SetSFXVolume(float value)
    {
        SFXVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(PrefSFX, SFXVolume);
        PlayerPrefs.Save();
    }

    // Reproduce un clip puntual respetando el volumen SFX configurado
    public void PlaySFX(AudioSource source, AudioClip clip = null)
    {
        if (source == null) return;
        if (clip != null)
            source.PlayOneShot(clip, SFXVolume);
        else
        {
            source.volume = SFXVolume;
            source.Play();
        }
    }

    private void ApplyBGMVolume()
    {
        if (Music.Instance != null)
            Music.Instance.SetVolume(BGMVolume);
    }
}
