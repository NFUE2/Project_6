using Photon.Pun;
using Photon.Pun.Demo.Procedural;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_ArrowScript : MonoBehaviour
{
    private Camera mainCamera;
    public float damage;
    PhotonView pv;
    void Start()
    {
        mainCamera = Camera.main;
        pv = GetComponent<PhotonView>();
        Invoke("DestroyObject", 5.0f);
    }

    void Update()
    {
        CheckIfOffscreen();
    }

    void CheckIfOffscreen()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);

        // 화면 좌표를 이용하여 화면 밖에 있는지 확인
        if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
        {
            //Destroy(gameObject); // 화면 밖으로 나갔을 때 총알 파괴
            pv.RPC("DestroyObject", RpcTarget.All);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out P_Tentaclypse boss))
        {
            boss.TakeDamage(damage);
            //Destroy(gameObject);
            //DestroyObject();
            pv.RPC("DestroyObject", RpcTarget.All);
            Debug.Log("적에게 데미지를 입혔습니다.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            pv.RPC("DestroyObject", RpcTarget.All);
        }
    }

    [PunRPC]
    private void DestroyObject()
    {
        //PhotonNetwork.Destroy(gameObject);
        Destroy(gameObject);
    }
}
