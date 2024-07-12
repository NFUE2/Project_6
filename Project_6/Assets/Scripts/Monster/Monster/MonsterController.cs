using Photon.Pun;
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

    [Header("AttackType")]
    public MonsterAttackType type;

    [Header("Target")]
    public LayerMask targetLayer;

    //Rigidbody2D rigidbody;
    MonsterStateMachine stateMachine;

    [field : Header("Animation")]
    [field: SerializeField] public MonsterAnimationData animationData { get; private set; }

    //List<Transform> players = new List<Transform>(); //게임 매니저에서 가져오도록 설정
    [HideInInspector]
    public Transform target = null;

    private void Awake()
    {
        animationData.Initialize();
        //rigidbody = GetComponent<Rigidbody2D>();
        animtor = GetComponent<Animator>();
        stateMachine = new MonsterStateMachine(this);
    }

    private void Update()
    {
        stateMachine.HandleInput(true);
    }

    private void OnDrawGizmos()
    {
        //공격범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.attackDamage);

        //탐색범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,data.searchDistance);
    }
}
