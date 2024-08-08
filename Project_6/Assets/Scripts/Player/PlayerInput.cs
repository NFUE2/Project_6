using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("move_Data")]
    protected Vector2 moveInput;
    protected bool isRunning;

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
    protected Vector2 lookInput; // ���콺 ��ġ ���� ����

    [Header("Input On/Off Controll")]
    public bool isDead = false;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D ������Ʈ�� �����ϴ�.");
        }
        if (animator == null)
        {
            Debug.LogError("Animator ������Ʈ�� �����ϴ�.");
        }
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
        if (isDead)
        {
            return;
        }
        float speed = isRunning ? playerdata.runSpeed : playerdata.walkSpeed;
        rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);
    }

    private void Jump()
    {
        if (isDead)
        {
            return;
        }
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * playerdata.jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("Jump"); // ���� �ִϸ��̼� Ʈ����
        }
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); // ĳ������ �� �Ʒ��� Raycast�� ��� ���� ��� �ִ��� Ȯ��
    }

    private void UpdateAnimation()
    {
        if (animator != null)
        {
            bool isWalking = moveInput.x != 0;

            // ���߿� ���� �� ���� �ִϸ��̼�
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

                // �̵��� ���� �� ���̵� ���·� ��ȯ
                animator.SetBool("IsIdle", !isWalking);
            }
        }
    }


    private void RotateTowardsMouse()
    {
        if (isDead)
        {
            return;
        }
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(lookInput);
        mousePosition.z = 0;

        Vector3 direction = (mousePosition - transform.position).normalized;

        transform.localScale = direction.x >= 0.01f ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (isDead)
        {
            return;
        }
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isDead)
        {
            return;
        }
        if (context.performed)
        {
            Jump();
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (isDead)
        {
            return;
        }
        isRunning = context.ReadValueAsButton();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (isDead)
        {
            return;
        }
        lookInput = context.ReadValue<Vector2>(); // ���콺 ��ġ �Է� �ޱ�
    }

    public Vector2 GetMousePosition()
    {
        return lookInput;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (isDead)
        {
            return;
        }
        if (context.performed)
        {
            player.Attack();
        }
    }

    public void OnSkillQ(InputAction.CallbackContext context)
    {
        if (isDead)
        {
            return;
        }
        if (context.performed)
        {
            player.UseSkillQ();
        }
    }

    public void OnSkillE(InputAction.CallbackContext context)
    {
        if (isDead)
        {
            return;
        }
        if (context.performed)
        {
            player.UseSkillE();
        }
    }
}
