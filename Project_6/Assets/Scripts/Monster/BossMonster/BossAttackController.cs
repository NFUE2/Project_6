using UnityEngine;

public class BossAttackController : MonoBehaviour
{
    public int countOfAttack;
    // ���� �߰�
    public virtual void SelectAttack()
    {
        
    }

    public void ExitAttack()
    {
        BossBattleManager.Instance.ToggleIsAttacking();
        BossBattleManager.Instance.bossStateMachine.ChangeState(BossBattleManager.Instance.bossStateMachine.IdleState);
    }
}