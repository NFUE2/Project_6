using UnityEngine;
using UnityEngine.InputSystem;

public class GrenadeSkill : SkillBase
{
    public GameObject rifleGrenade;
    private RiflePlayer player; //public으로 transform을 받아옵시다
    public PlayerDataSO PlayerData;

    void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration) return;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); //PlayerInput에서의 데이터 받아서 처리

        Vector2 attackDirection = (mousePosition - (Vector2)player.attackPoint.position).normalized;

        GameObject grenadeInstance = Instantiate(rifleGrenade, player.attackPoint.position, Quaternion.identity);
        grenadeInstance.GetComponent<Rigidbody2D>().velocity = attackDirection * 10f; //velocity 수정

        grenadeInstance.GetComponent<Grenade>().Initialize(10f, 5f, 3f); //데이터 부분은 ScriptableObject가 처리합니다
    }
}
