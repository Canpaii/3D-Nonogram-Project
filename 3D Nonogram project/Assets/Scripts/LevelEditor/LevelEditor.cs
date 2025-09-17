using System.IO;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] private Color _color;
    [SerializeField] private GameObject _voxel;
    [SerializeField] private GameObject _transparentVoxel;
    [SerializeField] private Transform voxelParent;
    [SerializeField] private LayerMask voxelLayer; 
    [SerializeField] private float maxDistance;
    [SerializeField] private int maxHitsBuffer;
    [SerializeField] private LoadMenu menu;
    public Vector3Int GridSize { get; private set; }

    private Camera _mainCam;
    private RaycastHit[] _hitBuffer;
    private GameObject voxelHit;    
    private Vector3Int _lastGridPos;
    private Vector3 _lastMousePos = Vector3.negativeInfinity;
    private Transform _transparentT;
    private RaycastHit _currentHit;
    private bool _hasHit;
    private EditMode _editMode;
    private GridSaveData _data = new GridSaveData();

    private void Awake()
    {
        _mainCam = Camera.main; // cache once
        _hitBuffer = new RaycastHit[maxHitsBuffer];
        if (_transparentVoxel) _transparentT = _transparentVoxel.transform;
    }

    private void Start()
    {
        menu.RefreshSaveList();
    }
    private void Update()
    {
        // Only raycast if the mouse moved 
        if (Input.mousePosition != _lastMousePos)
        {
            _lastMousePos = Input.mousePosition;
            ShootRayCast();
        }

        if (!_hasHit) return;

        Vector3 hitPos = _currentHit.point;
        
        Vector3Int gridPos = GetGridPosition(hitPos, _currentHit.normal);
        if (gridPos != _lastGridPos)
        {
            HandleHover(gridPos);
            _lastGridPos = gridPos;
        }

        if (Input.GetMouseButtonDown(0) && !IsInsideGrid(gridPos))
        {
            HandleClick(gridPos, voxelHit);
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

                // Just need a small check because I dont want to destroy the bounding box
                if(_currentHit.collider.transform.tag == "a") voxelHit = _currentHit.collider.gameObject;
                

                Debug.Log(_currentHit);
                _hasHit = true;
                return;
            }
        }

        _hasHit = false;
    }

    private void HandleClick(Vector3Int gridPos, GameObject voxel = null)
    {

        switch (_editMode)
        {
            case EditMode.Place:
                if (_data.voxelMap.ContainsKey(gridPos)) return;

                GameObject voxelInstance = Instantiate(_voxel, gridPos, Quaternion.identity);
                voxelInstance.transform.SetParent(voxelParent, true);

                voxelInstance.GetComponent<VoxelData>().Initialize(gridPos, _color);

                VoxelSaveData voxelSaveData = new VoxelSaveData(gridPos, _color);

                _data.voxelMap.Add(gridPos, voxelSaveData);
                _data.voxels.Add(voxelSaveData);

                ShootRayCast();
                break;

            case EditMode.Erase:
                if (voxel == null) return;

                Vector3Int pos = voxel.GetComponent<VoxelData>().GridPosition;

                // Remove from dictionary and list
                if (_data.voxelMap.ContainsKey(pos))
                {
                    VoxelSaveData sData = _data.voxelMap[pos];

                    _data.voxels.Remove(sData);
                    _data.voxelMap.Remove(pos);
                }

                Destroy(voxel);

                ShootRayCast();
                break;

            case EditMode.Paint:
                if (voxel == null) return;

                VoxelData vData = voxel.GetComponent<VoxelData>();
                vData.SetColor(_color);

                if (_data.voxelMap.TryGetValue(vData.GridPosition, out VoxelSaveData saveData))
                    saveData.voxelColor = _color;
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

    public void SaveFile(string fileName)
    { 

        SaveLoadSystem.Save(_data ,fileName);
    }

    public void LoadSaveFile(string fileName)
    {
        _data = SaveLoadSystem.Load<GridSaveData>(fileName);
    }

   

    public void SetEditMode(EditMode editMode) => _editMode = editMode;
    public void SetColor(Color color) => _color = color;
    public void SetGridSize(Vector3Int v3Int) => GridSize = v3Int;
}
