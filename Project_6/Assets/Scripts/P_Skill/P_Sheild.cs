using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Sheild : MonoBehaviour,P_IDamagable
{
    public float amount;

    public void TakeDamage(float damage)
    {
        amount -= damage;

        if(amount <= 0) PhotonNetwork.Destroy(gameObject);
    }
}
