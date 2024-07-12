using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float damage;

    private void Start()
    {
        Invoke("DestroyObject",5.0f);
    }

    private void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out P_BossMonster boss)) //향후 맞는 판정 수정
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
