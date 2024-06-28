using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_SwordProjectile : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        Debug.Log(transform.right);
        transform.position += transform.right * speed * Time.deltaTime;
    }
}
