using System.Collections.Generic;
using UnityEngine;

public class NonogramClueGenerator : MonoBehaviour
{
    public LevelDataSO levelData;

    public void GenerateAndAssignClues(Voxel[,,] voxelGrid)
    {
        Vector3Int size = levelData.Data.GridData.gridSize;

        // Match offset used in LevelCreation
        Vector3Int min = new Vector3Int(
            Mathf.FloorToInt(-(size.x - 1) * 0.5f),
            Mathf.FloorToInt(-(size.y - 1) * 0.5f),
            Mathf.FloorToInt(-(size.z - 1) * 0.5f)
        );

        // X-axis lines (vary x, fixed y,z)
        for (int y = 0; y < size.y; y++)
        {
            for (int z = 0; z < size.z; z++)
            {
                List<int> runs = CountLine(x =>
                    new Vector3Int(x + min.x, y + min.y, z + min.z), size.x);
                string clueText = FormatClueText(runs);

                for (int x = 0; x < size.x; x++)
                {
                    voxelGrid[x, y, z].SetClue(Axis.X, clueText);
                }
            }
        }

        // Y-axis lines (vary y, fixed x,z)
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.z; z++)
            {
                List<int> runs = CountLine(y =>
                    new Vector3Int(x + min.x, y + min.y, z + min.z), size.y);
                string clueText = FormatClueText(runs);

                for (int y = 0; y < size.y; y++)
                {
                    voxelGrid[x, y, z].SetClue(Axis.Y, clueText);
                }
            }
        }

        // Z-axis lines (vary z, fixed x,y)
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                List<int> runs = CountLine(z =>
                    new Vector3Int(x + min.x, y + min.y, z + min.z), size.z);
                string clueText = FormatClueText(runs);

                for (int z = 0; z < size.z; z++)
                {
                    voxelGrid[x, y, z].SetClue(Axis.Z, clueText);
                }
            }
        }
    }

    // Helper to format the clue text from runs of filled cells
    private string FormatClueText(List<int> runs)
    {
        if (runs.Count == 1 && runs[0] == 0)
            return "";  // no filled blocks in this line

        int total = 0;
        runs.ForEach(r => total += r);
        int sequences = runs.Count;
        // Format as "total<sup>sequences</sup>" if multiple sequences, else just total
        return (sequences > 1) ? $"{total}<sup>{sequences}</sup>" : total.ToString();
    }


    // Walks one straight line; counts consecutive filled cells into a list of run-lengths.
    private List<int> CountLine(System.Func<int, Vector3Int> getPos, int length)
    {
        List<int> clues = new List<int>();
        int count = 0; // current run length of filled cells

        for (int i = 0; i < length; i++)
        {
            Vector3Int pos = getPos(i);       // convert line index -> (x,y,z) for this line
            bool filled = levelData.Data.GridData.HasVoxel(pos);

            if (filled)
            {
                count++;                       // extend the current run
            }
            else
            {
                if (count > 0)                 // a run just ended -> record it
                {
                    clues.Add(count);
                    count = 0;
                }
            }
        }

        if (count > 0)                         // flush a run that ends at the line’s edge
            clues.Add(count);

        if (clues.Count == 0)                  // convention: no voxels in this line -> [0]
            clues.Add(0);

        return clues;
    }
}

