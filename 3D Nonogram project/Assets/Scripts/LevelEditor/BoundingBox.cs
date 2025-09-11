using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    // This will be done really cursed, But I simply dont know any other method
    // I will make a box made out of planes, and with a raycast I will ignore the first plane hit. 
    // With that I'm able to get "Inside" the box and place my voxel with a second raycast that hits the other plane
    [SerializeField] private GameObject xPlane, minXPlane;
    [SerializeField] private GameObject yPlane, minYPlane;
    [SerializeField] private GameObject zPlane, minZPlane;

    public Vector3Int gridSize { get; private set; }

    public void SetGridSize(Vector3Int size)
    {
        gridSize = size;
        AdjustPlaneScales();
    }

    public void AdjustPlaneScales()
    {

    }
}
