using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : NPCBase
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactionUI.SetActive(true);

        if(collision.TryGetComponent(out PhotonView pv) && pv.IsMine &&Input.GetKeyDown(KeyCode.F))
        {
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactionUI.SetActive(false);
    }
}
