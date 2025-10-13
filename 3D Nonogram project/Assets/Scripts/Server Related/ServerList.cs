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
        string url = "https://db08756d6d51.ngrok-free.app/list";
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
                LevelButton buttonInstance = Instantiate(button, transform);
                buttonInstance.Initialize(level, downloadLvls);

                buttonInstance.transform.SetParent(contenObject, false);
                
                buttonInstance.GetComponent<Button>().onClick.AddListener(()=> downloadLvls.StartDownloadCoroutine(level));

                if (buttonInstance.GetComponent<Button>() == null)
                {
                    print("Error");
                }
            }
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }
}
