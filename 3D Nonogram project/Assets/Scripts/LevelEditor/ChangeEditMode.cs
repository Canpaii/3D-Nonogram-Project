using UnityEngine;

public enum EditMode
{
    Place = 0,
    Erase = 1,
    Paint = 2,
}

public class ChangeEditMode : MonoBehaviour
{
    [SerializeField] private LevelEditor _levelEditor;
    public void OnEditModeChange(int i)
    {
        _levelEditor.SetEditMode((EditMode)i);
    }
}
