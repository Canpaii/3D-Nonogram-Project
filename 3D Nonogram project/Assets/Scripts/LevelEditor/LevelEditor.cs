using System.Collections;
using UnityEngine;


public class LevelEditor : MonoBehaviour
{
    // private Stack _previousMoves = new Stack();
    private EditMode _editMode;
    [SerializeField] private Color _color;

    public void SetEditMode(EditMode editMode)
    {
        _editMode = editMode;
    }

    public void SetColor(Color color)
    {
        _color = color;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            HandleClick();
        }
    }

    private void HandleClick()
    {
        switch (_editMode)
        {
            case EditMode.Place: 

                break;
            case EditMode.Erase:

                break;
            case EditMode.Paint:

                break;  
        }
    }

    private void PlaceVoxel()
    {

    }

    private void EraseVoxel()
    {

    }

    private void PaintVoxel()
    {

    }
}
