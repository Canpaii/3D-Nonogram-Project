using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class UploadLevelData : MonoBehaviour
{
    [Header("Level Info")]
    public string LevelName { get; private set; }
    public string AuthorName { get; private set; }
    public string Description { get; private set; }

    [SerializeField] private TMP_InputField _lvlName, _authName, _desc;
    public IEnumerator UploadLevel(GridSaveData data)
    {
        LevelSaveData levelData = new LevelSaveData(LevelName, AuthorName, Description, data);
        string rawFileName = $"{AuthorName}_{LevelName}";
        string safeFileName = MakeSafeFileName(rawFileName);

        // pretty print true, for debugging will need to turn it to false later
        string json = JsonUtility.ToJson(levelData, true); 
        string url = $"https://5b685f8e6d16.ngrok-free.app/upload?name={UnityWebRequest.EscapeURL(safeFileName)}";

        // Turn json to bytes, the flask server decodes it back to text
        byte[] body = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest req = new UnityWebRequest(url, "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(body);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.SetRequestHeader("ngrok-skip-browser-warning", "true");

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Succes: " + req.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"Upload failed:{req.responseCode} {req.error}");
            }
        }
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

    //private bool CheckName()
    //{
    //    return (LevelName != null && AuthorName != null);
    //}

    public void SetLevelName()
    {
        LevelName = _lvlName.text;
    }    
    public void SetAuthorName()
    {
        AuthorName = _authName.text;
    }
        
    public void SetDescription()
    {
        Description = _desc.text;
    }
}
