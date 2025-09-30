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
    public Button button;
    [SerializeField] 
    public void ListCoroutine()
    {
        StartCoroutine(ListLevels());
    }
    private IEnumerator ListLevels()
    {
        string url = "http://<url>:5000/list";
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
                Button buttonInstance = Instantiate(button, transform);
                buttonInstance.transform.SetParent(transform, false);
            }
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }

      
    }
}
