using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Collections.Generic;

public class LoadMenu : MonoBehaviour
{
    [SerializeField] private Transform contentParent;    // The ScrollView Content
    [SerializeField] private GameObject buttonPrefab;    // Our SaveButtonPrefab
    [SerializeField] private LevelEditor levelEditor;    // Reference to your LevelEditor

    private void OnEnable()
    {
        RefreshSaveList();
    }

    public void RefreshSaveList()
    {
        // Clear old buttons
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        // Get all save files
        List<string> saves = SaveLoadSystem.GetAllSaveFiles();

        foreach (string saveName in saves)
        {
            GameObject buttonGO = Instantiate(buttonPrefab, contentParent);
            Button button = buttonGO.GetComponent<Button>();

            // Set button text
            TMP_Text text = buttonGO.GetComponentInChildren<TMP_Text>();
            if (text != null) text.text = saveName;

            // Add listener
            button.onClick.AddListener(() =>
            {
                Debug.Log("Loading " + saveName);
                levelEditor.LoadSaveFile(saveName);
            });
        }
    }
}
