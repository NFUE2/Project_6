using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections;

public class ShieldSkill : SkillBase
{
    public float ShieldDuration;
    public GameObject shield;
    private GameObject createShield;
    public PlayerDataSO PlayerData;

    void Start()
    {
        cooldownDuration = PlayerData.SkillQCooldown;
    }

    public override void UseSkill()
    {
        if (createShield != null || Time.time - lastActionTime < cooldownDuration) return;

        createShield = PhotonNetwork.Instantiate("Prototype/" + shield.name, transform.position, Quaternion.identity);
        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        createShield.transform.localEulerAngles = new Vector3(0, 0, angle);

        StartCoroutine(FollowPlayer());

        lastActionTime = Time.time;
        StartCoroutine(DestroyShieldAfterTime());
    }

    private IEnumerator FollowPlayer()
    {
        while (createShield != null)
        {
            createShield.transform.position = transform.position;
            yield return null;
        }
    }

    private IEnumerator DestroyShieldAfterTime()
    {
        yield return new WaitForSeconds(ShieldDuration);
        if (createShield != null)
        {
            PhotonNetwork.Destroy(createShield);
        }
    }
}
