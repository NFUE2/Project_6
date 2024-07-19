using Photon.Pun;
using UnityEngine;

public enum DeleteType
{
    NotisMaster,
    NotisMine,
}


public class NetworkConditionDelete : MonoBehaviourPun
{
    public DeleteType type;

    public Component[] components;
    public GameObject[] gameObjects;
    
    private void Awake()
    {
        switch (type)
        {
            case DeleteType.NotisMaster:
                if(!PhotonNetwork.IsMasterClient) Delete();
                break;

            case DeleteType.NotisMine:
                if(!photonView.IsMine) Delete();
                break;
        }
    }

    void Delete()
    {
        foreach(var c in components) Destroy(c);
        foreach (var g in gameObjects) Destroy(g);

        Destroy(this);
    }
}
