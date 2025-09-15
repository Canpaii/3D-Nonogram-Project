using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    // This will be done really cursed, But I simply dont know any other method
    // I will make a box made out of planes, and with a raycast I will ignore the first plane hit. 
    // With that I'm able to get "Inside" the box and place my voxel with a second raycast that hits the other plane
    [SerializeField] private GameObject _xPlane, _minXPlane;
    [SerializeField] private GameObject _yPlane, _minYPlane;
    [SerializeField] private GameObject _zPlane, _minZPlane;
    [field: SerializeField] public Vector3Int GridSize { get; private set; }
    public void SetGridSize(Vector3Int size)
    {
        GridSize = size;
        AdjustPlaneScales();
    }

    public void SetGridSizeTest()
    {
        AdjustPlaneScales();
    }

    public void AdjustPlaneScales()
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
    }
}
