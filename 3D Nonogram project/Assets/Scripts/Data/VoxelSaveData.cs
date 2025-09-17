using System;
using UnityEngine;

[Serializable]
public class VoxelSaveData
{
    public Vector3Int gridPosition;
    public Color voxelColor;
    public VoxelSaveData(Vector3Int pos, Color color)
    {
        gridPosition = pos;
        voxelColor = color;
    }
}
