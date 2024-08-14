using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuinskaldAttackController : BossAttackController
{
    private Rigidbody2D bossRB;
    private float rollForce = 10f;

    [Header("Slash Attack")]
    public GameObject slashObjectR;
    public GameObject slashObjectL;

    private void Start()
    {
        bossRB = BossTestManager.Instance.spawnedBoss.GetComponent<Rigidbody2D>();
    }
    public override void SelectAttack()
    {
        base.SelectAttack();

        int index = Random.Range(0, 2);

        switch (index)
        {
            case 0:
                //Roll();
                Slash();
                break;
            case 1:
                Slash();
                break;
            case 2:
                Debug.Log("3번 패턴");
                BossTestManager.Instance.ToggleIsAttacking();
                BossTestManager.Instance.bossStateMachine.ChangeState(BossTestManager.Instance.bossStateMachine.IdleState);
                break;
        }
    }

    // 패턴 전 구르기
    private void Roll()
    {
        float rollDirection = BossTestManager.Instance.spawnedBoss.transform.position.x < BossTestManager.Instance.targetPlayer.transform.position.x ? 1 : -1;
        bossRB.velocity = new Vector2(rollForce * rollDirection, bossRB.velocity.y);
        BossTestManager.Instance.bossAnimator.SetBool("isRoll", true);
        
    }

    private void AfterRoll()
    {
        BossTestManager.Instance.bossAnimator.SetBool("isRoll", false);
        Slash();
    }
    // 견제형 패턴
    private void Slash()
    {
        BossTestManager.Instance.bossAnimator.SetBool("isSlash", true);
    }

    private void SlashObjectEnabled()
    {
        if(BossTestManager.Instance.targetPlayer.transform.position.x >= BossTestManager.Instance.spawnedBoss.transform.position.x)
        {
            slashObjectR.SetActive(true);
        }
        else
        {
            slashObjectL.SetActive(true);
        }
        
    }

    private void SlashAnimControll()
    {
        if(BossTestManager.Instance.bossAnimator.GetBool("isSlash") == true)
        {
            int attack = Random.Range(0, 3);
            if(attack == 0)
            {
                BossTestManager.Instance.bossAnimator.SetBool("isSlash", false);
                ExitTestAttack();
            }
            else if(attack == 1)
            {
                BossTestManager.Instance.bossAnimator.SetBool("isSlash", false);
                BossTestManager.Instance.bossAnimator.SetBool("isSlash2", true);
            }
            else if(attack == 2)
            {
                BossTestManager.Instance.bossAnimator.SetBool("isSlash", false);
                BossTestManager.Instance.bossAnimator.SetBool("isSlash3", true);
            }
        }
        else if(BossTestManager.Instance.bossAnimator.GetBool("isSlash2") == true)
        {
            int attack = Random.Range(0, 2);
            if(attack == 0)
            {
                BossTestManager.Instance.bossAnimator.SetBool("isSlash2", false);
                ExitTestAttack();
            }
            else if( attack == 1)
            {
                BossTestManager.Instance.bossAnimator.SetBool("isSlash2", false);
                BossTestManager.Instance.bossAnimator.SetBool("isSlash3", true);
            }
        }
        else if (BossTestManager.Instance.bossAnimator.GetBool("isSlash3") == true)
        {
            BossTestManager.Instance.bossAnimator.SetBool("isSlash3", false);
            ExitTestAttack();
        }
    }

    private void ExitTestAttack()
    {
        BossTestManager.Instance.ToggleIsAttacking();
        BossTestManager.Instance.bossStateMachine.ChangeState(BossTestManager.Instance.bossStateMachine.IdleState);
    }
    // 
}
