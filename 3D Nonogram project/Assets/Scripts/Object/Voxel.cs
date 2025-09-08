using UnityEngine;
using TMPro;

public enum VoxelState
{
    Base,
    Marked,
    Painted,
    Finished,
}

public class Voxel : MonoBehaviour
{
    // All sides of the voxels
    // these text components are what hold numbers on the voxel

    public TMP_Text xAxis;
    public TMP_Text yAxis;
    public TMP_Text zAxis;

    public TMP_Text minXAxis;
    public TMP_Text minYAxis;
    public TMP_Text minZAxis;

    [Header("Materials and colors")]
    public Color FinalColor {get; private set;}

    public Material baseMaterial;
    public Material markedMaterial;
    public Material colloredMaterial;
    
    private Renderer rend;

    private VoxelState state;
    public void Awake()
    {
        rend = GetComponent<Renderer>();
    }
    public void Initialize(Color color)
    {
        FinalColor = color;
    }

    // I have to find a way to properly set these numbers 
    public void SetNumber(string number)
    {

    }
    public void ChangeVoxxelState(VoxelState state)
    {
        this.state = state;

        switch (state)
        {
            case VoxelState.Base:
                rend.material = baseMaterial;
                break;

            case VoxelState.Marked:
                rend.material = markedMaterial;
                break;

            case VoxelState.Painted:
                rend.material = colloredMaterial;
                break;

            case VoxelState.Finished:
                rend.material = baseMaterial;
                rend.material.color = FinalColor;
                break;

        }
    }
}

