using Photon.Pun;
using UnityEngine;

public class P_AllRoundAttackObject : MonoBehaviour
{
    private float keepingTime = 15;
    private float curTime = 0;

    PhotonView pv;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Update()
    {
        curTime += Time.deltaTime;
        if(curTime >= keepingTime)
        {
            //Destroy(gameObject);
            DestroyObject();
            //(복원)pv.RPC("DestroyObject", RpcTarget.All);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(collision.TryGetComponent<IDamagable>(out IDamagable player))
            {
                float damage = BossBattleManager.Instance.boss.attackPower;
                player.TakeDamage(damage);
                DestroyObject();
            }
            //Destroy(gameObject);
            //(복원)pv.RPC("DestroyObject", RpcTarget.All);

        }
        //else if (collision.TryGetComponent(out P_Sheild shield))
        //{
        //    shield.TakeDamage(tentaclypse.attackPower);
            //Destroy(gameObject);
            //DestroyObject();
       //     pv.RPC("DestroyObject", RpcTarget.All);
       // }
    }

    [PunRPC]
    private void DestroyObject()
    {
        Destroy(gameObject);
        //PhotonNetwork.Destroy(gameObject);
    }
}