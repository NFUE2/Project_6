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

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = destination.position;
    }
}
