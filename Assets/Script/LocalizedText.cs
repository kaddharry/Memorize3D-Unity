using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizedText : MonoBehaviour
{
    public string Key;

    private Text _text;

    void Awake() => _text = GetComponent<Text>();

    void OnEnable()
    {
        LocalizationManager.OnLanguageChanged += Refresh;
        Refresh();
    }

    void OnDisable()
    {
        LocalizationManager.OnLanguageChanged -= Refresh;
    }

    void Refresh()
    {
        if (_text != null && !string.IsNullOrEmpty(Key))
            _text.text = LocalizationManager.Get(Key);
    }
}
