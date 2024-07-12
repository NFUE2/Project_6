using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAttackController : BossAttackController, IPunObservable
{
    public GameObject stompHitBox;
    public GameObject swingHitBoxLeft;
    public GameObject swingHitBoxRight;
    public GameObject razorHitBox;

    private int additionalAttack;

    public override void SelectAttack()
    {
        base.SelectAttack();
        countOfAttack = 3;
        int index = Random.Range(0, countOfAttack);
        switch (index)
        {
            case 0:
                RazorReady();
                break;
            case 1:
                StompReady();
                break;
            case 2:
                SwingReady();
                break;
        }
        //BossBattleManager.Instance.bossStateMachine.ChangeState(BossBattleManager.Instance.bossStateMachine.IdleState);
    }

    // 지진
    public void StompReady()
    {
        BossBattleManager.Instance.ToggleIsAttacking();
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
        additionalAttack = Random.Range(0, 2);
        if( additionalAttack == 0 ) 
        {
            ExitAttack() ;
        }
        else
        {
            SwingReady();
        }
    }

    // 스윙
    // 스윙 + 지진
    public void SwingReady()
    {
        BossBattleManager.Instance.ToggleIsAttacking();
        BossBattleManager.Instance.bossAnimator.SetBool("isSwingReady", true);
    }

    private void Swing()
    {
        BossBattleManager.Instance.bossAnimator.SetBool("isSwingReady", false);
        BossBattleManager.Instance.bossAnimator.SetBool("isSwing", true);
    }
    
    private void EnableSwingHitBox()
    {
        Debug.Log(BossBattleManager.Instance.targetPlayer);
        if(transform.position.x > BossBattleManager.Instance.targetPlayer.transform.position.x)
        {
            swingHitBoxLeft.SetActive(true);
        }
        else
        {
            swingHitBoxRight.SetActive(true);
        }
        BossBattleManager.Instance.bossAnimator.SetBool("isSwing", false);
        additionalAttack = Random.Range(0, 2);
        if(additionalAttack == 0)
        {
            ExitAttack() ;
        }
        else
        {
            StompReady();
        }
    }

    // 레이저
    public void RazorReady()
    {
        BossBattleManager.Instance.ToggleIsAttacking();
        BossBattleManager.Instance.bossAnimator.SetBool("isRazorReady", true);
    }

    private void Razor()
    {
        BossBattleManager.Instance.bossAnimator.SetBool("isRazorReady", false);
        BossBattleManager.Instance.bossAnimator.SetBool("isRazor", true);
    }

    private void EnableRazorHitBox()
    {
        if (transform.position.x > BossBattleManager.Instance.targetPlayer.transform.position.x)
        {
            // y rotation 180 하지 않고 회전
            razorHitBox.SetActive(true);
            razorHitBox.transform.rotation = Quaternion.Euler(0, 0, 20);
            StartCoroutine(RotateRazor());
        }
        else
        {
            razorHitBox.SetActive(true) ;
            razorHitBox.transform.rotation = Quaternion.Euler(0, 180, 20);
            StartCoroutine(RotateRazor());
        }
    }

    // 회전
    private IEnumerator RotateRazor()
    {
        float elapsedTime = 0f;
        Quaternion startRotation = Quaternion.Euler(razorHitBox.transform.rotation.eulerAngles.x, razorHitBox.transform.rotation.eulerAngles.y, 20f);
        Quaternion endRotation = Quaternion.Euler(razorHitBox.transform.rotation.eulerAngles.x, razorHitBox.transform.rotation.eulerAngles.y, -20f);

        while(elapsedTime < 1f)
        {
            razorHitBox.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void ExitRazorAttack()
    {
        BossBattleManager.Instance.bossAnimator.SetBool("isRazor", false);
        ExitAttack();
    }

    private void ExitAttack()
    {
        BossBattleManager.Instance.ToggleIsAttacking();
        BossBattleManager.Instance.bossStateMachine.ChangeState(BossBattleManager.Instance.bossStateMachine.IdleState);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(stompHitBox.activeInHierarchy);
            stream.SendNext(swingHitBoxLeft.activeInHierarchy);
            stream.SendNext(swingHitBoxRight.activeInHierarchy);
            stream.SendNext(razorHitBox.activeInHierarchy);
        }
        else
        {
            stompHitBox.SetActive((bool)stream.ReceiveNext());
            swingHitBoxLeft.SetActive((bool)stream.ReceiveNext()); ;
            swingHitBoxRight.SetActive((bool)stream.ReceiveNext()); ;
            razorHitBox.SetActive((bool)stream.ReceiveNext()); ;
        }
    }
}
