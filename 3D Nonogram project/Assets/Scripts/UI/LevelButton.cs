using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelButton : MonoBehaviour
{
    private string levelName;

    [SerializeField] private TMP_Text text;
    public void Initialize(string name, DownloadLevels downloadLvls)
    {
        levelName = name;

        text.text = name;
    }
}
