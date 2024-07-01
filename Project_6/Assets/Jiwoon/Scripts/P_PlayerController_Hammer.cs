using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController_Hammer : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    public float attackTime;
    private float lastAttackTime;

    public float meleeAttackRange = 1f; // ���� ���� ����
    public LayerMask enemyLayer; // �� ���̾�
    public Animator animator; // ���� �ִϸ�����
    public BoxCollider2D meleeCollider;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isJumping;
    private Camera mainCamera;
    private PlayerControls playerControls;

    private P_HammerQ P_HammerQ;
    private P_HammerE P_HammerE;

    PhotonView pv;
    public Text cooltimeQText, cooltimeEText;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        playerControls = new PlayerControls(); // �߰�: PlayerControls �ν��Ͻ� ����
        meleeCollider.enabled = false; // �ݶ��̴� ��Ȱ��ȭ
        P_HammerQ = GetComponent<P_HammerQ>();
        P_HammerE = GetComponent<P_HammerE>();

        pv = GetComponent<PhotonView>();
        cooltimeQText = GameObject.Find("Skill_Q").GetComponentInChildren<Text>();
        cooltimeEText = GameObject.Find("Skill_E").GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if (!pv.IsMine) return;

        Look(Mouse.current.position.ReadValue());
    }

    private void OnEnable()
    {
        if (!pv.IsMine) return;

        playerControls.Player.Enable();

        playerControls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerControls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        playerControls.Player.Jump.performed += ctx => Jump();

        playerControls.Player.Attack.performed += ctx => Attack();
        playerControls.Player.Attack.canceled += ctx => StopAttack();

        playerControls.Player.SkillQ.performed += ctx => SkillQ();
        playerControls.Player.SkillE.performed += ctx => SkillE();
        //playerControls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        playerControls.Player.Disable();
    }

    private void FixedUpdate()
    {
        if (!pv.IsMine) return;

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
        if (GetComponent<P_HammerE>().isCharging) return;
        if (Time.time - lastAttackTime < attackTime) return;

        lastAttackTime = Time.time;
        // ���� �ִϸ��̼� ���
        animator.SetTrigger("Attack");
    }

    private void StopAttack()
    {

    }

    // �ִϸ��̼� �̺�Ʈ�� ȣ��� �޼���
    public void EnableMeleeCollider()
    {
        meleeCollider.enabled = true;
    }

    // �ִϸ��̼� �̺�Ʈ�� ȣ��� �޼���
    public void DisableMeleeCollider()
    {
        meleeCollider.enabled = false;
    }

    private void Look(Vector2 lookInput)
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(lookInput);
        Vector2 direction = mousePosition - (Vector2)transform.position;

        // �θ� ������Ʈ �������� ȸ�� ó��
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // ������
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // ����
        }
    }

    private void SkillQ()
    {
        Debug.Log("SkillQ ���");
        P_HammerQ.SkillAction();
    }

    private void SkillE()
    {
        Debug.Log("SkillE ���");
        P_HammerE.SkillAction();
    }
}
