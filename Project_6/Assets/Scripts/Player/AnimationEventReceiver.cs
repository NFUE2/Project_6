using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    private MeleePlayerBase meleePlayerBase;

    void Start()
    {
        // ���� ������Ʈ�� MeleePlayerBase ��ũ��Ʈ�� �����մϴ�.
        meleePlayerBase = GetComponentInParent<MeleePlayerBase>();
    }

    // �ִϸ��̼� �̺�Ʈ���� ȣ���� �޼���
    public void PerformAttack()
    {
        if (meleePlayerBase != null)
        {
            meleePlayerBase.PerformAttack();
        }
    }
}
