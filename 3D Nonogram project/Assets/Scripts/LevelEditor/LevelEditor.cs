using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] private float _maxDistance;
    [SerializeField] private int _maxHitsBuffer;

    
    [SerializeField] private GameObject _voxel;
    [SerializeField] private GameObject _transparentVoxel;
    [SerializeField] private Transform _voxelParent;
    [SerializeField] private LayerMask _voxelLayer; 
    [SerializeField] private Color _color;

    [SerializeField] private LoadMenu _menu;
    [SerializeField] private BoundingBox _boundingBox;
    [SerializeField] private UploadLevelData _uploadLevelData;


    private Camera _mainCam;
    private RaycastHit[] _hitBuffer;
    private GameObject voxelHit;    
    private Vector3Int _lastGridPos;
    private Vector3 _lastMousePos = Vector3.negativeInfinity;
    private Transform _transparentT;
    private RaycastHit _currentHit;
    private bool _hasHit;
    private EditMode _editMode;
    private EditorHistory _history = new EditorHistory();

    public GridSaveData data = new GridSaveData();

    private void Awake()
    {
        _mainCam = Camera.main; // cache once
        _hitBuffer = new RaycastHit[_maxHitsBuffer];
        if (_transparentVoxel) _transparentT = _transparentVoxel.transform;
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

        if (Input.GetMouseButtonDown(0) && IsInsideGrid(gridPos))
        {
            HandleClick(gridPos, voxelHit);
        }
    }

    private void ShootRayCast()
    {
        Ray ray = _mainCam.ScreenPointToRay(_lastMousePos);

        // Use RaycastNonAlloc to avoid allocations.
        int hits = Physics.RaycastNonAlloc(ray, _hitBuffer, _maxDistance, _voxelLayer, QueryTriggerInteraction.Ignore);

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
                if (!data.voxelMap.ContainsKey(gridPos) && IsInsideGrid(gridPos))
                    _history.Do(new PlaceVoxelCommand(this, gridPos, _color));
                break;

            case EditMode.Erase:
                if (voxel == null) return;
                var vd = voxel.GetComponent<VoxelData>();
                _history.Do(new EraseVoxelCommand(this, vd.GridPosition, vd.VoxelColor, voxel));
                break;

            case EditMode.Paint:
                if (voxel == null) return;
                var vData = voxel.GetComponent<VoxelData>();
                if (vData.VoxelColor != _color)
                    _history.Do(new PaintVoxelCommand(this, vData.GridPosition, vData.VoxelColor, _color));
                break;
        }

        ShootRayCast();
    }


    private void HandleHover(Vector3Int gridPos)
    {
        if (_transparentT != null)
            _transparentT.position = gridPos;
    }

    public GameObject InstantiateVoxel(Vector3Int pos, Color color)
    {
        var go = Instantiate(_voxel, pos, Quaternion.identity, _voxelParent);
        go.GetComponent<VoxelData>().Initialize(pos, color);
        return go;
    }

    public void AddVoxelToData(Vector3Int pos, Color color)
    {
        var s = new VoxelSaveData(pos, color);
        data.voxelSaveDataMap[pos] = s;
        data.voxels.Add(s);
    }

    public void RemoveVoxelFromData(Vector3Int pos)
    {
        if (data.voxelSaveDataMap.TryGetValue(pos, out var s))
        {
            data.voxelSaveDataMap.Remove(pos);
            data.voxels.Remove(s);
        }
    }

    public void PaintVoxel(Vector3Int pos, Color color)
    {
        foreach (Transform child in _voxelParent)
        {
            var vd = child.GetComponent<VoxelData>();
            if (vd != null && vd.GridPosition == pos)
            {
                vd.SetColor(color);
                break;
            }
        }
        if (data.voxelSaveDataMap.TryGetValue(pos, out var s)) s.voxelColor = color;
    }
    
    public void SetGridSize(Vector3Int newSize, out List<VoxelSaveData> trimmedOut)
    {
        data.gridSize = newSize;
        // collect trimmed before deleting
        GetCenteredBounds(newSize, out var min, out var max);
        trimmedOut = new List<VoxelSaveData>();

        List<Vector3Int> toRemove = new();
        foreach (var kv in data.voxelMap)
        {
            var p = kv.Key;
            if (p.x < min.x || p.x > max.x || p.y < min.y || p.y > max.y || p.z < min.z || p.z > max.z)
                toRemove.Add(p);
        }
        foreach (var p in toRemove)
        {
            if (data.voxelSaveDataMap.TryGetValue(p, out var s)) trimmedOut.Add(new VoxelSaveData(s.gridPosition, s.voxelColor));
            RemoveVoxelFromData(p);

            foreach (Transform child in _voxelParent)
            {
                var vd = child.GetComponent<VoxelData>();
                if (vd != null && vd.GridPosition == p)
                {
                    Destroy(child.gameObject);
                    break;
                }
            }
        }
    }

    private static void GetCenteredBounds(Vector3Int size, out Vector3Int min, out Vector3Int max)
    {
        min = new Vector3Int(
            Mathf.FloorToInt(-(size.x - 1) * 0.5f),
            Mathf.FloorToInt(-(size.y - 1) * 0.5f),
            Mathf.FloorToInt(-(size.z - 1) * 0.5f)
        );
        max = min + size - Vector3Int.one;
    }

    private bool IsInsideGrid(Vector3Int p)
    {
        GetCenteredBounds(_boundingBox.GridSize, out var min, out var max);
        return p.x >= min.x && p.x <= max.x
            && p.y >= min.y && p.y <= max.y
            && p.z >= min.z && p.z <= max.z;
    }

    private Vector3Int GetGridPosition(Vector3 hitPoint, Vector3 normal)
    {
        // Move to the voxel space by rounding, then offset in the hit normal direction
        return Vector3Int.RoundToInt(hitPoint + normal * 0.5f);
    }

    public void SaveFile(string fileName)
    { 

        SaveLoadSystem.Save(data ,fileName);
    }

    public void LoadSaveFile(string fileName)
    {
        foreach (Transform child in _voxelParent)
        {
            Destroy(child.gameObject);
        }

        data = SaveLoadSystem.Load<GridSaveData>(fileName);

        if (data == null) return; 

        _boundingBox.LoadGridSize(data.gridSize);

        // Rebuild Dictionary for easy look ups
        data.voxelSaveDataMap = new Dictionary<Vector3Int, VoxelSaveData>();
        foreach (VoxelSaveData v in data.voxels)
        {
            data.voxelSaveDataMap[v.gridPosition] = v;

            GameObject voxelInstance = Instantiate(_voxel, v.gridPosition, Quaternion.identity);
            voxelInstance.transform.SetParent(_voxelParent, true);

            voxelInstance.GetComponent<VoxelData>().SetGridPosition(v.gridPosition);
            voxelInstance.GetComponent<VoxelData>().SetColor(v.voxelColor);
        }
    }

    public void UploadLevelData()
    {
        _uploadLevelData.UploadCoroutine(data);
    }
    public void ClearAllVoxels()
    {
        foreach (Transform child in _voxelParent)
        {
            Destroy(child.gameObject);
        }

        data.voxelMap.Clear();
    }

    public void OnGridSizeCommand(Vector3Int newSize)
    {
        _history.Do(new GridSizeCommand(this, data.gridSize,newSize));
    }
    public void UndoButton() => _history.Undo();
    public void RedoButton() => _history.Redo();

    public void SetEditMode(EditMode editMode) => _editMode = editMode;
    public void SetColor(Color color) => _color = color;
    public void SetGridSize(Vector3Int gridSize) => data.gridSize = gridSize;
}
