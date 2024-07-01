using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Sheild : MonoBehaviour,P_IDamagable
{
    public float amount;
    public float shieldTime = 5.0f;
    private void Start()
    {
        Invoke("DestroyObject", shieldTime);
    }

    public void TakeDamage(float damage)
    {
        amount -= damage;
        Debug.Log(1);
        if(amount <= 0) DestroyObject();
    }

    private void DestroyObject()
    {
        PhotonNetwork.Destroy(transform.parent.gameObject);
    }
}
