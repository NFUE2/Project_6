using UnityEngine;

public class ChargeAttackEventReceiver : MonoBehaviour
{
    private ChargeSkill chargeSkill;

    void Start()
    {
        // 상위 오브젝트의 ChargeSkill 스크립트를 참조합니다.
        chargeSkill = GetComponentInParent<ChargeSkill>();
    }

    // 애니메이션 이벤트에서 호출할 메서드
    public void TriggerChargingAttack()
    {
        if (chargeSkill != null)
        {
            chargeSkill.ChargingAttack();
        }
    }
}
