using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Close_Inventory : MonoBehaviour
{

    public GameObject Inventory;

    public void CloseInventory()
    {
        Inventory.SetActive(false);
    }
}
