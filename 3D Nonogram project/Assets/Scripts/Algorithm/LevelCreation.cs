using UnityEngine;

public class LevelCreation : MonoBehaviour
{
    // This script is for instancing the puzzle when a level gets selected, not for making the level itself
    public LevelSaveData LevelData { get; private set; }
    [SerializeField] private GameObject voxelPrefab;

    public void SetCurrentSelectedLevel(LevelSaveData levelData)
    {
        LevelData = levelData;
    }

    public void GenerateLevel()
    {
       /* foreach (VoxelData voxel in LevelSaveData.GridData.voxelsInGrid)
        {
            GameObject voxelInstance = Instantiate(voxelPrefab, voxel.GridPosition, Quaternion.identity);

            voxelInstance.GetComponent<Voxel>().Initialize(voxel.VoxelColor);
            voxelInstance.GetComponent<Voxel>().SetVoxelType(true);
        }*/
        
        //foreach (Vector3Int emptyPos in LevelSaveData.GridData.emptySpaces)
        //{
        //    GameObject voxelInstance = Instantiate(voxelPrefab, emptyPos, Quaternion.identity);
        //    voxelInstance.GetComponent<Voxel>().SetVoxelType(false);
        //}
    }
}
