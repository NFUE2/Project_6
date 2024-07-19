using UnityEngine;
using UnityEngine.InputSystem;

public class GrenadeSkill : SkillBase
{
    public GameObject rifleGrenade;
    private RiflePlayer player; //public���� transform�� �޾ƿɽô�
    public PlayerDataSO PlayerData;

    void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration) return;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); //PlayerInput������ ������ �޾Ƽ� ó��

        Vector2 attackDirection = (mousePosition - (Vector2)player.attackPoint.position).normalized;

        GameObject grenadeInstance = Instantiate(rifleGrenade, player.attackPoint.position, Quaternion.identity);
        grenadeInstance.GetComponent<Rigidbody2D>().velocity = attackDirection * 10f; //velocity ����

        grenadeInstance.GetComponent<Grenade>().Initialize(10f, 5f, 3f); //������ �κ��� ScriptableObject�� ó���մϴ�
    }
}
