using UnityEngine;

public class PaintVoxelCommand : IEditorCommand
{
    private readonly LevelEditor _editor;
    private readonly Vector3Int _pos;
    private readonly Color _from;
    private readonly Color _to;

    public PaintVoxelCommand(LevelEditor editor, Vector3Int pos, Color from, Color to)
    { _editor = editor; _pos = pos; _from = from; _to = to; }

    public void Execute() => _editor.PaintVoxel(_pos, _to);
    public void Undo() => _editor.PaintVoxel(_pos, _from);
}
