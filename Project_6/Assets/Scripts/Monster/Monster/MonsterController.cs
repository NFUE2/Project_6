using Photon.Pun;
using UnityEngine;

//public enum MonsterAttackType
//{
//    Single, //근접 단일
//    Multi, //근접 다수
//    Long //원거리
//}

//[RequireComponent(typeof(PhotonView),typeof(Rigidbody2D),typeof(Animator))]
public class MonsterController : MonoBehaviourPun,IPunInstantiateMagicCallback
{
    [Header("EnemyData")]
    public EnemyDataSO data;
    public Animator animator { get; private set; }
    public Rigidbody2D rigid;
    public Collider2D col;

    public Vector2 offsetPos;
    public MonsterCondition condition;
    public bool isRight;

    public MonsterStageList stage;

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
        rigid = GetComponent<Rigidbody2D>();
        animationData.Initialize();
        animator = GetComponent<Animator>();
        stateMachine = new MonsterStateMachine(this);
        col = GetComponent<Collider2D>();
        offsetPos = col.offset;

        condition.OnDie += ComponentToggle;
        condition.OnSpawn += ComponentToggle;

        //GetComponent<MonsterCondition>().OnDie += Die;
        //condition.OnDie += Die;
    }

    private void Update()
    {
        stateMachine.HandleInput(true);
    }

    private void OnDrawGizmos()
    {
        float dir = transform.localScale.x == 1 ? offsetPos.x : -offsetPos.x;

        Vector2 newOffset = new Vector2(dir,offsetPos.y);

        //공격범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(((Vector3)newOffset + transform.position), data.attackDistance);

        //탐색범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(((Vector3)newOffset + transform.position), data.searchDistance);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private void ComponentToggle()
    {
        col.enabled = !col.enabled;

        rigid.constraints =
            rigid.constraints == RigidbodyConstraints2D.FreezeAll ?
            RigidbodyConstraints2D.FreezeRotation : RigidbodyConstraints2D.FreezeAll;
    }

    //생성될때 작동하는 함수
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        //transform.SetParent(GameManager.instance.enemyList);
        transform.SetParent(GameManager.instance.SpawnStage(stage));
    }
}
