using System.Collections;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadLevels : MonoBehaviour
{
    [SerializeField] private LevelDataSO so;
    public IEnumerator DownloadLevel(string levelName)
    {
        string url = $"http://<pi-ip>:5000/download?name={levelName}";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Step 1: Raw JSON string from Flask
            string json = request.downloadHandler.text;
            Debug.Log("Received JSON: " + json);

            // Step 2: Deserialize into a C# scriptable object
            so.Data = JsonUtility.FromJson<LevelSaveData>(json);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }
}
