using UnityEngine;

public class ColorCardUIManager : MonoBehaviour
{
    private bool isActive = false;
    [SerializeField] private GameObject colorCardPanel;
    public void OnButtonPress()
    {
        if (isActive)
        {
            colorCardPanel.SetActive(false);
            isActive = false;
        }
        else
        {
            colorCardPanel.SetActive(true);
            isActive = true;
        }
    }
}
