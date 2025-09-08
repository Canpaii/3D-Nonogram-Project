using UnityEngine;

public class LevelCreation : MonoBehaviour
{
    // This script is for instancing the puzzle when a level gets selected, not for UGC
    public LevelData LevelData { get; private set; }
    [SerializeField] private GameObject voxelPrefab;

    public void SetCurrentSelectedLevel(LevelData levelData)
    {
        LevelData = levelData;
    }

    public void GenerateLevel()
    {
        foreach (VoxelData voxel in LevelData.VoxelData)
        {
            GameObject voxelInstance = Instantiate(voxelPrefab, voxel.GridPosition, Quaternion.identity);

            voxelInstance.GetComponent<Voxel>().Initialize(voxel.VoxelColor);
        }
    }
}
