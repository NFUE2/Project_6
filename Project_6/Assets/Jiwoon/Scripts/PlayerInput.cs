using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour  //각 플레이어의 겹치는 역할들을 통합하고 상속을 내리는 방식으로 구현되어 있다.
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
    protected Vector2 lookInput; // 마우스 위치 저장 변수

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

        // 즉시 애니메이션 업데이트
        if (animator != null)
        {
            bool isWalking = moveInput.x != 0;

            // 공중에 있을 때는 점프 애니메이션 유지
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

                // 이동 입력이 없으면 Idle 상태로 전환
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

    //수정사항 - OnCollisionEnter2D에서 처리
    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); // 캐릭터의 발 아래에 Raycast를 쏘아 땅에 닿아 있는지 확인
    }
    private void UpdateAnimation()
    {
        if (animator != null)        // 걷기 및 달리기 애니메이션 업데이트
        {
            bool isWalking = moveInput.x != 0;

            if (!isGrounded)            // 공중에 있을 때는 점프 애니메이션 유지
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
                animator.SetBool("IsIdle", !isWalking); // 이동 입력이 없으면 Idle 상태로 전환
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
        lookInput = context.ReadValue<Vector2>(); // 마우스 위치 입력 받기
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