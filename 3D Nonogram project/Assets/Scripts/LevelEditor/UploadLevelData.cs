using UnityEngine;

public class UploadLevelData : MonoBehaviour
{
    [Header("Level Info")]
    public string LevelName { get; private set; }
    public string AuthorName { get; private set; }
    public string Discription { get; private set; }
    
    public GridData GridData {  get; private set; }
   
    public void UploadLevel()
    {
        LevelData levelData = new LevelData(LevelName, AuthorName, Discription, GridData);

        JsonUtility.ToJson(levelData);

        // Send JSON to server
    }
}
