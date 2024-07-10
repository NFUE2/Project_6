using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PhotonView),typeof(Rigidbody2D),typeof(Animator))]
public class MonsterController : MonoBehaviour
{
    public EnemyDataSO data;

    Animator animtor;
    Rigidbody2D rigidbody;
    MonsterStateMachine stateMachine;

    List<Transform> players = new List<Transform>();
    Transform target = null;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animtor = GetComponent<Animator>();

        stateMachine = new MonsterStateMachine(this);
    }

    private void Update()
    {
        //if(target == null)
        //{
        //    foreach(Transform p in players)
        //    {
        //        float distance = Vector3.Distance(transform.position,target.position);

        //    }
        //}
    }
}
