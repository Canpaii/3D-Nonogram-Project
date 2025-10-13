using System.Collections.Generic;
using UnityEngine;

public class LevelCreation : MonoBehaviour
{
    // This script is for instancing the puzzle when a level gets selected, not for making the level itself

    [SerializeField] private LevelDataSO levelData;
    [SerializeField] private Voxel voxelPrefab;

    private Dictionary<Vector3Int, Voxel> voxelsInGrid = new Dictionary<Vector3Int, Voxel>();

    public void GenerateLevel()
    {
        foreach (var item in levelData.Data.GridData.voxels)
        {
            Voxel voxelInstance = Instantiate(voxelPrefab,item.gridPosition, Quaternion.identity);

            voxelInstance.SetVoxelType(true);
            voxelsInGrid[item.gridPosition] = voxelInstance;
        }

        for (int x = 0; x < levelData.Data.GridData.gridSize.x; x++)
        {
            for (int y = 0; y < levelData.Data.GridData.gridSize.y; y++)
            {
                for (int z = 0; z < levelData.Data.GridData.gridSize.z; z++)
                {
                    Vector3Int pos = new Vector3Int(x, y, z);
                    if (!voxelsInGrid.ContainsKey(pos)) // fill in empty spaces
                    {
                        Voxel fillerVoxelInstance = Instantiate(voxelPrefab, pos, Quaternion.identity);
                        fillerVoxelInstance.SetVoxelType(false);
                    }
                }
            }

        }
    }
}
