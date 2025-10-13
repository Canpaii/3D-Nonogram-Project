using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GridData : MonoBehaviour
{
    public Vector3Int gridSize;  // Needs to be public for JSON 

    // Keep track what positions are filled in to know what places need to be filled with filler voxels.
    public Dictionary<Vector3Int, VoxelSaveData> voxelsInGrid = new Dictionary<Vector3Int, VoxelSaveData>();
    public List<Vector3Int> emptySpaces = new List<Vector3Int>();

    public void AddVoxelInGridData(Vector3Int gridPOS, VoxelSaveData voxel)
    {
        voxelsInGrid.Add(gridPOS, voxel);
    }

    public void SetGridSize(Vector3Int size)
    {
        gridSize = size;
    }

    public bool HasVoxel(Vector3Int pos)
    {
        return voxelsInGrid.ContainsKey(pos);
    }

    public void RemoveVoxel(Vector3Int pos)
    {
        voxelsInGrid.Remove(pos);
    }

    public VoxelSaveData GetVoxel(Vector3Int pos)
    {
        voxelsInGrid.TryGetValue(pos, out VoxelSaveData voxel);
        return voxel;
    }

    public void GetEmptySpaces() // Generate this after level is done in the level creator 
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int z = 0; z < gridSize.z; z++)
                {
                    Vector3Int pos = new Vector3Int(x, y, z);
                    if (!voxelsInGrid.ContainsKey(pos)) // not in dictionary
                    {
                        emptySpaces.Add(pos);
                    }
                }
            }
        } 
    }
}
