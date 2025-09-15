using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] private Color _color;
    [SerializeField] private GameObject _voxel;
    [SerializeField] private GameObject _transparentVoxel;
    [SerializeField] private Transform voxelParent;
    [SerializeField] private LayerMask voxelLayer; // assign layer(s) that contain voxels
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private int maxHitsBuffer = 2;

    public Vector3Int GridSize { get; private set; }

    private Camera _mainCam;
    private RaycastHit[] _hitBuffer;
    private Vector3Int _lastGridPos;
    private Vector3 _lastMousePos = Vector3.negativeInfinity;
    private Transform _transparentT;
    private RaycastHit _currentHit;
    public bool hasHit;
    private EditMode _editMode;

    private void Awake()
    {
        _mainCam = Camera.main; // cache once
        _hitBuffer = new RaycastHit[maxHitsBuffer];
        if (_transparentVoxel) _transparentT = _transparentVoxel.transform;
    }

    private void Update()
    {
        // Only raycast if the mouse moved (reduces work massively)
        if (Input.mousePosition != _lastMousePos)
        {
            _lastMousePos = Input.mousePosition;
            ShootRayCast();
        }

        if (!hasHit) return;

        Vector3 hitPos = _currentHit.point;
        
        Vector3Int gridPos = GetGridPosition(hitPos, _currentHit.normal);
        if (gridPos != _lastGridPos)
        {
            HandleHover(gridPos);
            _lastGridPos = gridPos;
        }

        if (Input.GetMouseButtonDown(0) && !IsInsideGrid(gridPos))
        {
            HandleClick(gridPos);
        }
    }

    // This function I still dont fully understand but it works for now
    private void ShootRayCast()
    {
        Ray ray = _mainCam.ScreenPointToRay(_lastMousePos);

        // Use RaycastNonAlloc to avoid allocations.
        int hits = Physics.RaycastNonAlloc(ray, _hitBuffer, maxDistance, voxelLayer, QueryTriggerInteraction.Ignore);

        if (hits >= 2)
        {
            // find the two nearest hits 
            int first = -1, second = -1;
            for (int i = 0; i < hits; i++)
            {
                float d = _hitBuffer[i].distance;
                if (first < 0 || d < _hitBuffer[first].distance)
                {
                    second = first;
                    first = i;
                }
                else if (second < 0 || d < _hitBuffer[second].distance)
                {
                    second = i;
                }
            }

            // entry is first, exit is second 
            if (first >= 0 && second >= 0)
            {
                _currentHit = _hitBuffer[second];
                Debug.Log(_currentHit);
                hasHit = true;
                return;
            }
        }

        hasHit = false;
    }

    private void HandleClick(Vector3Int gridPos)
    {
        switch (_editMode)
        {
            case EditMode.Place:
                GameObject voxelInstance = Instantiate(_voxel, gridPos, Quaternion.identity);
                voxelInstance.transform.SetParent(voxelParent, true);

                // Add to a list or array 
                break;
            case EditMode.Erase:
                break;
            case EditMode.Paint:
                break;
        }
    }

    private void HandleHover(Vector3Int gridPos)
    {
        if (_transparentT != null)
            _transparentT.position = gridPos;
    }

    // This doesn't work yet
    private bool IsInsideGrid(Vector3Int gridPos)
    {
        Vector3Int halfSize = GridSize / 2; 

        return gridPos.x >= -halfSize.x && gridPos.x <= halfSize.x &&
               gridPos.y >= -halfSize.y && gridPos.y <= halfSize.y &&
               gridPos.z >= -halfSize.z && gridPos.z <= halfSize.z;
    }
    private Vector3Int GetGridPosition(Vector3 hitPoint, Vector3 normal)
    {
        // Move to the voxel space by rounding, then offset in the hit normal direction
        return Vector3Int.RoundToInt(hitPoint + normal * 0.5f);
    }


    public void SetEditMode(EditMode editMode) => _editMode = editMode;
    public void SetColor(Color color) => _color = color;
    public void SetGridSize(Vector3Int v3Int) => GridSize = v3Int;  
}
