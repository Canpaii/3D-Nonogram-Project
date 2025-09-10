using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] private LevelEditor _levelEditor;

    public Color[] colors;
    public GameObject buttonPrefab;
    public Transform buttonParent;
    public Image currentColorDisplay;

    public void GenerateButtons()
    {
        foreach (Color c in colors)
        {
            GameObject btnObj = Instantiate(buttonPrefab, buttonParent);
            var btnImage = btnObj.GetComponent<Image>();
            btnImage.color = c;

            var btn = btnObj.GetComponent<Button>();
            btn.onClick.AddListener(() => SetColor(c));
        }
    }

    private void SetColor(Color color)
    {
        _levelEditor.SetColor(color);
    }
}
