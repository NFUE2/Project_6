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
    
    private void Start()
    {
        switch (type)
        {
            case DeleteType.NotisMaster:
                Delete();
                break;

            case DeleteType.NotisMine:
                Delete();
                break;
        }
    }

    void Delete()
    {
        if(!photonView.IsMine)
        {
            foreach(var c in components) Destroy(c);
            foreach (var g in gameObjects) Destroy(g);
        }

        Destroy(this);
    }
}
