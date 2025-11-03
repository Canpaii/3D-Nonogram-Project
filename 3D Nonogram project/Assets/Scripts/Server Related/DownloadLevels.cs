using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DownloadLevels : MonoBehaviour
{
    [SerializeField] private LevelDataSO so;
    [SerializeField] private string puzzleScene;

    public void StartDownloadCoroutine(string name)
    {
        print("PLS");
        StartCoroutine(DownloadLevel(name));
    }
    private IEnumerator DownloadLevel(string levelName)
    {
        string url = $"https://f2d7e2fad01b.ngrok-free.app/download?name={levelName}";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            print("works");
            // Step 1: Raw JSON string from Flask
            string json = request.downloadHandler.text;
            Debug.Log("Received JSON: " + json);

            // Step 2: Deserialize into a C# scriptable object
            so.Data = JsonUtility.FromJson<LevelSaveData>(json);

            // Step 3: Swap scenes
            SceneManager.LoadScene(puzzleScene);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }
}
