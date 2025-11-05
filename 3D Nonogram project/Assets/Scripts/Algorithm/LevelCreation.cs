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

        // Compute centered offset (same as editor)
        Vector3Int min = new Vector3Int(
            Mathf.FloorToInt(-(size.x - 1) * 0.5f),
            Mathf.FloorToInt(-(size.y - 1) * 0.5f),
            Mathf.FloorToInt(-(size.z - 1) * 0.5f)
        );

        // Initialize puzzle manager
        int puzzleCount = data.voxels.Count;
        int fillerCount = (size.x * size.y * size.z) - puzzleCount;
        _puzzleManager.Initialize(puzzleCount, fillerCount);

        // Build the grid using centered positions
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    Vector3Int gridIndex = new Vector3Int(x, y, z);
                    Vector3Int worldPos = gridIndex + min; // Shift index to centered world-space position

                    bool isPuzzle = data.voxelSaveDataMap.ContainsKey(worldPos);

                    Voxel v = Instantiate(_voxelPrefab, worldPos, Quaternion.identity, _voxelContainer);
                    v.Initialize(isPuzzle, _puzzleManager);
                    voxelGrid[x, y, z] = v;

                    if (isPuzzle)
                    {
                        data.voxelMap[worldPos] = v; // use centered position as key
                        v.SetFinalColor(data.voxelSaveDataMap[worldPos].voxelColor);
                        _puzzleManager.puzzleVoxelList.Add(v);
                    }
                }
            }
        }

        // Generate clues using grid
        _clueGenerator.GenerateAndAssignClues(voxelGrid);
    }

}
