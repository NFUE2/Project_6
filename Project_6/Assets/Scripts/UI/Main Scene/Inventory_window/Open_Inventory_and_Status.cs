using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open_Inventory_and_Status : MonoBehaviour
{

    public GameObject inventory;
    public GameObject Status;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OpenInventory();
    }


    public void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.SetActive(true);
            Status.SetActive(true);
        }
    }
}
