using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
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

        // ȭ�� ��ǥ�� �̿��Ͽ� ȭ�� �ۿ� �ִ��� Ȯ��
        if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
        {
            Destroy(gameObject); // ȭ�� ������ ������ �� �Ѿ� �ı�
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
            Debug.Log("������ �������� �������ϴ�.");
        }
    }

    [PunRPC]
    private void DestroyObject()
    {
        //PhotonNetwork.Destroy(gameObject);
        Destroy(gameObject);
    }
}
