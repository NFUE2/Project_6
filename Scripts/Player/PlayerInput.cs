
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
    public Vector2 lookInput; // 마우스 위치 저장 변수

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

        Movement();         // 죽은 상태에서도 이동 가능
        CheckGrounded();    // 땅에 닿았는지 체크
        UpdateAnimation();  // 애니메이션 업데이트
        RotateTowardsMouse(); // 죽은 상태에서도 마우스 회전 가능

        // 땅에 닿아 있고 점프 요청이 있을 때 점프 실행
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
        if (isGrounded && !isJumping) // 캐릭터가 땅에 있고 점프 중이 아닐 때만 점프
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); // 수직 속도를 초기화하여 남은 점프 힘 제거
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
            isJumping = false; // 땅에 닿으면 점프 상태 해제
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
        mousePosition.z = transform.position.z; // Z 축 값 고정

        Vector3 direction = (mousePosition - transform.position).normalized;

        // 플레이어의 회전 방향 설정
        if (direction.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1); // 오른쪽 바라봄
        }
        else if (direction.x <= -0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1); // 왼쪽 바라봄
        }

        // 쿨타임 바 등의 특정 UI 요소는 회전하지 않도록 설정
        Transform cooldownUI = transform.Find("PlayerAttackUI");
        if (cooldownUI != null)
        {
            cooldownUI.localScale = new Vector3(1 / transform.localScale.x, 1, 1); // UI의 스케일을 반대로 설정
        }
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
            jumpRequest = true; // 점프 요청을 기록
        }
        else if (context.canceled)
        {
            jumpRequest = false; // 점프 키가 해제되면 요청 해제
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