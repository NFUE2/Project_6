using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAttackController : BossAttackController
{
    public GameObject stompHitBox;
    public override void SelectAttack()
    {
        base.SelectAttack();
        countOfAttack = 1;
        int index = Random.Range(0, countOfAttack);
        switch (index)
        {
            case 0:
                StompReady();
                break;
            case 1:
                Punch();
                break;
        }
        //BossBattleManager.Instance.bossStateMachine.ChangeState(BossBattleManager.Instance.bossStateMachine.IdleState);
    }
    public void EyeRazor()
    {
        Debug.Log("·¹ÀÌÀú ºö!");
        ExitAttack();
    }

    public void Punch()
    {
        Debug.Log("ÈÖÀû... ÈÖÀû...");
        ExitAttack();
    }

    // ÁöÁø
    public void StompReady()
    {
        BossBattleManager.Instance.bossAnimator.SetBool("isStompReady", true);
    }

    private void Stomp()
    {
        BossBattleManager.Instance.bossAnimator.SetBool("isStompReady", false);
        BossBattleManager.Instance.bossAnimator.SetBool("isStomp", true);
    }

    private void EnableStompHitBox()
    {
        stompHitBox.SetActive(true);
        BossBattleManager.Instance.bossAnimator.SetBool("isStomp", false);
        ExitAttack() ;
    }

    private void ExitAttack()
    {
        Debug.Log("exit attack");
        BossBattleManager.Instance.isAttacking = false;
        BossBattleManager.Instance.bossStateMachine.ChangeState(BossBattleManager.Instance.bossStateMachine.IdleState);
    }
}
