using System;
using UnityEngine;

[Serializable]
public struct PanelsToChange
{
    public GameObject panelToTurnOn;
    public GameObject panelToTurnOff;
}
public class TurnOnOffPanels : MonoBehaviour
{
    public PanelsToChange[] panels;

    public void ChangePanels(int i)
    {
        panels[i].panelToTurnOn.SetActive(true);
        panels[i].panelToTurnOff.SetActive(false);
    }
}
