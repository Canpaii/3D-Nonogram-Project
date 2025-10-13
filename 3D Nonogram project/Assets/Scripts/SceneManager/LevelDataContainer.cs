using UnityEngine;

public class LevelDataContainer : MonoBehaviour
{

    public LevelDataSO Data { get; private set; }
    public void SetLevelData(LevelDataSO data) => Data = data;
}
