using UnityEngine;

public enum EditMode
{
    Place = 0,
    Erase = 1,
    Paint = 2,
}

public class ChangeEditMode : MonoBehaviour
{
    private LevelEditor _levelEditor;

    private void Awake()
    {
        _levelEditor = GetComponent<LevelEditor>();
    }

    public void OnEditModeChange(int i)
    {
        _levelEditor.SetEditMode((EditMode)i);
    }
}
