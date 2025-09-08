using System.Collections.Generic;
using UnityEngine;

public class GridData : MonoBehaviour
{
    public Vector3Int GridSize {  get; private set; }

    // Keep track what positions are filled in to know what places need to be filled with filler voxels.
    public Dictionary<Vector3Int, Voxel> voxelsInGrid = new Dictionary<Vector3Int, Voxel>();

}
