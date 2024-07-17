using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_View : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject playerObject;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove(playerObject);
    }

    private void CameraMove(GameObject player)
    {
        mainCamera.transform.position = player.transform.position;
    }
}
