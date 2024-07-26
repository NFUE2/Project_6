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
    public GameObject fireBallHitBox;

    public AudioClip swingAudioClip;
    public AudioClip stompAudioClip;
    public AudioClip razorAudioClip;
    public AudioClip fireBallAudioClip;

    public BoxCollider2D bossCollider;
    public BoxCollider2D chargeCollider;

    private float beforeChargeHP;
    private float afterChargeHP;
    private float chargeCancleDamage = 50;

    private int additionalAttack;

    public override void SelectAttack()
    {
        base.SelectAttack();
        countOfAttack = 3;
        float distanceToTarget = BossBattleManager.Instance.distanceToTarget;
        Debug.Log(distanceToTarget);
        if(distanceToTarget > 7)
        {
            //RazorReady();
            FireBallReady();
        }
        else if(distanceToTarget <= 7)
        {
            int index = Random.Range(0, countOfAttack - 1);
            switch (index)
            {
                case 0:
                    StompReady();
                    break;
                case 1:
                    SwingReady();
                    break;
                case 2:
                    SwingReady();
                    break;
                case 3:
                    Charge();
                    break;
            }
        }
        
        //BossBattleManager.Instance.bossStateMachine.ChangeState(BossBattleManager.Instance.bossStateMachine.IdleState);
    }

    // ����
    public void StompReady()
    {
        BossBattleManager.Instance.ToggleIsAttacking();
        BossBattleManager.Instance.bossAnimator.SetBool("isStompReady", true);
    }

    private void Stomp()
    {
        //SoundManager.Instance.Shot(stompAudioClip);
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

    // ����
    // ���� + ����
    public void SwingReady()
    {
        BossBattleManager.Instance.ToggleIsAttacking();
        BossBattleManager.Instance.bossAnimator.SetBool("isSwingReady", true);
    }

    private void Swing()
    {
        //SoundManager.Instance.Shot(swingAudioClip);
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

    // ������
    public void RazorReady()
    {
        BossBattleManager.Instance.ToggleIsAttacking();
        BossBattleManager.Instance.bossAnimator.SetBool("isRazorReady", true);
    }

    private void Razor()
    {
        //SoundManager.Instance.Shot(razorAudioClip);
        BossBattleManager.Instance.bossAnimator.SetBool("isRazorReady", false);
        BossBattleManager.Instance.bossAnimator.SetBool("isRazor", true);
    }

    private void EnableRazorHitBox()
    {
        if (transform.position.x > BossBattleManager.Instance.targetPlayer.transform.position.x)
        {
            // y rotation 180 ���� �ʰ� ȸ��
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

    // ȸ��
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

    // ����
    public void Charge()
    {
        BossBattleManager.Instance.ToggleIsAttacking();
        BossBattleManager.Instance.bossAnimator.SetBool("isCharging", true);
        bossCollider.enabled = false;
        chargeCollider.enabled = true;
        beforeChargeHP = BossBattleManager.Instance.boss.currentHp;
    }

    private void EndCharge()
    {
        afterChargeHP = BossBattleManager.Instance.boss.currentHp;
        chargeCollider.enabled = false;
        bossCollider.enabled = true;
        BossBattleManager.Instance.bossAnimator.SetBool("isCharging", false);
        if(beforeChargeHP - afterChargeHP >= chargeCancleDamage)
        {
            BossBattleManager.Instance.bossAnimator.SetBool("isFaint", true);
        }
        else
        {
            BossBattleManager.Instance.bossAnimator.SetBool("isChargePunch", true);
        }
        BossBattleManager.Instance.bossAnimator.SetBool("isCharging", true);
    }

    private void FaintEnd()
    {
        BossBattleManager.Instance.bossAnimator.SetBool("isFaint", false);
        ExitAttack() ;
    }

    private void ChargePunch()
    {
        //SoundManager.Instance.Shot(swingAudioClip);
        foreach (GameObject P in BossBattleManager.Instance.players)
        {
            PlayerCondition condition = P.GetComponent<PlayerCondition>();
            condition.TakeDamage(BossBattleManager.Instance.boss.attackPower);
        }
    }

    private void EndChargePunch()
    {
        BossBattleManager.Instance.bossAnimator.SetBool("isChargePunch", false);
        ExitAttack() ;
    }

    public void FireBallReady()
    {
        BossBattleManager.Instance.ToggleIsAttacking();
        BossBattleManager.Instance.bossAnimator.SetBool("isFireBallReady", true);
    }
    
    private void SpawnFireBall()
    {
        GameObject fireBall = Instantiate(fireBallHitBox);
        fireBall.transform.SetParent(BossBattleManager.Instance.spawnedBoss.transform);
        fireBall.transform.position += new Vector3(0, 5, 0);
    }

    private void FireBall()
    {
        BossBattleManager.Instance.bossAnimator.SetBool("isFireBallReady", false);
        BossBattleManager.Instance.bossAnimator.SetBool("isFireBall", true);
    }

    private void FireBallEnd()
    {
        BossBattleManager.Instance.bossAnimator.SetBool("isFireBall", false);
        ExitAttack();
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
