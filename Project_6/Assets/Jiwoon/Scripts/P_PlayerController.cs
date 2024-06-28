using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_Long : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform attackPoint;
    public GameObject attackPrefab;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isJumping;
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private PlayerControls playerControls;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerControls = new PlayerControls(); // �߰�: PlayerControls �ν��Ͻ� ����
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();

        playerControls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerControls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        playerControls.Player.Jump.performed += ctx => Jump();
        playerControls.Player.Attack.performed += ctx => Attack();
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
        if (Mathf.Abs(rb.velocity.y) < 0.001f) // �÷��̾ ���� �ִ��� Ȯ��
        {
            isJumping = true;
        }
    }

    private void Attack() // ���Ÿ� ĳ���Ͱ� �����Ѵ�.
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // ���콺�� ��ġ��
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized; // ���콺�� ��ġ������ �������� ���� ���� ��ġ���� ���� => ���� ����
        GameObject attackInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity); // �Ѿ��� ������ �߻��� �����Ѵ�.
        attackInstance.GetComponent<Rigidbody2D>().velocity = attackDirection * 15f; // ���� �ӵ� ���� �Ѵ�.
    }


    private void Look(Vector2 lookInput)
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(lookInput);
        Vector2 direction = mousePosition - (Vector2)transform.position;

        if (direction.x > 0)
        {
            spriteRenderer.flipX = false; // ������
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true; // ����
        }
    }
}