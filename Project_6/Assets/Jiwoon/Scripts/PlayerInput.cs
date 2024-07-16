using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour  //�� �÷��̾��� ��ġ�� ���ҵ��� �����ϰ� ����� ������ ������� �����Ǿ� �ִ�.
{

    [Header("move_Data")]
    protected Vector2 moveInput;
    protected bool isRunning;

    [Header("animtion_Data")]
    protected Animator animator;

    [Header("Player_Data & Source")]
    protected Rigidbody2D rb;
    public PlayerData playerdata;
    public PlayerBase player;

    [Header("ground_Data")]
    protected bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    [Header("Mouse_Data")]
    protected Vector2 lookInput; // ���콺 ��ġ ���� ����

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Movement();
        CheckGrounded();
        UpdateAnimation();
        RotateTowardsMouse();
    }

    private void Movement()
    {
        float speed = isRunning ? playerdata.runSpeed : playerdata.walkSpeed;
        rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);

        // ��� �ִϸ��̼� ������Ʈ
        if (animator != null)
        {
            bool isWalking = moveInput.x != 0;

            // ���߿� ���� ���� ���� �ִϸ��̼� ����
            if (!isGrounded)
            {
                animator.SetBool("IsJump", true);
                animator.SetBool("IsWalk", false);
                animator.SetBool("IsRun", false);
                animator.SetBool("IsIdle", false);
            }
            else
            {
                animator.SetBool("IsJump", false);
                animator.SetBool("IsWalk", isWalking && !isRunning);
                animator.SetBool("IsRun", isWalking && isRunning);

                // �̵� �Է��� ������ Idle ���·� ��ȯ
                animator.SetBool("IsIdle", !isWalking);
            }
        }
    }
    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * playerdata.jumpForce, ForceMode2D.Impulse);
            animator.SetBool("IsJump", true);
        }
    }

    //�������� - OnCollisionEnter2D���� ó��
    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); // ĳ������ �� �Ʒ��� Raycast�� ��� ���� ��� �ִ��� Ȯ��
    }
    private void UpdateAnimation()
    {
        if (animator != null)        // �ȱ� �� �޸��� �ִϸ��̼� ������Ʈ
        {
            bool isWalking = moveInput.x != 0;

            if (!isGrounded)            // ���߿� ���� ���� ���� �ִϸ��̼� ����
            {
                animator.SetBool("IsJump", true);
                animator.SetBool("IsWalk", false);
                animator.SetBool("IsRun", false);
                animator.SetBool("IsIdle", false);
            }
            else
            {
                animator.SetBool("IsJump", false);
                animator.SetBool("IsWalk", isWalking && !isRunning);
                animator.SetBool("IsRun", isWalking && isRunning);
                animator.SetBool("IsIdle", !isWalking); // �̵� �Է��� ������ Idle ���·� ��ȯ
            }
        }
    }
    private void RotateTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(lookInput);
        mousePosition.z = 0;

        Vector3 direction = (mousePosition - transform.position).normalized;

        transform.localScale = direction.x >= 0.01f ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Jump();
        }
    }
    public void OnRun(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>(); // ���콺 ��ġ �Է� �ޱ�
    }
    public Vector2 GetMousePosition()
    {
        return lookInput;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.Attack();
        }
    }
    public void OnSkillQ(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.UseSkillQ();
        }
    }
    public void OnSkillE(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player. UseSkillE();
        }
    }
}