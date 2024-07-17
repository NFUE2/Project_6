using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using System;

//public enum MonsterAttackType
//{
//    Single, //근접 단일
//    Multi, //근접 다수
//    Long //원거리
//}

//[RequireComponent(typeof(PhotonView),typeof(Rigidbody2D),typeof(Animator))]
public class MonsterController : MonoBehaviour
{
    [Header("EnemyData")]
    public EnemyDataSO data;
    public Animator animator { get; private set; }
    [field : SerializeField] public Rigidbody2D rigidbody { get; private set; }
    public Vector2 offsetPos { get; private set; }

    public MonsterCondition condition;
    public bool isRight;

    //[Header("AttackType")]
    //public MonsterAttackType type;

    //[Header("Target")]
    //public LayerMask targetLayer;

    MonsterStateMachine stateMachine;

    [field : Header("Animation")]
    [field: SerializeField] public MonsterAnimationData animationData { get; private set; }


    //List<Transform> players = new List<Transform>(); //게임 매니저에서 가져오도록 설정
    [HideInInspector]
    public Transform target = null;

    private void Awake()
    {
        //rigidbody = GetComponent<Rigidbody2D>();
        animationData.Initialize();
        animator = GetComponent<Animator>();
        stateMachine = new MonsterStateMachine(this);
        offsetPos = GetComponent<Collider2D>().offset;
        //GetComponent<MonsterCondition>().OnDie += Die;
        //condition.OnDie += Die;
    }

    private void Update()
    {
        stateMachine.HandleInput(true);
    }

    private void OnDrawGizmos()
    {
        //공격범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector3)offsetPos + transform.position, data.attackDistance);

        //탐색범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere((Vector3)offsetPos + transform.position, data.searchDistance);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
