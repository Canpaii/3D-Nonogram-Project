using UnityEngine;

public class LevelCreation : MonoBehaviour
{
    [SerializeField] private LevelDataSO _levelData;
    [SerializeField] private Voxel _voxelPrefab;
    [SerializeField] private PuzzleManager _puzzleManager;
    [SerializeField] private NonogramClueGenerator _clueGenerator;
    [SerializeField] private Transform _voxelContainer;

    private Voxel[,,] voxelGrid;

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        GridSaveData data = _levelData.Data.GridData;
        data.BuildVoxelMap();

        Vector3Int size = data.gridSize;
        voxelGrid = new Voxel[size.x, size.y, size.z];

        // init puzzle manager
        int puzzleCount = data.voxels.Count;
        int fillerCount = (size.x * size.y * size.z) - puzzleCount;
        _puzzleManager.Initialize(puzzleCount, fillerCount);

        // build the grid
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    Vector3Int pos = new Vector3Int(x, y, z);

                    bool isPuzzle = data.voxelSaveDataMap.ContainsKey(pos);
                    Voxel v = Instantiate(_voxelPrefab, pos, Quaternion.identity, _voxelContainer);
                    v.Initialize(isPuzzle, _puzzleManager);
                    voxelGrid[x, y, z] = v;

                    if (isPuzzle)
                    {
                        data.voxelMap[pos] = v;  // mark this position as a filled puzzle voxel
                    }

                }
            }
        }

        // now generate/display clues on edge voxels
        _clueGenerator.GenerateAndAssignClues(voxelGrid);
    }
}
