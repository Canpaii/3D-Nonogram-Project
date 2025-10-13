using UnityEngine;

public class LevelDataContainer : MonoBehaviour
{
    public static LevelDataContainer Instance;
    [SerializeField] private LevelDataSO _data;

    public void SetLevelData(LevelDataSO data) => _data = data;

    private void Awake()
    {
        Instance = this;
    }
}
