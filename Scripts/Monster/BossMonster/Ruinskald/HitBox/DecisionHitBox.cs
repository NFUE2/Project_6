using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionHitBox : MonoBehaviour
{
    private float curDuration;
    private float duration;
    private void Start()
    {
        curDuration = 0f;
        duration = 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        curDuration += Time.deltaTime;
        if (curDuration >= duration)
        {
            Destroy(gameObject);
        }
    }
}
