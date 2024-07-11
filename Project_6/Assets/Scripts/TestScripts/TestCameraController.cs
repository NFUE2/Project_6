using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraController : MonoBehaviour
{
    public Transform target;
    private void LateUpdate()
    {
        if(target != null)
            transform.position = new Vector3(target.position.x, 0, transform.position.z);
    }
}
