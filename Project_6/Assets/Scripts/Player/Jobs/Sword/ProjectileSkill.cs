using UnityEngine;
using TMPro;
using Photon.Pun;

public class ProjectileSkill : SkillBase
{
    public GameObject projectilePrefab;
    public Transform attackPoint;
    public PlayerDataSO PlayerData;

    void Start()
    {
        cooldownDuration = PlayerData.SkillECooldown;
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration) return;

        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //GameObject projectileInstance = Instantiate(projectilePrefab, attackPoint.position, Quaternion.identity);
        GameObject projectile = PhotonNetwork.Instantiate("Projectile/" + projectilePrefab.name,transform.position,Quaternion.Euler(0,0,angle));
        //projectile.transform.localEulerAngles = new Vector3(0, 0, angle);
        //projectileInstance.GetComponent<Projectile>().SetDirection(dir);

        lastActionTime = Time.time;
    }
}
