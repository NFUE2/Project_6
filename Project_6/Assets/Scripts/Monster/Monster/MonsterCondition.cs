using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;


public class MonsterCondition : MonoBehaviour,IDamagable//,IPunObservable
{
    MonsterController controller;

    public Image hpBar;
    //public event Action OnAlive;
    public event Action OnDie;
    public event Action OnSpawn;

    float curHP;

    private void Awake()
    {
        controller = GetComponent<MonsterController>();
        OnSpawn += SetHP;
    }

    private void OnEnable()
    {
        OnSpawn?.Invoke();
    }

    public void TakeDamage(float damage)
    {
        GameObject hpParent = hpBar.transform.parent.gameObject;
        hpParent.SetActive(true);

        curHP = Mathf.Clamp(curHP - damage, 0, controller.data.maxHP);
        hpBar.fillAmount = curHP / controller.data.maxHP;

        if (curHP == 0)
        {
            hpParent.SetActive(false);
            OnDie?.Invoke();
        }
    }

    private void SetHP()
    {
        curHP = controller.data.maxHP;
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if(stream.IsWriting)
    //    {
    //        stream.SendNext(curHP);
    //    }
    //    else
    //    {
    //        curHP = (float)stream.ReceiveNext();
    //    }
    //}
}
