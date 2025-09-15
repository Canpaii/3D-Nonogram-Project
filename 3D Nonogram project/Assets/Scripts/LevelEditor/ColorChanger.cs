using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] private LevelEditor _levelEditor;

    [SerializeField] private Material[] _material;
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private Transform _buttonParent;
    [SerializeField] private Image _currentColorDisplay;

    private void Start()
    {
       // GenerateButtons();
    }
    //public void GenerateButtons()
    //{
    //    foreach (Material c in _material)
    //    {
    //        GameObject btnObj = Instantiate(_buttonPrefab, _buttonParent);
    //        var btnImage = btnObj.GetComponent<Image>();
    //        btnImage.color = c;

    //        var btn = btnObj.GetComponent<Button>();
    //        btn.onClick.AddListener(() => SetColor(c));
    //    }
    //}

    private void SetColor(Color color)
    {
        _levelEditor.SetColor(color);
    }
}
