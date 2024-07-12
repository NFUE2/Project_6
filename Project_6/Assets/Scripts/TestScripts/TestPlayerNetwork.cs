using Photon.Pun;
using UnityEngine;

public class TestPlayerNetwork : MonoBehaviourPun
{
    public Component[] components;

    private void Start()
    {
        if(!photonView.IsMine)
            foreach(var c in components) Destroy(c);

        Destroy(this);
    }
}
