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

    public TMP_Text minXAxis, minYAxis, minZAxis;

    [Header("Voxel state")]
    private VoxelState state;
    private VoxelState previousState;
    public bool PuzzleVoxel { get; private set; } // Determines if the voxel is part of the puzzle or filler

    [Header("Materials and colors")]
    public Color FinalColor { get; private set; }

    [SerializeField] private Material baseMaterial;
    [SerializeField] private Material markedMaterial;
    [SerializeField] private Material paintedMaterial;

    private Renderer _rend;


    private PuzzleManager puzzleManager; // Reference to central manager

    public void Awake()
    {
        _rend = GetComponent<Renderer>();
    }
    public void Initialize(bool isPuzzleVoxel, PuzzleManager manager)
    {
        PuzzleVoxel = isPuzzleVoxel;
        puzzleManager = manager;
        _rend = GetComponent<Renderer>();
        _rend.material = baseMaterial;
    }
    public void SetClue(Axis axis, string number)
    {
        switch (axis)
        {
            case Axis.X:
                minXAxis.text = number;
                xAxis.text = number;
                break;

            case Axis.Y:
                minYAxis.text = number;
                yAxis.text = number;
                break;

            case Axis.Z:
                minZAxis.text = number;
                zAxis.text = number;
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

            case InteractionType.Mark:
                MarkVoxel();
                break;
            case InteractionType.Paint:
                TryPaint();
                break;
            case InteractionType.Destroy:
                TryDestroy();
                break;

        }
    }

    private void TryPaint()
    {
        if (!PuzzleVoxel)
        {
            puzzleManager.ShowFeedback("Incorrect! That voxel shouldn't be painted.");
            return;
        }

        if (state != VoxelState.Painted)
        {
            state = VoxelState.Painted;
            _rend.material = paintedMaterial;
            puzzleManager.OnVoxelPainted();
        }
    }

    private void TryDestroy()
    {
        if (PuzzleVoxel)
        {
            puzzleManager.ShowFeedback("Incorrect! That voxel is part of the puzzle.");
            return;
        }

        if (state != VoxelState.Destroyed)
        {
            state = VoxelState.Destroyed;
            _rend.enabled = false; // hide it
            puzzleManager.OnVoxelDestroyed();
        }
    }

    private void MarkVoxel()
    {
        _rend.material = markedMaterial;
    }

}
