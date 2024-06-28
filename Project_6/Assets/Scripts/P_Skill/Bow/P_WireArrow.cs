using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_WireArrow : MonoBehaviour
{
    public float arrowSpeed;
    public float wireSpeed = 1f;
    private bool isCollision;

    public Transform player;
    public float wireEndDistance;

    private void Update()
    {
        if(!isCollision) transform.position += transform.right * arrowSpeed * Time.deltaTime;
        else
        {
            Debug.Log(1);
            player.position = Vector2.Lerp(player.position,transform.position + transform.right,Time.deltaTime * wireSpeed);

            if (Vector3.Distance(transform.position, player.position) < wireEndDistance)
                Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isCollision = true;

    }
}
