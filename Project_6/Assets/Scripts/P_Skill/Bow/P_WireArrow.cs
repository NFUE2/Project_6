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
            player.GetComponent<PlayerController_Bow>().isWiring = true;
            player.position = Vector2.Lerp(player.position,transform.position + transform.right,Time.deltaTime * wireSpeed);
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            if (Vector3.Distance(transform.position, player.position) < wireEndDistance)
            {
                Destroy(gameObject);
                player.GetComponent<PlayerController_Bow>().isWiring = false;

            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isCollision = true;

    }
}
