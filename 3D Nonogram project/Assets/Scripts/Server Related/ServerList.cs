using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class LevelList
{
    public string[] levels;
}

public class ServerList : MonoBehaviour
{
    [SerializeField] private LevelButton button;
    [SerializeField] private DownloadLevels downloadLvls;
    [SerializeField] private Transform contenObject;
    public void ListCoroutine()
    {
        StartCoroutine(ListLevels());
    }
    private IEnumerator ListLevels()
    {
        string url = "https://f2d7e2fad01b.ngrok-free.app/list";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log("Received JSON: " + json);


            string wrappedJson = "{\"levels\":" + json + "}";
            LevelList list = JsonUtility.FromJson<LevelList>(wrappedJson);
            foreach (var level in list.levels)
            {
                string levelCopy = level; // fix closure bug

                LevelButton buttonInstance = Instantiate(button, transform);
                buttonInstance.Initialize(levelCopy, downloadLvls);
                buttonInstance.transform.SetParent(contenObject, false);

                Button uiButton = buttonInstance.GetComponentInChildren<Button>();
                if (uiButton != null)
                {
     
                    uiButton.onClick.AddListener(() =>
                        downloadLvls.StartDownloadCoroutine(levelCopy));

                    print(uiButton.onClick.GetPersistentEventCount());
                }
                else
                {
                    Debug.LogWarning("Button component not found on LevelButton.");
                }
            }

        }
        else
        {
            Debug.LogError("Error: " + request.error);

        }
    }
}
