//using System;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using System.Collections;
//using Photon.Pun;
//using UnityEngine.UI;

//public class PlayerController_Gun : MonoBehaviourPunCallbacks
//{
//    public float moveSpeed = 5f;
//    public float jumpForce = 5f;
//    public Transform attackPoint;
//    public GameObject attackPrefab;
//    public BoxCollider2D meleeCollider;

//    private bool isAttackCooldown = false;
//    private int attackCount = 0;
//    private float cooldownDuration = 3f;

//    public float attackTime;
//    private float lastAttackTime;

//    private Rigidbody2D rb;
//    private Vector2 moveInput;
//    private bool isJumping;
//    private Camera mainCamera;
//    private SpriteRenderer spriteRenderer;
//    private PlayerControls playerControls;

//    private P_GunQ  P_gunQ;
//    private P_GunE  P_gunE;

//    public bool isRolling;
//    public bool fanningReady;

//    PhotonView pv;
//    public Text cooltimeQText, cooltimeEText;

//    private void Awake()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        mainCamera = Camera.main;
//        spriteRenderer = GetComponent<SpriteRenderer>();
//        playerControls = new PlayerControls(); // 추가: PlayerControls 인스턴스 생성
//        P_gunQ = GetComponent<P_GunQ>();
//        P_gunE = GetComponent<P_GunE>();

//        pv = GetComponent<PhotonView>();
//        //cooltimeQText = GameObject.Find("Skill_Q").GetComponentInChildren<Text>();
//        //cooltimeEText = GameObject.Find("Skill_E").GetComponentInChildren<Text>();
//    }

//    private void Start()
//    {
        
//    }

//    private void OnEnable()
//    {
//        if (!pv.IsMine) return;

//        playerControls.Player.Enable();

//        playerControls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
//        playerControls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

//        playerControls.Player.Jump.performed += ctx => Jump();
//        playerControls.Player.Attack.performed += ctx => Attack();
//        playerControls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
//        playerControls.Player.SkillQ.performed += ctx => SkillQ();
//        playerControls.Player.SkillE.performed += ctx => SkillE();
//    }

//    private void OnDisable()
//    {
//        playerControls.Player.Disable();
//    }

//    private void Update()
//    {
//        if (!pv.IsMine) return;

//        Look(Mouse.current.position.ReadValue());
//    }

//    private void FixedUpdate()
//    {
//        if (!pv.IsMine) return;

//        if (isRolling) return;
//        Move();
//    }

//    private void Move()
//    {
//        Vector2 movement = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
//        rb.velocity = movement;

//        if (isJumping)
//        {
//            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
//            isJumping = false;
//        }
//    }

//    private void Jump()
//    {
//        if (Mathf.Abs(rb.velocity.y) < 0.001f) // 플레이어가 땅에 있는지 확인
//        {
//            isJumping = true;
//        }
//    }

//    private void Attack()
//    {
//        if (isAttackCooldown) return;
//        if (fanningReady || isRolling) return;

//        if (Time.time - lastAttackTime < attackTime) return;
//        lastAttackTime = Time.time;

//        attackCount++;
//        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // 마우스의 위치값
//        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized; // 마우스의 위치값에서 지정해준 공격 시작 위치값을 뺀다 => 공격 방향
//        //GameObject attackInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity); // 총알을 생성해 발사해 공격한다.
//        GameObject attackInstance = PhotonNetwork.Instantiate("Prototype/" + attackPrefab.name, attackPoint.position, Quaternion.identity); // 총알을 생성해 발사해 공격한다.

//        attackInstance.GetComponent<Rigidbody2D>().velocity = attackDirection * 15f; // 공격 속도 설정 한다.

//        if (attackCount >= 6)
//        {
//            StartCoroutine(AttackCooldown());
//        }
//    }

//    public IEnumerator AttackCooldown() //6발을 쓰고 장전을 한다.
//    {
//        isAttackCooldown = true;
//        attackCount = 0;
//        yield return new WaitForSeconds(cooldownDuration);
//        isAttackCooldown = false;
//    }

//    private void Look(Vector2 lookInput)
//    {
//        {
//            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(lookInput);
//            Vector2 direction = mousePosition - (Vector2)transform.position;

//            if (direction.x > 0)
//            {
//                // 오른쪽을 바라볼 때
//                transform.localRotation = Quaternion.Euler(0, 0, 0); // 기본 방향으로 설정
//            }
//            else if (direction.x < 0)
//            {
//                // 왼쪽을 바라볼 때
//                transform.localRotation = Quaternion.Euler(0, 180, 0); // Y축으로 회전하여 반대 방향으로 설정
//            }
//        }
//    }
//    private void SkillQ()
//    {
//        Debug.Log("SkillQ 사용");
//        P_gunQ.SkillAction();
//    }

//    private void SkillE()
//    {
//        Debug.Log("SkillE 사용");
//        P_gunE.SkillAction();
//    }
//}