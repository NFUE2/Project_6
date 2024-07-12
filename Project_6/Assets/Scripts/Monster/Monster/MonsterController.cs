using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public enum MonsterAttackType
{
    Single, //���� ����
    Multi, //���� �ټ�
    Long //���Ÿ�
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

    //List<Transform> players = new List<Transform>(); //���� �Ŵ������� ���������� ����
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
        //���ݹ���
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.attackDamage);

        //Ž������
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,data.searchDistance);
    }
}
