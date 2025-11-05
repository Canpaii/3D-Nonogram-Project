using UnityEngine;

public class PausePanel : MonoBehaviour
{
    private bool paused = false;
    [SerializeField] private GameObject pausePanel; 
    public void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && !paused)
        {
            paused = true;
            pausePanel.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.Escape) && paused) 
        { 
            paused = false;
            pausePanel.SetActive(false);
        }
        
    }
}
