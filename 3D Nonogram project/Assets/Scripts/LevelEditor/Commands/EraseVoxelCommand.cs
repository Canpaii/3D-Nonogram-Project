using UnityEngine;

public class EraseVoxelCommand : IEditorCommand
{
    private readonly LevelEditor _editor;
    private readonly Vector3Int _pos;
    private readonly Color _prevColor;
    private GameObject _instance; // recreated on undo

    public EraseVoxelCommand(LevelEditor editor, Vector3Int pos, Color prevColor, GameObject instance)
    { _editor = editor; _pos = pos; _prevColor = prevColor; _instance = instance; }

    public void Execute()
    {
        if (_instance != null) GameObject.Destroy(_instance);
        _editor.RemoveVoxelFromData(_pos);
    }

    public void Undo()
    {
        _instance = _editor.InstantiateVoxel(_pos, _prevColor);
        _editor.AddVoxelToData(_pos, _prevColor);
    }
}
