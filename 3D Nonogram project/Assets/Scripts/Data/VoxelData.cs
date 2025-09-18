using UnityEngine;
public class VoxelData : MonoBehaviour
{
    public Vector3Int GridPosition { get; private set; }
    public Color VoxelColor { get; private set; }
    private Renderer _rend;
    private void Awake()
    {
        _rend = GetComponent<Renderer>();
    }
    public void Initialize(Vector3Int pos, Color color )
    {
        GridPosition = pos;

        this.VoxelColor = color;

        _rend = GetComponent<Renderer>();
        _rend.material.color = VoxelColor;
    }

    public void SetColor(Color color)
    {   
        VoxelColor = color;
        _rend.material.color = VoxelColor;
    }
    public void SetGridPosition(Vector3Int pos) => GridPosition = pos;
}
