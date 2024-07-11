using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : Singleton<TestGameManager>
{
    public TestCameraController cam { get; private set; }
    public GameObject player;
    public List<GameObject> players = new List<GameObject>();

    public override void Awake()
    {
        base.Awake();
        cam = Camera.main.GetComponent<TestCameraController>();
    }
}
