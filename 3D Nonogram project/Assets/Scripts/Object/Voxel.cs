using UnityEngine;
using TMPro;

public enum VoxelState
{
    Base,
    Marked,
    Painted,
    Finished,
    Destroyed,
}

public enum Axis
{
    X,
    Y, 
    Z,
}

public class Voxel : MonoBehaviour
{
    // All sides of the voxels
    // these text components are what hold clues(number) on the voxel

    [Header("Worldspace tekst")]
    public TMP_Text xAxis, yAxis, zAxis;

    public TMP_Text minXAxis, minYAxis,minZAxis;
  
    [Header("Voxel state")]
    private VoxelState state;
    private VoxelState previousState;
    public bool PuzzleVoxel { get; private set; } // Determines if the voxel is part of the puzzle or filler

    [Header("Materials and colors")]
    public Color FinalColor {get; private set;}

    [SerializeField] private Material baseMaterial;
    [SerializeField] private Material markedMaterial;
    [SerializeField] private Material colloredMaterial;
    
    private Renderer _rend;

    public void Awake()
    {
        _rend = GetComponent<Renderer>();
    }
    public void Initialize(Color color)
    {
        FinalColor = color;
    }

    public void SetClue(Axis axis, string number, bool isMinSide)
    {
        switch (axis)
        {
            case Axis.X:
                if (isMinSide) minXAxis.text = number;
                else xAxis.text = number;
                break;

            case Axis.Y:
                if (isMinSide) minYAxis.text = number;
                else yAxis.text = number;
                break;

            case Axis.Z:
                if (isMinSide) minZAxis.text = number;
                else zAxis.text = number;
                break;
        }
    }


    public void SetVoxelType(bool b)
    {
        PuzzleVoxel = b;
    }

    public void HandleClick(InteractionType iType)
    {
        switch (iType)
        {
            case InteractionType.Drag:
                print("Drag");
                // Do nothing, probably wanted to drag through the level
                break;
            case InteractionType.Mark:
                ChangeVoxelState(VoxelState.Marked);
                break;
            case InteractionType.Paint:
                ChangeVoxelState(VoxelState.Painted);
                break;
            case InteractionType.Destroy:
                ChangeVoxelState(VoxelState.Destroyed); 
                break;
             
        }
    }
    private void ChangeVoxelState(VoxelState state)
    {
        previousState = state;
        this.state = state;

        switch (state)
        {
            case VoxelState.Base:
                _rend.material = baseMaterial;
                break;

            case VoxelState.Marked:
                _rend.material = markedMaterial;
                break;

            case VoxelState.Painted:
                if (!PuzzleVoxel)
                {
                    // Point penalty since it is the wrong pieze being painted 
                    state = previousState;

                    return;
                }

                _rend.material = colloredMaterial;
                break;

            case VoxelState.Finished:
                _rend.material = baseMaterial;
                _rend.material.color = FinalColor;
                break;

            case VoxelState.Destroyed:
                if (PuzzleVoxel)
                {
                    // Point penalty since it is the wrong piece being broken
                    state = previousState;

                    return;
                }

                break;
        }
    }
}

