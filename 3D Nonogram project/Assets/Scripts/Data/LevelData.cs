using System;
using UnityEngine;

[Serializable]
public class LevelData
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
    public GridData GridData;

    public LevelData(string lvlName, string authorName, string desc, GridData gridData)
    {
        LevelName = lvlName;
        AuthorName = authorName;
        Description = desc;
        GridData = gridData;
    }
}
