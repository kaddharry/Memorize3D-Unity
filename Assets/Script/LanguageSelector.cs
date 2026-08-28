using UnityEngine;
using UnityEngine.UI;

public class LanguageSelector : MonoBehaviour
{
    private static readonly (string code, string label)[] Languages =
    {
        ("en", "English"),
        ("es", "Español")
    };

    void Start()
    {
        var canvas = FindObjectOfType<Canvas>();
        if (canvas == null) return;

        var font = FindObjectOfType<Text>()?.font;

        var container = CreateContainer(canvas.transform);
        CreateLabel(container, font);

        foreach (var (code, label) in Languages)
            CreateButton(container, code, label, font);
    }

    private RectTransform CreateContainer(Transform parent)
    {
        var go = new GameObject("LanguageButtons");
        go.transform.SetParent(parent, false);

        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = new Vector2(0, -100);
        rt.sizeDelta = new Vector2(300, 200);

        var layout = go.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 20;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;

        return rt;
    }

    private void CreateLabel(RectTransform container, Font font)
    {
        var go = new GameObject("LanguageLabel");
        go.transform.SetParent(container, false);

        var rt = go.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(300, 50);

        var text = go.AddComponent<Text>();
        text.text = LocalizationManager.Get("ui.language_label");
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.fontSize = 36;
        if (font != null) text.font = font;

        var localized = go.AddComponent<LocalizedText>();
        localized.Key = "ui.language_label";
    }

    private void CreateButton(RectTransform container, string langCode, string label, Font font)
    {
        var go = new GameObject(label);
        go.transform.SetParent(container, false);

        var rt = go.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(220, 60);

        var img = go.AddComponent<Image>();

        var btn = go.AddComponent<Button>();
        var capturedCode = langCode;
        btn.onClick.AddListener(() => LocalizationManager.Instance?.SetLanguage(capturedCode));

        var textGo = new GameObject("Label");
        textGo.transform.SetParent(go.transform, false);
        var textRt = textGo.AddComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.sizeDelta = Vector2.zero;

        var text = textGo.AddComponent<Text>();
        text.text = label;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.fontSize = 32;
        if (font != null) text.font = font;

        UpdateHighlight(img, langCode);
        LocalizationManager.OnLanguageChanged += () => UpdateHighlight(img, langCode);
    }

    private void UpdateHighlight(Image img, string langCode)
    {
        if (LocalizationManager.Instance == null) return;
        img.color = LocalizationManager.Instance.CurrentLanguage == langCode
            ? new Color(0.15f, 0.55f, 0.15f, 0.9f)
            : new Color(0.2f, 0.2f, 0.2f, 0.8f);
    }
}
