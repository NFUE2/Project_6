using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Close_Inventory : MonoBehaviour
{

    public GameObject Inventory;

    public void CloseInventory()
    {
        Inventory.SetActive(false);
    }
}
