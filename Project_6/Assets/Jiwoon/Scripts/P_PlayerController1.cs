using UnityEngine;

public class PlayerController_Melee : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    public float meleeAttackRange = 1f; // 근접 공격 범위
    public LayerMask enemyLayer; // 적 레이어
    public Animator animator; // 공격 애니메이터
    public BoxCollider2D meleeCollider;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isJumping;
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private PlayerControls playerControls;
    private bool isAttacking = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerControls = new PlayerControls(); // 추가: PlayerControls 인스턴스 생성
        meleeCollider.enabled = false; // 콜라이더 비활성화
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();

        playerControls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerControls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        playerControls.Player.Jump.performed += ctx => Jump();
        playerControls.Player.Attack.performed += ctx => Attack();
        playerControls.Player.Attack.canceled += ctx => StopAttack();
        playerControls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
        playerControls.Player.Disable();
    }

    private void FixedUpdate()
    {
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
        if (Mathf.Abs(rb.velocity.y) < 0.001f) // 플레이어가 땅에 있는지 확인
        {
            isJumping = true;
        }
    }

    private void Attack()
    {
        // 공격 애니메이션 재생
        animator.SetTrigger("Attack");
        isAttacking = true;
    }

    private void StopAttack()
    {
        isAttacking = false;
    }

    // 애니메이션 이벤트로 호출될 메서드
    public void EnableMeleeCollider()
    {
        meleeCollider.enabled = true;
    }

    // 애니메이션 이벤트로 호출될 메서드
    public void DisableMeleeCollider()
    {
        meleeCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttacking && meleeCollider.enabled && (enemyLayer == (enemyLayer | (1 << collision.gameObject.layer))))
        {
            Debug.Log("적 공격됨: " + collision.name);
            // 여기에 적에게 데미지를 주는 로직 추가
        }
    }

    private void Look(Vector2 lookInput)
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(lookInput);
        Vector2 direction = mousePosition - (Vector2)transform.position;

        if (direction.x > 0)
        {
            spriteRenderer.flipX = false; // 오른쪽
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true; // 왼쪽
        }
    }
}
