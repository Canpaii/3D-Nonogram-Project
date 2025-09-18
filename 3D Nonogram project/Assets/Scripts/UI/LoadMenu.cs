using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Collections.Generic;

public class LoadMenu : MonoBehaviour
{
    [SerializeField] private Transform[] contentParent;    // The ScrollView Content
    [SerializeField] private GameObject buttonPrefab;    // Our SaveButtonPrefab
    [SerializeField] private LevelEditor levelEditor;    // Reference to your LevelEditor

    private void OnEnable()
    {
        RefreshSaveList();
    }

    public void RefreshSaveList()
    {
        foreach (Transform parent in contentParent)
        {
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);
            }
        }

        // Get all save files
        List<string> saves = SaveLoadSystem.GetAllSaveFiles();
       
        foreach (string saveName in saves)
        {
            // I got two panels for the same thing but the return button is different so i'm doing it this way since this is easier
            for (int i = 0; i < contentParent.Length; i++)
            {
                 GameObject buttonGO = Instantiate(buttonPrefab, contentParent[i]);
                 Button button = buttonGO.GetComponent<Button>();

                // Set button text
                TMP_Text text = buttonGO.GetComponentInChildren<TMP_Text>();
                if (text != null) text.text = saveName;

                string file = saveName; 
                button.onClick.AddListener(() =>
                {
                    Debug.Log("Loading " + file);
                    levelEditor.LoadSaveFile(file);
                });


            }
        }
    }
}
