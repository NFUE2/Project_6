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
//        playerControls = new PlayerControls(); // �߰�: PlayerControls �ν��Ͻ� ����
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
//        if (Mathf.Abs(rb.velocity.y) < 0.001f) // �÷��̾ ���� �ִ��� Ȯ��
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
//        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // ���콺�� ��ġ��
//        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized; // ���콺�� ��ġ������ �������� ���� ���� ��ġ���� ���� => ���� ����
//        //GameObject attackInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity); // �Ѿ��� ������ �߻��� �����Ѵ�.
//        GameObject attackInstance = PhotonNetwork.Instantiate("Prototype/" + attackPrefab.name, attackPoint.position, Quaternion.identity); // �Ѿ��� ������ �߻��� �����Ѵ�.

//        attackInstance.GetComponent<Rigidbody2D>().velocity = attackDirection * 15f; // ���� �ӵ� ���� �Ѵ�.

//        if (attackCount >= 6)
//        {
//            StartCoroutine(AttackCooldown());
//        }
//    }

//    public IEnumerator AttackCooldown() //6���� ���� ������ �Ѵ�.
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
//                // �������� �ٶ� ��
//                transform.localRotation = Quaternion.Euler(0, 0, 0); // �⺻ �������� ����
//            }
//            else if (direction.x < 0)
//            {
//                // ������ �ٶ� ��
//                transform.localRotation = Quaternion.Euler(0, 180, 0); // Y������ ȸ���Ͽ� �ݴ� �������� ����
//            }
//        }
//    }
//    private void SkillQ()
//    {
//        Debug.Log("SkillQ ���");
//        P_gunQ.SkillAction();
//    }

//    private void SkillE()
//    {
//        Debug.Log("SkillE ���");
//        P_gunE.SkillAction();
//    }
//}