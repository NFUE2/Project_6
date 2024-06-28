using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Dispenser : MonoBehaviour
{
    private float targetTime = 5f;
    private float curTime = 0;


    void Update()
    {
        curTime += Time.deltaTime;
        if (curTime >= targetTime)
        {
            Destroy(gameObject);
        }
    }
}
