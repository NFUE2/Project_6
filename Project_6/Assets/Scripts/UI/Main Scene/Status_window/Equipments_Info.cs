using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipments_Info : MonoBehaviour
{

    public GameObject Information_Panel;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowInformation()
    {
        Information_Panel.SetActive(true);
    }
    
    public void HideInformation()
    {
        Information_Panel.SetActive(false);
    }
}
