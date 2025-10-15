using UnityEngine;

public class LevelCreation : MonoBehaviour
{
    [SerializeField] private LevelDataSO levelData;
    [SerializeField] private Voxel voxelPrefab;
    [SerializeField] private PuzzleManager puzzleManager;
    [SerializeField] private NonogramClueGenerator clueGenerator; // ?? assign in inspector

    private Voxel[,,] voxelGrid;

    public void Start()
    {
        GenerateLevel();
        clueGenerator.GenerateAndAssignClues(voxelGrid);
    }

    private void GenerateLevel()
    {
        GridSaveData data = levelData.Data.GridData;
        Vector3Int size = data.gridSize;
        voxelGrid = new Voxel[size.x, size.y, size.z];

        int puzzleCount = data.voxels.Count;
        int fillerCount = (size.x * size.y * size.z) - puzzleCount;
        puzzleManager.Initialize(puzzleCount, fillerCount);

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    Vector3Int pos = new Vector3Int(x, y, z);
                    bool isPuzzle = data.voxelMap.ContainsKey(pos);

                    Voxel v = Instantiate(voxelPrefab, pos, Quaternion.identity);
                    v.Initialize(isPuzzle, puzzleManager);

                    voxelGrid[x, y, z] = v;
                }
            }
        }
    }
}
