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

    [Header("Worldspace text")]
    public TMP_Text xAxis, yAxis, zAxis;

    public TMP_Text minXAxis, minYAxis, minZAxis;

    [Header("Voxel state")]
    private VoxelState state = VoxelState.Base;
    public bool puzzleVoxel;// Determines if the voxel is part of the puzzle or filler

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
        puzzleVoxel = isPuzzleVoxel;
        puzzleManager = manager;
        _rend = GetComponent<Renderer>();
        state = VoxelState.Base;
        _rend.material = baseMaterial;
    }

    public void SetClue(Axis axis, string number)
    {
        // If clue is "0" or empty, hide it for clarity
        if (string.IsNullOrEmpty(number) || number == "0")
        {
            switch (axis)
            {
                case Axis.X:
                    minXAxis.text = "";
                    xAxis.text = "";
                    break;
                case Axis.Y:
                    minYAxis.text = "";
                    yAxis.text = "";
                    break;
                case Axis.Z:
                    minZAxis.text = "";
                    zAxis.text = "";
                    break;
            }
        }
        else
        {
            // Display the clue number on both positive and negative faces for this axis
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
        if (!puzzleVoxel)
        {
            puzzleManager.ShowFeedback("Incorrect! That voxel shouldn't be painted.");
            return;
        }
        if (state != VoxelState.Painted)
        {
            // Paint the voxel as it's part of the puzzle
            state = VoxelState.Painted;
            _rend.material = paintedMaterial;
            puzzleManager.OnVoxelPainted();
        }
    }

    private void TryDestroy()
    {
        if (puzzleVoxel)
        {
            puzzleManager.ShowFeedback("Incorrect! That voxel is part of the puzzle.");
            return;
        }
        if (state != VoxelState.Destroyed)
        {
            // Mark as destroyed and disable the voxel's visibility/collisions
            state = VoxelState.Destroyed;
            _rend.enabled = false;  // hide the cube's mesh
            // Hide all clue text on this voxel (so numbers disappear when it's destroyed)
            xAxis.text = ""; minXAxis.text = "";
            yAxis.text = ""; minYAxis.text = "";
            zAxis.text = ""; minZAxis.text = "";
            // Disable collider to prevent further clicks on this invisible voxel
            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;
            // Notify the PuzzleManager that a filler voxel was correctly destroyed
            puzzleManager.OnVoxelDestroyed();
        }
    }
    public void SetFinalColor(Color color)
    {
        FinalColor = color;
    }
    public void DisplayFinalColor()
    {
        _rend.material.color = FinalColor;
    }

    public void SetCluesToZero() // Set clues to 0 after a puzzle is finished to show the full artwork
    {
        minXAxis.text = "";
        xAxis.text = "";
        minYAxis.text = "";
        yAxis.text = "";
        minZAxis.text = "";
        zAxis.text = "";
    }
    private void MarkVoxel()
    {
        // Only allow marking/unmarking on voxels that are not already decided
        if (state == VoxelState.Base)
        {
            // Mark the voxel (player suspects this is an empty/filler voxel)
            state = VoxelState.Marked;
            _rend.material = markedMaterial;
        }
        else if (state == VoxelState.Marked)
        {
            // Un-mark the voxel (toggle off)
            state = VoxelState.Base;
            _rend.material = baseMaterial;
        }
        // If the voxel is Painted or Destroyed, we ignore marking (cannot mark a decided voxel).
    }

}
