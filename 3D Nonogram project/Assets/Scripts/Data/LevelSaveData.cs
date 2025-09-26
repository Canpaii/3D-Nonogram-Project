using System;
using UnityEngine;

[Serializable]
public class LevelSaveData
{
    [Header("Creator Data")]
    public string LevelName;
    public string AuthorName;
    public string Description;

    [Header("Level Stats")]
    public int Likes;
    public int Plays;
    public int AverageClearTimeInSeconds;

    [Header("Level Creation Data")]
    public GridSaveData GridData;

    public LevelSaveData(string lvlName, string authorName, string desc, GridSaveData gridData)
    {
        LevelName = lvlName;
        AuthorName = authorName;
        Description = desc;
        GridData = gridData;
    }
}
