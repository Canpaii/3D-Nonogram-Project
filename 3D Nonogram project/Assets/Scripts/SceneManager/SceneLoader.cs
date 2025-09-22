using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("The name of the scene to load.")]
    public string sceneName;



    // Store the scene name globally so you can use it for JSON lookup
    public static string currentSceneKey;

    private void Awake()
    {

        DontDestroyOnLoad(gameObject);

    }

    // Call this from a Button OnClick() in the Inspector
    public void LoadScene()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("SceneLoader: No scene name set!");
            return;
        }

        currentSceneKey = sceneName; // save the key for JSON lookup
        SceneManager.LoadScene(sceneName);
    }

    // Optional: Load a scene by string argument directly
    public void LoadScene(string newSceneName)
    {
        if (string.IsNullOrEmpty(newSceneName))
        {
            Debug.LogWarning("SceneLoader: No scene name provided!");
            return;
        }

        currentSceneKey = newSceneName;
        SceneManager.LoadScene(newSceneName);
    }
}
