using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class RangedPlayerBase : PlayerBase
{
    public GameObject attackPrefab;
    public Transform attackPoint;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerInput playerInput; // 인스펙터에서 할당할 수 있도록 설정
    [SerializeField] private AudioClip attackSound; // 공격 효과음 클립
    protected AudioSource audioSource; // 오디오 소스 컴포넌트

    private void Awake()
    {
        // 인스펙터에서 playerInput이 설정되지 않았다면 GetComponent로 할당 시도
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }

        // AudioSource 컴포넌트 가져오기
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
            Debug.LogError("PlayerInput 컴포넌트가 GameObject에 없습니다.");
        }
        else
        {
            Debug.Log("PlayerInput 컴포넌트가 성공적으로 초기화되었습니다.");
        }

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera가 할당되지 않았거나 찾을 수 없습니다.");
        }
        else
        {
            Debug.Log("Main Camera가 성공적으로 초기화되었습니다.");
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource 컴포넌트가 GameObject에 없습니다.");
        }
        else
        {
            Debug.Log("AudioSource 컴포넌트가 성공적으로 초기화되었습니다.");
        }

        if (attackSound == null)
        {
            Debug.LogError("attackSound가 할당되지 않았습니다.");
        }
    }

    public override void Attack()
    {
        if (Time.time - lastAttackTime < playerData.attackCooldown) return;
        lastAttackTime = Time.time;

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput이 초기화되지 않았습니다.");
            return;
        }

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized;

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        //GameObject attackInstance = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity);
        PhotonNetwork.Instantiate("Prefabs/" + attackPrefab.name, attackPoint.position, Quaternion.Euler(0,0,angle));
        //attackInstance.GetComponent<Projectile>().SetDirection(attackDirection);
        // 공격 효과음 재생

        PlayAttackSound();
    }

    protected void PlayAttackSound()
    {
        if (attackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackSound);
            Debug.Log("공격 효과음 재생: " + attackSound.name);
        }
        else
        {
            Debug.LogError("attackSound 또는 audioSource가 할당되지 않았습니다.");
        }
    }
}
