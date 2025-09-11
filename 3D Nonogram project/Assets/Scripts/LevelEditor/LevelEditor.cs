using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;


public class LevelEditor : MonoBehaviour
{
    // private Stack _previousMoves = new Stack();
    private EditMode _editMode;
    [SerializeField] private Color _color;
    [SerializeField] private GameObject transparentVoxel;
    [SerializeField] private Transform voxelParent;

    private RaycastHit hit;
    public bool hasHit;
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
        ShootRayCast();

        if (!hasHit) return;

        Vector3 hitPos = hit.point;
        Vector3Int gridPos = GetGridPosition(hitPos);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            HandleClick(gridPos);
        }
        else
        {
             HandleHover(gridPos);
        }
    }

    // I need to find the inside of the box, so I shoot a raycastall and get the 2 hitpoint.
    // That way you are always inside the box, and if you happen to hit another voxel that also counts as the second hitpoint so everything should work correctly
    private void ShootRayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray,1000f);
        Debug.Log(hits);
        // Sort by distance just to be safe
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        if (hits.Length > 1)
        {
            // hits[0] is the entry point
            // hits[1] is the exit point (other side of collider)
            Debug.Log("Exit point: " + hits[1].point);

            hit = hits[1];
            hasHit = true;  
        }
        else
        {
            hasHit = false;
        }
        
    }

    private void HandleClick(Vector3Int gridPos)
    {
        switch (_editMode)
        {
            case EditMode.Place: 
                GameObject voxelInstance = Instantiate(transparentVoxel, gridPos, Quaternion.identity);
                voxelInstance.transform.SetParent(voxelParent, true);
                break;
            case EditMode.Erase:

                break;
            case EditMode.Paint:

                break;  
        }
    }

    private void HandleHover(Vector3Int hit)
    {
        Vector3 hitPoint = hit;
        
        Vector3 gridPosition = GetGridPosition(hitPoint);

        // Instantiate a preview voxel 
        // I shouldn't instantiate that but rather enable and disable it otherwise it will be bad for performance
       // GameObject previewVoxel = Instantiate(transparentVoxel, gridPosition, Quaternion.identity);
    }
    private Vector3Int GetGridPosition(Vector3 hitPoint)
    {
        Vector3Int v3Int = Vector3Int.RoundToInt(hitPoint);

        return v3Int;
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
