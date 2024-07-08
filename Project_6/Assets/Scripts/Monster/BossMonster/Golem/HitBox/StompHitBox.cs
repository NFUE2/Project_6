using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompHitBox : MonoBehaviour
{
    private float duration = 0.2f;
    private float curDuration = 0f;

    private void OnEnable()
    {
        curDuration = 0f;
    }

    private void Update()
    {
        curDuration += Time.deltaTime;
        if(curDuration >= duration) 
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"{collision.gameObject.name}가 스톰프에 피격되었습니다.");
        }
    }
}
