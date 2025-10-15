using UnityEngine;
using TMPro;

public class PuzzleManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text feedbackText;

    private int totalPuzzleVoxels;
    private int totalFillerVoxels;
    private int paintedCount;
    private int destroyedCount;

    private float feedbackTimer;

    private void Start()
    {
        if (winPanel) winPanel.SetActive(false);
    }

    public void Initialize(int puzzleCount, int fillerCount)
    {
        totalPuzzleVoxels = puzzleCount;
        totalFillerVoxels = fillerCount;
        paintedCount = 0;
        destroyedCount = 0;
    }

    public void OnVoxelPainted()
    {
        paintedCount++;
        CheckWin();
    }

    public void OnVoxelDestroyed()
    {
        destroyedCount++;
        CheckWin();
    }

    private void CheckWin()
    {
        if (paintedCount >= totalPuzzleVoxels && destroyedCount >= totalFillerVoxels)
        {
            if (winPanel != null)
                winPanel.SetActive(true);
            ShowFeedback("Puzzle Complete!");
        }
    }

    public void ShowFeedback(string message)
    {
        if (feedbackText == null) return;
        feedbackText.text = message;
        feedbackTimer = 3f; // Show for 2 seconds
    }

    private void Update()
    {
        if (feedbackTimer > 0)
        {
            feedbackTimer -= Time.deltaTime;
            if (feedbackTimer <= 0 && feedbackText != null)
                feedbackText.text = "";
        }
    }
}
