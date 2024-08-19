
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviourPun
{
    [Header("move_Data")]
    public Vector2 moveInput;
    protected bool isRunning;
    private bool isJumping = false;
    private bool jumpRequest = false;

    [Header("animation_Data")]
    protected Animator animator;

    [Header("Player_Data & Source")]
    protected Rigidbody2D rb;
    public PlayerDataSO playerdata;
    public PlayerBase player;

    [Header("ground_Data")]
    protected bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    [Header("Mouse_Data")]
    public Vector2 lookInput; // ���콺 ��ġ ���� ����

    [Header("Input On/Off Control")]
    public bool isDead = false;

    protected virtual void Start()
    {
        isJumping = false;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        Movement();         // ���� ���¿����� �̵� ����
        CheckGrounded();    // ���� ��Ҵ��� üũ
        UpdateAnimation();  // �ִϸ��̼� ������Ʈ
        RotateTowardsMouse(); // ���� ���¿����� ���콺 ȸ�� ����

        // ���� ��� �ְ� ���� ��û�� ���� �� ���� ����
        if (isGrounded && jumpRequest)
        {
            Jump();
        }
    }


    private void Movement()
    {
        float speed = isRunning ? playerdata.moveSpeed * 1.5f : playerdata.moveSpeed;
        rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);
    }

    private void Jump()
    {
        if (isGrounded && !isJumping) // ĳ���Ͱ� ���� �ְ� ���� ���� �ƴ� ���� ����
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); // ���� �ӵ��� �ʱ�ȭ�Ͽ� ���� ���� �� ����
            rb.AddForce(Vector2.up * playerdata.jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
            isJumping = true;
        }
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded)
        {
            isJumping = false; // ���� ������ ���� ���� ����
        }
    }

    private void UpdateAnimation()
    {
        if (animator != null)
        {
            bool isWalking = moveInput.x != 0;

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

                if (isWalking)
                {
                    if (isRunning)
                    {
                        animator.SetBool("IsRun", true);
                        animator.SetBool("IsWalk", false);
                    }
                    else
                    {
                        animator.SetBool("IsWalk", true);
                        animator.SetBool("IsRun", false);
                    }
                }
                else
                {
                    animator.SetBool("IsWalk", false);
                    animator.SetBool("IsRun", false);
                }
                animator.SetBool("IsIdle", !isWalking);
            }
        }
    }

    private void RotateTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(lookInput);
        mousePosition.z = transform.position.z; // Z �� �� ����

        Vector3 direction = (mousePosition - transform.position).normalized;

        // �÷��̾��� ȸ�� ���� ����
        if (direction.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1); // ������ �ٶ�
        }
        else if (direction.x <= -0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1); // ���� �ٶ�
        }

        // ��Ÿ�� �� ���� Ư�� UI ��Ҵ� ȸ������ �ʵ��� ����
        Transform cooldownUI = transform.Find("PlayerAttackUI");
        if (cooldownUI != null)
        {
            cooldownUI.localScale = new Vector3(1 / transform.localScale.x, 1, 1); // UI�� �������� �ݴ�� ����
        }
    }









    public void OnMove(InputAction.CallbackContext context)
    {
        // �̵� �Է��� ���� ���¿����� ����
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpRequest = true; // ���� ��û�� ���
        }
        else if (context.canceled)
        {
            jumpRequest = false; // ���� Ű�� �����Ǹ� ��û ����
        }
    }
    public void OnRun(InputAction.CallbackContext context)
    {
        // �޸��� �Էµ� ���� ���¿��� ����
        isRunning = context.ReadValueAsButton();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // ���콺 ���� ��ȯ�� ���� ���¿��� ����
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        // ������ ���� ���¿��� �Ұ���
        if (!isDead && context.performed)
        {
            player.Attack();
        }
    }

    public void OnSkillQ(InputAction.CallbackContext context)
    {
        // ��ų Q�� ���� ���¿��� �Ұ���
        if (!isDead && context.performed)
        {
            player.UseSkillQ();
        }
    }

    public void OnSkillE(InputAction.CallbackContext context)
    {
        // ��ų E�� ���� ���¿��� �Ұ���
        if (!isDead && context.performed)
        {
            player.UseSkillE();
        }
    }

    public Vector2 GetMousePosition()
    {
        return lookInput;
    }
}