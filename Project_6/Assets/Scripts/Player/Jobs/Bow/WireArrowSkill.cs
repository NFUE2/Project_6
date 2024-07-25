using UnityEngine;
using Photon.Pun;
using TMPro;

public class WireArrowSkill : SkillBase
{
    public GameObject wireArrow;
    public PlayerDataSO PlayerData;

    void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;
    }

    public override void UseSkill()
    {
        if (Time.time - lastActionTime < cooldownDuration) return;

        lastActionTime = Time.time;
        GameObject go = PhotonNetwork.Instantiate("Prototype/" + wireArrow.name, transform.position, Quaternion.identity);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        go.transform.localEulerAngles = new Vector3(0, 0, angle);
        go.GetComponent<P_WireArrow>().player = transform;
    }
}
