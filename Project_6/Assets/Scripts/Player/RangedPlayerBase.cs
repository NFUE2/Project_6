using Photon.Pun;
using UnityEngine;

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
        if (mainCamera == null)
        {
            
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

    }

    public override void Attack()
    {
        if (Time.time - lastAttackTime < playerData.attackCooldown) return;
        lastAttackTime = Time.time;

        if (mainCamera == null)
        {
            return;
        }

        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackDirection = (mousePosition - (Vector2)attackPoint.position).normalized;

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        GameObject projectile = PhotonNetwork.Instantiate(attackPrefab.name, attackPoint.position, Quaternion.Euler(0, 0, angle));

        Projectile proj = projectile.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.SetDirection(attackDirection); // ����ü�� ���� ����
        }

        // ���� ȿ���� ���
        PlayAttackSound();
    }


    protected void PlayAttackSound()
    {
        if (attackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
        else
        {

        }
    }
}
