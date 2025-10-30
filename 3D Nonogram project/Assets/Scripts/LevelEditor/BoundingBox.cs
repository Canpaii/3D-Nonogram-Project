using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BoundingBox : MonoBehaviour
{
    // This will be done really cursed, But I simply dont know any other method
    // I will make a box made out of planes, and with a raycast I will ignore the first plane hit. 
    // With that I'm able to get "Inside" the box and place my voxel with a second raycast that hits the other plane
    [SerializeField] private Transform boundingBoxPivot;
    [SerializeField] private Transform _xPlane, _minXPlane;
    [SerializeField] private Transform _yPlane, _minYPlane;
    [SerializeField] private Transform _zPlane, _minZPlane;

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
        _xPlane.localScale = new Vector3(0.1f, GridSize.y, GridSize.z);
        _minXPlane.localScale = new Vector3(0.1f, GridSize.y, GridSize.z);

        _yPlane.localScale = new Vector3(GridSize.x, 0.1f, GridSize.z);
        _minYPlane.localScale = new Vector3(GridSize.x, 0.1f, GridSize.z);

        _zPlane.localScale = new Vector3(GridSize.x, GridSize.y, 0.1f);
        _minZPlane.localScale = new Vector3(GridSize.x, GridSize.y, 0.1f);

        float offsetX = (GridSize.x % 2 == 0) ? -0.5f : 0f;
        float offsetY = (GridSize.y % 2 == 0) ? -0.5f : 0f;
        float offsetZ = (GridSize.z % 2 == 0) ? -0.5f : 0f;

        float xPlanePos = GridSize.x / 2f;
        float yPlanePos = GridSize.y / 2f;
        float zPlanePos = GridSize.z / 2f;

        _xPlane.position = new Vector3(xPlanePos + offsetX, offsetY, offsetZ);
        _yPlane.position = new Vector3(offsetX, yPlanePos + offsetY, offsetZ);
        _zPlane.position = new Vector3(offsetX, offsetY, zPlanePos + offsetZ);

        _minXPlane.position = new Vector3(-xPlanePos + offsetX, offsetY, offsetZ);
        _minYPlane.position = new Vector3(offsetX,-yPlanePos + offsetY,offsetZ);
        _minZPlane.position = new Vector3(offsetX, offsetY, -zPlanePos + offsetZ);

        _levelEditor.OnGridSizeCommand(GridSize);
    }   

    public void LoadGridSize(Vector3Int gridSize)
    {
        GridSize = gridSize;
      
        AdjustPlaneScales();
        AdjustSlider();
    }
    public void SetGridSizeAll(Vector3Int newSize)
    {
        GridSize = newSize;
        _textX.text = $"X: {GridSize.x}";
        _textY.text = $"Y: {GridSize.y}";
        _textZ.text = $"Z: {GridSize.z}";

        AdjustPlaneScales();
        AdjustSlider();
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

    private void AdjustSlider()
    {
        _sliderX.value = GridSize.x;
        _textX.text = "X: " + GridSize.x.ToString();

        _sliderY.value = GridSize.y;
        _textY.text = "Y: " + GridSize.y.ToString();

        _sliderZ.value = GridSize.z;
        _textZ.text = "Z: " + GridSize.z.ToString();
    }
}
