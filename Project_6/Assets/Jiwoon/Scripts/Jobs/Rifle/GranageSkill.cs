using UnityEngine;
using UnityEngine.InputSystem;

public class GrenadeSkill : MonoBehaviour
{
    public GameObject rifleGrenade;
    private RiflePlayer player; //public으로 transform을 받아옵시다

    public void SetPlayer(RiflePlayer player)
    {
        this.player = player;
    }

    public void UseSkill()
    {
        if (Time.time - player.GetLastEActionTime() < player.PlayerData.SkillECooldown) return;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); //PlayerInput에서의 데이터 받아서 처리

        Vector2 attackDirection = (mousePosition - (Vector2)player.attackPoint.position).normalized;

        GameObject grenadeInstance = Instantiate(rifleGrenade, player.attackPoint.position, Quaternion.identity);
        grenadeInstance.GetComponent<Rigidbody2D>().velocity = attackDirection * 10f; //velocity 수정

        grenadeInstance.GetComponent<Grenade>().Initialize(10f, 5f, 3f); //데이터 부분은 ScriptableObject가 처리합니다

        player.SetLastEActionTime(Time.time); //쿨타임 처리는 상위클래스에서 처리
    }
}
