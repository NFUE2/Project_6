using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Bullet : MonoBehaviour
{
    public float speed;
    public float damage;

    private void Start()
    {
        Invoke("DestroyObject",5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out P_Tentaclypse boss))
        {
            boss.TakeDamage(damage);
            //Destroy(gameObject);
            DestroyObject();
        }
    }

    private void DestroyObject()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
