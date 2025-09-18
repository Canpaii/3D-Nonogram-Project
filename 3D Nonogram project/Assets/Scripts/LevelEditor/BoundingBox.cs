using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BoundingBox : MonoBehaviour
{
    // This will be done really cursed, But I simply dont know any other method
    // I will make a box made out of planes, and with a raycast I will ignore the first plane hit. 
    // With that I'm able to get "Inside" the box and place my voxel with a second raycast that hits the other plane
    [SerializeField] private GameObject _xPlane, _minXPlane;
    [SerializeField] private GameObject _yPlane, _minYPlane;
    [SerializeField] private GameObject _zPlane, _minZPlane;

    [SerializeField] private Slider _sliderX, _sliderY, _sliderZ;
    [SerializeField] private TMP_Text _textX, _textY, _textZ;

    [SerializeField] private LevelEditor _levelEditor;
    [field: SerializeField] public Vector3Int GridSize { get; private set; }


    public void SetGridSizeTest() // button test
    {
        AdjustPlaneScales();
    }

    private void AdjustPlaneScales()
    {
        // Change scale 
        _xPlane.transform.localScale = new Vector3(0.1f, GridSize.y, GridSize.z);
        _minXPlane.transform.localScale = new Vector3(0.1f, GridSize.y, GridSize.z);

        _yPlane.transform.localScale = new Vector3(GridSize.x, 0.1f, GridSize.z);
        _minYPlane.transform.localScale = new Vector3(GridSize.x, 0.1f, GridSize.z);

        _zPlane.transform.localScale = new Vector3(GridSize.x, GridSize.y , 0.1f);
        _minZPlane.transform.localScale = new Vector3(GridSize.x, GridSize.y, 0.1f);

        // Adjust positions
        float xPlanePos = GridSize.x / 2f;
        float yPlanePos = GridSize.y / 2f;
        float zPlanePos = GridSize.z / 2f;


        _xPlane.transform.position = new Vector3(xPlanePos, 0, 0);
        _minXPlane.transform.position = new Vector3(-xPlanePos, 0, 0);

        _yPlane.transform.position = new Vector3(0, yPlanePos, 0);
        _minYPlane.transform.position = new Vector3(0, -yPlanePos, 0);

        _zPlane.transform.position = new Vector3(0, 0, zPlanePos);
        _minZPlane.transform.position = new Vector3(0, 0, -zPlanePos);

        _levelEditor.SetGridSize(GridSize);
    }

    public void LoadGridSize(Vector3Int gridSize)
    {
        GridSize = gridSize;

        AdjustPlaneScales();
    }

    public void SetGridSizeX()
    {
        GridSize = new Vector3Int(((int)_sliderX.value), GridSize.y, GridSize.z);
        _textX.text = $"X: {(int)_sliderX.value}";
        AdjustPlaneScales();
    }

    public void SetGridSizeY()
    {
        GridSize = new Vector3Int(GridSize.x, (int)_sliderY.value, GridSize.z);
        _textY.text = $"Y: {(int)_sliderY.value}";
        AdjustPlaneScales();
    }
    public void SetGridSizeZ()
    {
        GridSize = new Vector3Int(GridSize.x, GridSize.y, (int)_sliderZ.value);
        _textZ.text = $"Z: {(int)_sliderZ.value}";
        AdjustPlaneScales();
    }
}
