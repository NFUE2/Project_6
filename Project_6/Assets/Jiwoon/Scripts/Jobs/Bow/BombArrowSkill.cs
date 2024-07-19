using UnityEngine;
using Photon.Pun;
using TMPro;

public class BombArrowSkill : SkillBase
{
    public GameObject bombArrow;
    public float fireAngle;
    public PlayerDataSO PlayerData;

    void Start()
    {
        cooldownDuration = PlayerData.SkillECooldown;
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration) return;

        lastActionTime = Time.time;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - fireAngle;

        for (int i = 0; i < 3; i++)
        {
            GameObject go = PhotonNetwork.Instantiate("Prototype/" + bombArrow.name, transform.position, Quaternion.identity);
            go.transform.localEulerAngles = new Vector3(0, 0, angle + i * fireAngle);
        }
    }
}
