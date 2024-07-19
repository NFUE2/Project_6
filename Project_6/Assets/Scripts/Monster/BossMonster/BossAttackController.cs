using UnityEngine;

public class BossAttackController : MonoBehaviour
{
    public int countOfAttack;
    // 추후 추가
    public virtual void SelectAttack()
    {
        
    }

    public void ExitAttack()
    {
        BossBattleManager.Instance.ToggleIsAttacking();
        BossBattleManager.Instance.bossStateMachine.ChangeState(BossBattleManager.Instance.bossStateMachine.IdleState);
    }
}