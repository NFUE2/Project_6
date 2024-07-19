using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundControl : MonoBehaviour
{
    private Transform cam;
    public Transform[] background;
    public float distance;

    private void Start()
    {
        cam = TestGameManager.instance.cam.transform;
    }
    private void Update()
    {
        SetBackground();
    }
    private void SetBackground() //캐릭터 이동 이벤트에 추가
    {
        for(int i = 0; i < background.Length; i++)
        {
            Transform b = background[i];

            if (distance < Vector2.Distance(b.position, cam.position))
            {
                Vector2 dir = b.position - cam.position;
                b.position += dir.x > 0 ? new Vector3(-distance * 2, 0) : new Vector3(distance * 2, 0);
            }
        }

        //foreach(Transform t in background)
        //{
        //    Debug.Log(t.position);
        //    Debug.Log(cam.position);

        //    if (distance < Vector2.Distance(t.position,cam.position))
        //    {
        //        Vector2 dir = t.position - cam.position;
        //        t.position += dir.x > 0 ? new Vector3(-distance,0) : new Vector3(distance,0);
        //    }
        //}
    }
}
