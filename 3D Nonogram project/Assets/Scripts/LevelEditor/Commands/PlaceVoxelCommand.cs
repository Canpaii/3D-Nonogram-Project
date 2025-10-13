using UnityEngine;

public class PlaceVoxelCommand : IEditorCommand
{
    private readonly LevelEditor _editor;
    private readonly Vector3Int _pos;
    private readonly Color _color;
    private GameObject _instance;

    public PlaceVoxelCommand(LevelEditor editor, Vector3Int pos, Color color)
    { _editor = editor; _pos = pos; _color = color; }

    public void Execute()
    {
        if (_editor.data.voxelMap.ContainsKey(_pos)) return;
        _instance = _editor.InstantiateVoxel(_pos, _color);
        _editor.AddVoxelToData(_pos, _color);
    }

    public void Undo()
    {
        if (_instance != null) GameObject.Destroy(_instance);
        _editor.RemoveVoxelFromData(_pos);
    }
}
