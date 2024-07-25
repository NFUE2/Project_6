using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public CameraController cam { get; private set; }
    public GameObject player;
    public List<GameObject> players = new List<GameObject>();

    public override void Awake()
    {
        base.Awake();
        cam = Camera.main.GetComponent<CameraController>();
    }
}
