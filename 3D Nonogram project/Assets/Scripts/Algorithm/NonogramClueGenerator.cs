using System.Collections.Generic;
using UnityEngine;

public class NonogramClueGenerator : MonoBehaviour
{
    public GridData gridData;


    // Returns all clues, keyed by which axis/line they belong to.
    public Dictionary<string, List<int>> GenerateClues()
    {
        var clues = new Dictionary<string, List<int>>();

        // ----- X-axis lines: for each (y,z), scan x = 0..GridSize.x-1 -----
        for (int y = 0; y < gridData.gridSize.y; y++)
        {
            for (int z = 0; z < gridData.gridSize.z; z++)
            {
                // getPos maps index i → (x=i, y, z) for this line
                List<int> lineClues = CountLine((x) => new Vector3Int(x, y, z), gridData.gridSize.x);
                // store under a readable key; you can swap to a struct later if you like
                clues[$"X_{y}_{z}"] = lineClues;
            }
        }

        // ----- Y-axis lines: for each (x,z), scan y = 0..GridSize.y-1 -----
        for (int x = 0; x < gridData.gridSize.x; x++)
        {
            for (int z = 0; z < gridData.gridSize.z; z++)
            {
                List<int> lineClues = CountLine((y) => new Vector3Int(x, y, z), gridData.gridSize.y);
                clues[$"Y_{x}_{z}"] = lineClues;
            }
        }

        // ----- Z-axis lines: for each (x,y), scan z = 0..GridSize.z-1 -----
        for (int x = 0; x < gridData.gridSize.x; x++)
        {
            for (int y = 0; y < gridData.gridSize.y; y++)
            {
                List<int> lineClues = CountLine((z) => new Vector3Int(x, y, z), gridData.gridSize.z);
                clues[$"Z_{x}_{y}"] = lineClues;
            }
        }

        return clues;
    }

    // Walks one straight line; counts consecutive filled cells into a list of run-lengths.
    private List<int> CountLine(System.Func<int, Vector3Int> getPos, int length)
    {
        List<int> clues = new List<int>();
        int count = 0; // current run length of filled cells

        for (int i = 0; i < length; i++)
        {
            Vector3Int pos = getPos(i);       // convert line index -> (x,y,z) for this line
            bool filled = gridData.HasVoxel(pos);

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

