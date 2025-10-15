using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GridSaveData 
{
    public Vector3Int gridSize;
    public List<VoxelSaveData> voxels = new List<VoxelSaveData>();

    [NonSerialized]
    public Dictionary<Vector3Int, VoxelSaveData> voxelSaveDataMap = new Dictionary<Vector3Int, VoxelSaveData>(); // Used for easy checkups

    public Dictionary<Vector3Int, Voxel> voxelMap = new Dictionary<Vector3Int, Voxel>(); // Used for voxel check up during puzzle
    public bool HasVoxel(Vector3Int pos)
    {
        return voxelMap.ContainsKey(pos);
    }

}
