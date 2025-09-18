using System;
using UnityEngine;

[Serializable]
public struct PanelsToChange
{
    public string name;
    public GameObject[] panelToTurnOn;
    public GameObject[] panelToTurnOff;
}
public class TurnOnOffPanels : MonoBehaviour
{
    public PanelsToChange[] panels;

    public void ChangePanels(int i)
    {
        for (int j = 0; j < panels[i].panelToTurnOff.Length; j++)
        {
            panels[i].panelToTurnOff[j].SetActive(false);
        }
        for (int k = 0; k < panels[i].panelToTurnOn.Length; k++)
        {
            panels[i].panelToTurnOn[k].SetActive(true);
        }
    }
}
