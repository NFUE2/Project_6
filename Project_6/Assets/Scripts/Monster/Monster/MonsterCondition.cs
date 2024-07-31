using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class MonsterCondition : MonoBehaviour,IDamagable//,IPunObservable
{
    MonsterController controller;
    public SpriteRenderer sprite;

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
        curHP = Mathf.Clamp(curHP - damage, 0, controller.data.maxHP);

        if (curHP == 0) OnDie?.Invoke();
        else StartCoroutine(Damaged());
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

    IEnumerator Damaged()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        sprite.color = Color.white;
    }
}
