using System.Collections.Generic;
using UnityEngine;

public class NonogramClueGenerator : MonoBehaviour
{
    public LevelDataSO levelData;

    // Returns all clues, keyed by which axis/line they belong to.
    public void GenerateAndAssignClues(Voxel[,,] voxelGrid)
    {
        Vector3Int size = levelData.Data.GridData.gridSize;
        // X-axis lines (vary x, fixed y,z)
        for (int y = 0; y < size.y; y++)
        {
            for (int z = 0; z < size.z; z++)
            {
                List<int> runs = CountLine(x => new Vector3Int(x, y, z), size.x);
                string clueText;
                if (runs.Count == 1 && runs[0] == 0)
                {
                    clueText = "0";  // no filled blocks in this line
                }
                else
                {
                    int total = 0;
                    runs.ForEach(r => total += r);
                    int sequences = runs.Count;
                    // Format as total with sequences in superscript if multiple sequences
                    clueText = (sequences > 1)
                               ? total + "<sup>" + sequences + "</sup>"
                               : total.ToString();
                }
                // Assign clue to the min and max X side voxels of this line:
                Voxel minVoxel = voxelGrid[0, y, z];
                Voxel maxVoxel = voxelGrid[size.x - 1, y, z];
                minVoxel.SetClue(Axis.X, clueText);
                maxVoxel.SetClue(Axis.X, clueText);
            }
        }
        // Y-axis lines (vary y, fixed x,z)
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.z; z++)
            {
                List<int> runs = CountLine(y => new Vector3Int(x, y, z), size.y);
                string clueText;
                if (runs.Count == 1 && runs[0] == 0)
                {
                    clueText = "0";
                }
                else
                {
                    int total = 0;
                    runs.ForEach(r => total += r);
                    int sequences = runs.Count;
                    clueText = (sequences > 1)
                               ? total + "<sup>" + sequences + "</sup>"
                               : total.ToString();
                }
                Voxel minVoxel = voxelGrid[x, 0, z];
                Voxel maxVoxel = voxelGrid[x, size.y - 1, z];
                minVoxel.SetClue(Axis.Y, clueText);
                maxVoxel.SetClue(Axis.Y, clueText);
            }
        }
        // Z-axis lines (vary z, fixed x,y)
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                List<int> runs = CountLine(z => new Vector3Int(x, y, z), size.z);
                string clueText;
                if (runs.Count == 1 && runs[0] == 0)
                {
                    clueText = "0";
                }
                else
                {
                    int total = 0;
                    runs.ForEach(r => total += r);
                    int sequences = runs.Count;
                    clueText = (sequences > 1)
                               ? total + "<sup>" + sequences + "</sup>"
                               : total.ToString();
                }
                Voxel minVoxel = voxelGrid[x, y, 0];
                Voxel maxVoxel = voxelGrid[x, y, size.z - 1];
                minVoxel.SetClue(Axis.Z, clueText);
                maxVoxel.SetClue(Axis.Z, clueText);
            }
        }
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

