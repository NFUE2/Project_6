using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAttackController : BossAttackController
{
    public GameObject stompHitBox;
    public GameObject swingHitBoxLeft;
    public GameObject swingHitBoxRight;

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
                SwingReady();
                break;
        }
        //BossBattleManager.Instance.bossStateMachine.ChangeState(BossBattleManager.Instance.bossStateMachine.IdleState);
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

    // ½ºÀ®
    // ½ºÀ® + ÁöÁø
    public void SwingReady()
    {
        BossBattleManager.Instance.bossAnimator.SetBool("isSwingReady", true);
    }

    private void Swing()
    {
        BossBattleManager.Instance.bossAnimator.SetBool("isSwingReady", false);
        BossBattleManager.Instance.bossAnimator.SetBool("isSwing", true);
    }
    
    private void EnableSwingHitBox()
    {
        if(transform.position.x > BossBattleManager.Instance.targetPlayer.transform.position.x)
        {
            swingHitBoxLeft.SetActive(true);
        }
        else
        {
            swingHitBoxRight.SetActive(true);
        }
        BossBattleManager.Instance.bossAnimator.SetBool("isSwing", false);
        ExitAttack() ;
    }

    private void ExitAttack()
    {
        BossBattleManager.Instance.isAttacking = false;
        BossBattleManager.Instance.bossStateMachine.ChangeState(BossBattleManager.Instance.bossStateMachine.IdleState);
    }
}
