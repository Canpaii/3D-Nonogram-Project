using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GridSaveData 
{
    public Vector3Int gridSize;
    public List<VoxelSaveData> voxels = new List<VoxelSaveData>();

    [NonSerialized]
    public Dictionary<Vector3Int, VoxelSaveData> voxelMap = new Dictionary<Vector3Int, VoxelSaveData>(); // Used for easy checkups
}
