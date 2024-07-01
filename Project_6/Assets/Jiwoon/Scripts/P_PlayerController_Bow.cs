using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerController_Bow : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform attackPoint;
    public GameObject attackPrefab;
    public BoxCollider2D meleeCollider;

    public float attackTime;
    private float lastAttackTime;

    private bool isAttackCooldown = false;
    private int attackCount = 0;
    private float cooldownDuration = 0.5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isJumping;
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private PlayerControls playerControls;

    private P_BowQ P_BowQ;
    private P_BowE P_BowE;

    PhotonView pv;
    public Text cooltimeQText, cooltimeEText;
    public bool isWiring;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerControls = new PlayerControls(); // �߰�: PlayerControls �ν��Ͻ� ����
        P_BowQ = GetComponent<P_BowQ>();
        P_BowE = GetComponent<P_BowE>();

        pv = GetComponent<PhotonView>();
        cooltimeQText = GameObject.Find("Skill_Q").GetComponentInChildren<Text>();
        cooltimeEText = GameObject.Find("Skill_E").GetComponentInChildren<Text>();
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        if (!pv.IsMine) return;

        playerControls.Player.Enable();

        playerControls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerControls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        playerControls.Player.Jump.performed += ctx => Jump();
        playerControls.Player.Attack.performed += ctx => Attack();
        playerControls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
        playerControls.Player.SkillQ.performed += ctx => SkillQ();
        playerControls.Player.SkillE.performed += ctx => SkillE();
    }

    private void OnDisable()
    {
        playerControls.Player.Disable();
    }

    private void Update()
    {
        if (!pv.IsMine) return;

        Look(Mouse.current.position.ReadValue());
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine) return;

        if (isWiring) return;
        Move();
    }

    private void Move()
    {
        Vector2 movement = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        rb.velocity = movement;

        if (isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = false;
        }
    }

    private void Jump()
    {
        if (Mathf.Abs(rb.velocity.y) < 0.001f) // �÷��̾ ���� �ִ��� Ȯ��
        {
            isJumping = true;
        }
    }

    private void Attack()
    {
        if (isAttackCooldown) return;

        if (Time.time - lastAttackTime < attackTime) return;
        lastAttackTime = Time.time;

        attackCount++;
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // ���콺�� ��ġ��
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized; // ���콺�� ��ġ������ �������� ���� ���� ��ġ���� ���� => ���� ����
        GameObject attackInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity); // �Ѿ��� ������ �߻��� �����Ѵ�.
        attackInstance.GetComponent<Rigidbody2D>().velocity = attackDirection * 15f; // ���� �ӵ� ���� �Ѵ�.

        if (attackCount >= 1)
        {
            Debug.Log(1);
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown() //6���� ���� ������ �Ѵ�.
    {
        isAttackCooldown = true;
        attackCount = 0;
        yield return new WaitForSeconds(cooldownDuration);
        isAttackCooldown = false;
    }

    private void Look(Vector2 lookInput)
    {
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(lookInput);
            Vector2 direction = mousePosition - (Vector2)transform.position;

            if (direction.x > 0)
            {
                // �������� �ٶ� ��
                transform.localRotation = Quaternion.Euler(0, 0, 0); // �⺻ �������� ����
            }
            else if (direction.x < 0)
            {
                // ������ �ٶ� ��
                transform.localRotation = Quaternion.Euler(0, 180, 0); // Y������ ȸ���Ͽ� �ݴ� �������� ����
            }
        }
    }
    private void SkillQ()
    {
        Debug.Log("SkillQ ���");
        P_BowQ.SkillAction();
    }

    private void SkillE()
    {
        Debug.Log("SkillE ���");
        P_BowE.SkillAction();
    }
}