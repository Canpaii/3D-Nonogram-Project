using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class CheckServerStatus : MonoBehaviour
{
    [SerializeField] private TMP_Text serverStatusText;
    [SerializeField] private TMP_Text extraInfo;
    [SerializeField] private TMP_Text levelSelectionServerStatus;

    [SerializeField] private Color green;
    [SerializeField] private Color red;
    
    private void Start()
    {
        StartCoroutine(CheckStatus());
    }
    private IEnumerator CheckStatus()
    {
        string url = "https://db08756d6d51.ngrok-free.app/list";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            levelSelectionServerStatus.enabled = false;

            serverStatusText.text = "Server Online";
            serverStatusText.color = green;
        }
        else
        {
            levelSelectionServerStatus.enabled = true;

            serverStatusText.text = "Server Offline";
            extraInfo.text = "You can still use the level creation to create and locally save levels.";

            serverStatusText.color = red;
        }
    }
}
