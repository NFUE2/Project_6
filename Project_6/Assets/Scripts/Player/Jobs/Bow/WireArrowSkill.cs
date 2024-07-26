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

        // PhotonNetwork.Instantiate 대신 Instantiate 사용
        GameObject go = Instantiate(wireArrow, transform.position, Quaternion.identity);

        // 마우스 위치 계산
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // 2D 게임이므로 Z축을 0으로 설정
        Vector2 direction = (mousePos - transform.position).normalized;

        // 화살의 각도 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        go.transform.localEulerAngles = new Vector3(0, 0, angle);

        // P_WireArrow 컴포넌트 설정
        Bow_WireArrow wireArrowComponent = go.GetComponent<Bow_WireArrow>();
        if (wireArrowComponent != null)
        {
            wireArrowComponent.player = transform;
        }
        else
        {
            Debug.LogError("Bow_WireArrow 컴포넌트를 찾을 수 없습니다.");
        }
    }
}
