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

    public float searchDistance { get; private set; }

    //public MonsterStageList stage;

    //[Header("AttackType")]
    //public MonsterAttackType type;

    //[Header("Target")]
    //public LayerMask targetLayer;

    MonsterStateMachine stateMachine;
    public RectTransform ui;

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
        condition.OnSpawn += () => searchDistance = data.searchDistance;
        searchDistance = data.searchDistance;
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
        Gizmos.DrawWireSphere(((Vector3)newOffset + transform.position), stateMachine.controller.searchDistance);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        target = null;
    }

    public void Hit() => searchDistance = 50f;

    private void ComponentToggle()
    {
        col.enabled = condition.curHP == 0 ? false : true;

        rigid.constraints = condition.curHP == 0 ?
            RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.FreezeRotation;
    }

    //생성될때 작동하는 함수
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        //transform.SetParent(GameManager.instance.enemyList);
        transform.SetParent(GameManager.instance.SpawnStage());
    }
}
