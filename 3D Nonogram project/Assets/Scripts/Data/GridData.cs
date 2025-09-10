using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridData : MonoBehaviour
{
    public Vector3Int GridSize {  get; private set; }

    // Keep track what positions are filled in to know what places need to be filled with filler voxels.
    public Dictionary<Vector3Int, Voxel> voxelsInGrid = new Dictionary<Vector3Int, Voxel>();
    public List<Vector3Int> emptySpaces = new List<Vector3Int>();

    public void StoreVoxelInGridData(Vector3Int gridPOS, Voxel voxel)
    {
        voxelsInGrid.Add(gridPOS, voxel);
    }

    public void SetGridSize(Vector3Int size)
    {
        GridSize = size;
    }

    public bool HasVoxel(Vector3Int pos)
    {
        return voxelsInGrid.ContainsKey(pos);
    }

    public void SetVoxel(Vector3Int pos, Voxel voxel)
    {
        voxelsInGrid[pos] = voxel;
    }

    public void RemoveVoxel(Vector3Int pos)
    {
        voxelsInGrid.Remove(pos);
    }

    public Voxel GetVoxel(Vector3Int pos)
    {
        voxelsInGrid.TryGetValue(pos, out Voxel voxel);
        return voxel;
    }

    public void GetEmptySpaces() // Generate this after level is done in the level creator 
    {
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                for (int z = 0; z < GridSize.z; z++)
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
