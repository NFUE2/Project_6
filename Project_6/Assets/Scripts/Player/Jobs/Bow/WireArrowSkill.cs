using UnityEngine;
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

        // PhotonNetwork.Instantiate ��� Instantiate ���
        GameObject go = Instantiate(wireArrow, transform.position, Quaternion.identity);

        // ���콺 ��ġ ���
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // 2D �����̹Ƿ� Z���� 0���� ����
        Vector2 direction = (mousePos - transform.position).normalized;

        // ȭ���� ���� ���
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        go.transform.localEulerAngles = new Vector3(0, 0, angle);

        // P_WireArrow ������Ʈ ����
        Bow_WireArrow wireArrowComponent = go.GetComponent<Bow_WireArrow>();
        if (wireArrowComponent != null)
        {
            wireArrowComponent.player = transform;
        }
        else
        {
            Debug.LogError("Bow_WireArrow ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }
}
