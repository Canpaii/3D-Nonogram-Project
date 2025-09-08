using UnityEngine;

[SerializeField]
public class LevelData
{
    [Header("Creator Data")]
    public string LevelName { get; private set; }
    public string AuthorName { get; private set; }
    public string Description { get; private set; }

    [Header("Level Stats")]
    public int Likes { get; private set; }
    public int Plays { get; private set; }
    public int AverageClearTimeInSeconds { get; private set; }

    [Header("Level Creation Data")]
    public VoxelData[] VoxelData { get; private set; }
    public Vector3Int GridSize;
}
