using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using System;

//public enum MonsterAttackType
//{
//    Single, //���� ����
//    Multi, //���� �ټ�
//    Long //���Ÿ�
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


    //List<Transform> players = new List<Transform>(); //���� �Ŵ������� ���������� ����
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
        //���ݹ���
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector3)offsetPos + transform.position, data.attackDistance);

        //Ž������
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere((Vector3)offsetPos + transform.position, data.searchDistance);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
