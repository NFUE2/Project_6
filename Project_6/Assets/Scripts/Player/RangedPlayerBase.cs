using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public abstract class RangedPlayerBase : PlayerBase
{
    public GameObject attackPrefab;
    public Transform attackPoint;
    protected Camera mainCamera;
    [SerializeField] private AudioClip attackSound; // ���� ȿ���� Ŭ��
    protected AudioSource audioSource; // ����� �ҽ� ������Ʈ

    private void Awake()
    {
        // ���� ī�޶� �±׷� ã��
        mainCamera = Camera.main;
        // AudioSource ������Ʈ ��������
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        AttackCoolTime(); // ���� ��Ÿ�� UI ������Ʈ
    }

    public override void Attack()
    {
        if (currentAttackTime < playerData.attackTime) return;
        currentAttackTime = 0f; // ���� �� ��Ÿ�� �ʱ�ȭ

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized;

        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        GameObject projectile = PhotonNetwork.Instantiate(attackPrefab.name, attackPoint.position, Quaternion.Euler(0, 0, angle));

        Projectile proj = projectile.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.SetDirection(attackDirection); // ����ü�� ���� ����
        }

        PlayAttackSound(); // ���� ȿ���� ���
    }
    protected void PlayAttackSound()
    {
        if (attackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }
}
