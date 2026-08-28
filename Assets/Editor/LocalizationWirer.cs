using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class LocalizationWirer
{
    private static readonly Dictionary<string, string> TextToKey = new Dictionary<string, string>
    {
        { "Play",     "ui.play"          },
        { "Settings", "ui.settings"      },
        { "Credits",  "ui.credits"       },
        { "Exit",     "ui.exit"          },
        { "Back",     "ui.back"          },
        { "Options",  "ui.options_title" },
    };

    [MenuItem("Tools/Localization/Wire Current Scene")]
    public static void WireCurrentScene()
    {
        int wired = 0;
        int skipped = 0;

        foreach (var text in Object.FindObjectsOfType<Text>())
        {
            var trimmed = text.text.Trim();

            if (!TextToKey.TryGetValue(trimmed, out string key))
            {
                skipped++;
                continue;
            }

            if (text.GetComponent<LocalizedText>() != null)
            {
                skipped++;
                continue;
            }

            Undo.RecordObject(text.gameObject, "Add LocalizedText");
            var localized = Undo.AddComponent<LocalizedText>(text.gameObject);
            localized.Key = key;
            EditorUtility.SetDirty(text.gameObject);
            wired++;
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log($"[LocalizationWirer] Wired: {wired}, Skipped: {skipped}");
        EditorUtility.DisplayDialog("Localization Wirer",
            $"Done!\nWired: {wired} text(s)\nSkipped: {skipped} (no match or already wired)", "OK");
    }
}
