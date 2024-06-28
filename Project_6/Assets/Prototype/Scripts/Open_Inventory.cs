using UnityEngine;

public class Open_Inventory : MonoBehaviour
{

    public GameObject Inventory;


    public void OpenInventory()
    {
        Inventory.SetActive(true);
    }
}
