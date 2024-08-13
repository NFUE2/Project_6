using UnityEngine;

public class ChargeAttackEventReceiver : MonoBehaviour
{
    private ChargeSkill chargeSkill;

    void Start()
    {
        // ���� ������Ʈ�� ChargeSkill ��ũ��Ʈ�� �����մϴ�.
        chargeSkill = GetComponentInParent<ChargeSkill>();
    }

    // �ִϸ��̼� �̺�Ʈ���� ȣ���� �޼���
    public void TriggerChargingAttack()
    {
        if (chargeSkill != null)
        {
            chargeSkill.ChargingAttack();
        }
    }
}
