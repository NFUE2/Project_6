using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class RangedPlayerBase : PlayerBase
{
    public GameObject attackPrefab;
    public Transform attackPoint;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerInput playerInput; // �ν����Ϳ��� �Ҵ��� �� �ֵ��� ����
    [SerializeField] private AudioClip attackSound; // ���� ȿ���� Ŭ��
    protected AudioSource audioSource; // ����� �ҽ� ������Ʈ

    private void Awake()
    {
        // �ν����Ϳ��� playerInput�� �������� �ʾҴٸ� GetComponent�� �Ҵ� �õ�
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }

        // AudioSource ������Ʈ ��������
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput ������Ʈ�� GameObject�� �����ϴ�.");
        }
        else
        {
            Debug.Log("PlayerInput ������Ʈ�� ���������� �ʱ�ȭ�Ǿ����ϴ�.");
        }

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera�� �Ҵ���� �ʾҰų� ã�� �� �����ϴ�.");
        }
        else
        {
            Debug.Log("Main Camera�� ���������� �ʱ�ȭ�Ǿ����ϴ�.");
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource ������Ʈ�� GameObject�� �����ϴ�.");
        }
        else
        {
            Debug.Log("AudioSource ������Ʈ�� ���������� �ʱ�ȭ�Ǿ����ϴ�.");
        }

        if (attackSound == null)
        {
            Debug.LogError("attackSound�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    public override void Attack()
    {
        if (Time.time - lastAttackTime < playerData.attackCooldown) return;
        lastAttackTime = Time.time;

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized;

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        //GameObject attackInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity);
        PhotonNetwork.Instantiate("Prefabs/" + attackPrefab.name, attackPoint.position, Quaternion.Euler(0,0,angle));
        //attackInstance.GetComponent<Projectile>().SetDirection(attackDirection);
        // ���� ȿ���� ���

        PlayAttackSound();
    }

    protected void PlayAttackSound()
    {
        if (attackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackSound);
            Debug.Log("���� ȿ���� ���: " + attackSound.name);
        }
        else
        {
            Debug.LogError("attackSound �Ǵ� audioSource�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}
