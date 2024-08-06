using Photon.Pun;
using UnityEngine;

public abstract class RangedPlayerBase : PlayerBase
{
    public GameObject attackPrefab;
    public Transform attackPoint;
    private Camera mainCamera;
    [SerializeField] private AudioClip attackSound; // 공격 효과음 클립
    protected AudioSource audioSource; // 오디오 소스 컴포넌트

    private void Awake()
    {
        // 메인 카메라를 태그로 찾기
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning("Main Camera를 찾을 수 없습니다.");
        }
        else
        {
            Debug.Log("Main Camera가 성공적으로 초기화되었습니다.");
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

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera가 초기화되지 않았습니다.");
            return;
        }

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized;

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        GameObject projectile = PhotonNetwork.Instantiate("Prefabs/" + attackPrefab.name, attackPoint.position, Quaternion.Euler(0, 0, angle));

        Projectile proj = projectile.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.SetDirection(attackDirection); // 투사체의 방향 설정
        }

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
