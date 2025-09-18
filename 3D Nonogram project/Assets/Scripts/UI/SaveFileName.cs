using TMPro;
using UnityEngine;

public class SaveFileName : MonoBehaviour
{
    [SerializeField] private TMP_InputField _saveNameInput;
    [SerializeField] private LevelEditor _levelEditor;
    public void OnSaveButton()
    {
        string rawName = _saveNameInput.text;

        // Sanitize input
        string safeName = MakeSafeFileName(rawName);

        // Fallback if empty
        if (string.IsNullOrWhiteSpace(safeName))
            safeName = "Save_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss");

        // Call your save system
        _levelEditor.SaveFile(safeName);
        Debug.Log($"Saved level as {safeName}.json");
    }

    private string MakeSafeFileName(string input)
    {
        // Remove invalid filename chars
        foreach (char c in System.IO.Path.GetInvalidFileNameChars())
        {
            input = input.Replace(c.ToString(), "_");
        }
        return input;
    }
}


