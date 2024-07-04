using UnityEngine;

public class P_Open_Inventory : MonoBehaviour
{

    public GameObject Inventory;


    public void OpenInventory()
    {
        Inventory.SetActive(true);
    }
}
