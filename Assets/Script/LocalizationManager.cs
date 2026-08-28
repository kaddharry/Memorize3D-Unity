using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }
    public static event Action OnLanguageChanged;

    public string CurrentLanguage { get; private set; } = "en";

    private readonly Dictionary<string, string> _strings = new Dictionary<string, string>();
    private const string PrefKey = "app_language";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        if (FindObjectOfType<LocalizationManager>() != null) return;
        var go = new GameObject("LocalizationManager");
        DontDestroyOnLoad(go);
        go.AddComponent<LocalizationManager>();
    }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SetLanguage(PlayerPrefs.GetString(PrefKey, "en"));
    }

    public static string Get(string key, params object[] args)
    {
        if (Instance == null || !Instance._strings.TryGetValue(key, out string value))
            return key;
        return args.Length > 0 ? string.Format(value, args) : value;
    }

    public void SetLanguage(string lang)
    {
        var asset = Resources.Load<TextAsset>($"Localization/{lang}");
        if (asset == null)
        {
            Debug.LogWarning($"[Localization] Missing file: Localization/{lang}, using 'en'");
            lang = "en";
            asset = Resources.Load<TextAsset>("Localization/en");
        }

        CurrentLanguage = lang;
        _strings.Clear();
        if (asset != null) Parse(asset.text);

        PlayerPrefs.SetString(PrefKey, lang);
        PlayerPrefs.Save();
        OnLanguageChanged?.Invoke();
    }

    private void Parse(string content)
    {
        foreach (var line in content.Split('\n'))
        {
            var trimmed = line.Trim();
            if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#")) continue;
            var idx = trimmed.IndexOf('=');
            if (idx < 0) continue;
            _strings[trimmed.Substring(0, idx).Trim()] = trimmed.Substring(idx + 1).Trim().Replace("\\n", "\n");
        }
    }
}
