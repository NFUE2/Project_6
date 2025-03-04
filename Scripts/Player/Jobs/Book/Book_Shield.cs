using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Book_Shield : MonoBehaviourPun
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttack"))
        {
            if (photonView.IsMine) PhotonNetwork.Destroy(collision.gameObject);
                //Destroy(collision.gameObject);
        }
    }

    public void SetParent(int index)
    {
        photonView.RPC(nameof(SetParentRPC),RpcTarget.All,index);
    }

    [PunRPC]
    public void SetParentRPC(int index)
    {
        transform.SetParent(GameManager.instance.players[index].transform);
        transform.localPosition = new Vector3(0,0.35f,0f);
    }
}
