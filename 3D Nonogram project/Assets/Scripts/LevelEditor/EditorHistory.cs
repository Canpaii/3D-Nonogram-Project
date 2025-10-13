using System.Collections.Generic;

public interface IEditorCommand
{
    void Execute();
    void Undo();
}

public class EditorHistory
{
    private readonly Stack<IEditorCommand> _undo = new();
    private readonly Stack<IEditorCommand> _redo = new();

    public void Do(IEditorCommand cmd)
    {
        cmd.Execute();
        _undo.Push(cmd);
        _redo.Clear();
    }

    public void Undo()
    {
        if (_undo.Count == 0) return;
        var cmd = _undo.Pop();
        cmd.Undo();
        _redo.Push(cmd);
    }

    public void Redo()
    {
        if (_redo.Count == 0) return;
        var cmd = _redo.Pop();
        cmd.Execute();
        _undo.Push(cmd);
    }

    public void Clear()
    {
        _undo.Clear();
        _redo.Clear();
    }
}
