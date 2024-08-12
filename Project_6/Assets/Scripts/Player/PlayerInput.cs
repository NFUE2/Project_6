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
    protected Vector2 lookInput; // 마우스 위치 저장 변수

    [Header("Input On/Off Control")]
    public bool isDead = false;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Movement();         // 죽은 상태에서도 이동 가능
        CheckGrounded();    // 땅에 닿았는지 체크
        UpdateAnimation();  // 애니메이션 업데이트
        RotateTowardsMouse(); // 죽은 상태에서도 마우스 회전 가능
    }

    private void Movement()
    {
        float speed = isRunning ? playerdata.runSpeed : playerdata.walkSpeed;
        rb.velocity = new Vector2(moveInput.x * speed, rb.velocity.y);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * playerdata.jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("Jump"); // 점프 애니메이션 트리거
        }
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); // 캐릭터의 발 아래에 Raycast를 쏘아 땅에 닿아 있는지 확인
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
        mousePosition.z = 0;

        Vector3 direction = (mousePosition - transform.position).normalized;

        transform.localScale = direction.x >= 0.01f ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // 이동 입력은 죽은 상태에서도 가능
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
        // 달리기 입력도 죽은 상태에서 가능
        isRunning = context.ReadValueAsButton();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // 마우스 방향 전환도 죽은 상태에서 가능
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        // 공격은 죽은 상태에서 불가능
        if (!isDead && context.performed)
        {
            player.Attack();
        }
    }

    public void OnSkillQ(InputAction.CallbackContext context)
    {
        // 스킬 Q는 죽은 상태에서 불가능
        if (!isDead && context.performed)
        {
            player.UseSkillQ();
        }
    }

    public void OnSkillE(InputAction.CallbackContext context)
    {
        // 스킬 E는 죽은 상태에서 불가능
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
