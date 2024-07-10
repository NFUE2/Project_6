using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CamType
{
    Normal,
    Boss
}

public class TestCameraController : MonoBehaviour
{
    Camera cam;
    public Transform target;
    CamType type;

    private void Awake()
    {
        cam = Camera.main;
        type = CamType.Normal;
    }

    private void LateUpdate()
    {
        if(target != null)
        {
            if (type == CamType.Normal) transform.position = new Vector3(target.position.x, 0, transform.position.z);

        }
    }
}
