using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_WireArrow : MonoBehaviour
{
    public float arrowSpeed;
    public float wireSpeed = 1f;
    public bool isCollision;
    public Transform player;
    public float wireEndDistance;

    private void Update()
    {
        if(isCollision) transform.position += transform.right * arrowSpeed * Time.deltaTime;
        else
        {
            Vector2.Lerp(player.position,transform.position,Time.deltaTime * wireSpeed);

            if (Vector3.Distance(transform.position, player.position) < wireEndDistance)
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isCollision = true;
    }
}
