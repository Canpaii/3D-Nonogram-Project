using System.Collections.Generic;
using UnityEngine;

public class GridSizeCommand : IEditorCommand
{

    private readonly LevelEditor _editor;
    private readonly Vector3Int _oldSize;
    private readonly Vector3Int _newSize;

    // store trimmed voxels to restore on undo
    private List<VoxelSaveData> _trimmed = new();

    public GridSizeCommand(LevelEditor editor, Vector3Int oldSize, Vector3Int newSize)
    { _editor = editor; _oldSize = oldSize; _newSize = newSize; }

    public void Execute()
    {
        _editor.SetGridSize(_newSize, out _trimmed); // fills _trimmed with removed voxels
    }

    public void Undo()
    {
        _editor.SetGridSize(_oldSize, out _); // discard new trim results
        foreach (var v in _trimmed)
        {
            var go = _editor.InstantiateVoxel(v.gridPosition, v.voxelColor);
            _editor.AddVoxelToData(v.gridPosition, v.voxelColor);
        }
    }
}

