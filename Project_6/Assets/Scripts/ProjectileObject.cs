using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileObject : MonoBehaviourPun
{
    public ProjectileDataSO data;
    float durationTime;

    private void Start()
    {
        durationTime = 5f;
    }

    private void Update()
    {
        if (durationTime < 0) DestroyObject();

        transform.position += transform.right * data.moveSpeed * Time.deltaTime;
        durationTime -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layerValue = data.target.value;
        int colLayer = collision.gameObject.layer;

        if (layerValue == 1 << colLayer && collision.TryGetComponent(out IDamagable target))
        {
            target.TakeDamage(data.damage);
            DestroyObject();
            //if(PhotonNetwork.IsMasterClient) PhotonNetwork.Destroy(gameObject);
            //photonView.RPC(nameof(Destroy),RpcTarget.MasterClient);
        }
    }

    //[PunRPC]
    private void DestroyObject()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
