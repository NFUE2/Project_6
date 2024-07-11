using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum MonsterAttackType
{
    Single, //근접 단일
    Multi, //근접 다수
    Long //원거리
}

[RequireComponent(typeof(PhotonView),typeof(Rigidbody2D),typeof(Animator))]
public class MonsterController : MonoBehaviour
{
    [Header("EnemyData")]
    public EnemyDataSO data;
    public Animator animtor { get; private set; }

    public MonsterAttackType type;
    public Rigidbody2D rigidbody;
    public LayerMask targetLayer;

    MonsterStateMachine stateMachine;

    [field : Header("Animation")]
    [field: SerializeField] public MonsterAnimationData animationData { get; private set; }

    //List<Transform> players = new List<Transform>(); //게임 매니저에서 가져오도록 설정
    public Transform target = null;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animtor = GetComponent<Animator>();
        stateMachine = new MonsterStateMachine(this);

        animationData.Initialize();
    }

    private void Update()
    {
        stateMachine.HandleInput(true);
    }
}
