using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTeleportObejct : MonoBehaviour//, TestIInteraction
{
    public Transform destination;

    //public void OnInteraction()
    //{
    //    //destination.position;
    //}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.position = destination.position;
        
    }
}
