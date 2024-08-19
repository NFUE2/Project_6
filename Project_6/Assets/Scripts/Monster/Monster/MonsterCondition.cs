using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class MonsterCondition : MonoBehaviourPun,IDamagable, IPunDamagable
{
    MonsterController controller;
    public SpriteRenderer sprite;

    public Image hpBar;
    //public event Action OnAlive;
    public event Action OnDie;
    public event Action OnSpawn;

    public float curHP { get; private set; }

    private void Awake()
    {
        controller = GetComponent<MonsterController>();
        OnSpawn += init;
    }

    private void OnEnable()
    {
        OnSpawn?.Invoke();
    }

    public void TakeDamage(float damage)
    {
        GameObject hpParent = hpBar.transform.parent.gameObject;
        hpParent.SetActive(true);
        controller.Hit();

        curHP = Mathf.Clamp(curHP - damage, 0, controller.data.maxHP);
        hpBar.fillAmount = curHP / controller.data.maxHP;

        if (curHP == 0) 
        {
            hpParent.SetActive(false);
            OnDie?.Invoke();
        }

        else StartCoroutine(Damaged());
    }

    private void OnDisable()
    {
        GameObject hpParent = hpBar.transform.parent.gameObject;
        hpParent.SetActive(false);
    }

    private void init() => curHP = controller.data.maxHP;

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

    //public void Damage(float damage)
    //{
    //    photonView.RPC(nameof(DamageRPC),RpcTarget.All,damage);
    //}

    [PunRPC]
    public void DamageRPC(float damage)
    {
        TakeDamage(damage);
    }

    public void Damage(float damage)
    {
        photonView.RPC(nameof(DamageRPC), RpcTarget.All, damage);
    }
}
