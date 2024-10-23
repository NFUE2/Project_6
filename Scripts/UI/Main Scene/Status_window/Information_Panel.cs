using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Information_Panel : MonoBehaviour
{

    public GameObject InformationPanel;

    public void ShowInformation()
    {
        InformationPanel.SetActive(true);
    }
    
    public void HideInformation()
    {
        InformationPanel.SetActive(false);
    }
}
