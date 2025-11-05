using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text feedbackText;

    [SerializeField] private int totalPuzzleVoxels;
    [SerializeField] private int totalFillerVoxels;
    [SerializeField] private int paintedCount;
    [SerializeField] private int destroyedCount;

    public List<Voxel> puzzleVoxelList = new List<Voxel>();


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

            ChangeColor();
            ShowFeedback("Puzzle Complete!");
        }
    }

    public void ShowFeedback(string message)
    {
        if (feedbackText == null) return;
        feedbackText.text = message;
        feedbackTimer = 3f; 
    }

    public void ChangeColor()
    {
        foreach (Voxel v in puzzleVoxelList) 
        { 
            v.DisplayFinalColor();
            v.SetCluesToZero();
        }
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
