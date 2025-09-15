using UnityEngine;
public class VoxelData : MonoBehaviour
{
    public Vector3Int GridPosition { get; private set; }
    public Color VoxelColor { get; private set; }
    private Renderer rend;

    public void Initialize(Vector3Int pos, Color color )
    {
        GridPosition = pos;

        this.VoxelColor = color;

        rend = GetComponent<Renderer>();
        rend.material.color = VoxelColor;
    }

    public void SetColor(Color color) => VoxelColor = color;    
}
