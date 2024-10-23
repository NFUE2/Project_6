using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    private MeleePlayerBase meleePlayerBase;

    void Start()
    {
        // 상위 오브젝트의 MeleePlayerBase 스크립트를 참조합니다.
        meleePlayerBase = GetComponentInParent<MeleePlayerBase>();
    }

    // 애니메이션 이벤트에서 호출할 메서드
    public void PerformAttack()
    {
        if (meleePlayerBase != null)
        {
            meleePlayerBase.PerformAttack();
        }
    }
}
