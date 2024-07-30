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
    private bool chargeAttackReady = true;

    public override void SelectAttack()
    {
        base.SelectAttack();
        countOfAttack = 5;
        int countOfMeleeAttack = 3;
        int countOfDistanceAttack = 2;
        float distanceToTarget = BossBattleManager.Instance.distanceToTarget;
        Debug.Log(distanceToTarget);
        if(distanceToTarget > 7)
        {
            int index = Random.Range(0, countOfDistanceAttack);
            switch (index)
            {
                case 0:
                    RazorReady(); 
                    break;
                case 1:
                    FireBallReady();
                    break;
            }
        }
        else if(distanceToTarget <= 7)
        {
            int index = Random.Range(0, countOfMeleeAttack);
            switch (index)
            {
                case 0:
                    StompReady();
                    break;
                case 1:
                    SwingReady();
                    break;
                case 2:
                    Charge();
                    break;
            }
        }
    }

    // 지진
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

    // 스윙
    // 스윙 + 지진
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
            //swingHitBoxLeft.SetActive(true);
            var bossPos = BossBattleManager.Instance.spawnedBoss.transform.position;
            Collider2D[] hit = Physics2D.OverlapBoxAll(new Vector2(bossPos.x + 0.5f, bossPos.y), new Vector2(10, 3), 0);
            foreach(Collider2D col in hit)
            {
                if(col.TryGetComponent<IDamagable>(out IDamagable P) && col.TryGetComponent<IKnockBackable>(out IKnockBackable K))
                {
                    float damage = BossBattleManager.Instance.boss.attackPower * 0.75f;
                    P.TakeDamage(damage);
                    Vector2 playerPos = col.transform.position;
                    Vector2 knockbackDirection = bossPos.x < playerPos.x ? new Vector2(1, 0) : new Vector2(-1, 0);
                    K.ApplyKnockback(knockbackDirection, 5);
                }
            }
        }
        else
        {
            //swingHitBoxRight.SetActive(true);
            var bossPos = BossBattleManager.Instance.spawnedBoss.transform.position;
            Collider2D[] hit = Physics2D.OverlapBoxAll(new Vector2(bossPos.x - 0.5f, bossPos.y), new Vector2(5, 1), 0);
            foreach (Collider2D col in hit)
            {
                if (col.TryGetComponent<IDamagable>(out IDamagable P) && col.TryGetComponent<IKnockBackable>(out IKnockBackable K))
                {
                    float damage = BossBattleManager.Instance.boss.attackPower * 0.75f;
                    P.TakeDamage(damage);
                    Vector2 playerPos = col.transform.position;
                    Vector2 knockbackDirection = bossPos.x < playerPos.x ? new Vector2(1, 0) : new Vector2(-1, 0);
                    K.ApplyKnockback(knockbackDirection, 5);
                }
            }
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
        //SoundManager.Instance.Shot(razorAudioClip);
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

    // 차지
    public void Charge()
    {
        if(chargeAttackReady)
        {
            Debug.Log("차지");
            chargeAttackReady = false;
            BossBattleManager.Instance.ToggleIsAttacking();
            bossCollider.enabled = false;
            chargeCollider.enabled = true;
            beforeChargeHP = BossBattleManager.Instance.boss.currentHp;
            BossBattleManager.Instance.bossAnimator.SetBool("isCharging", true);
        }
        else
        {
            chargeAttackReady = true;
            SelectAttack();
        }
    }

    private void EndCharge()
    {
        BossBattleManager.Instance.bossAnimator.SetBool("isCharging", false);
        afterChargeHP = BossBattleManager.Instance.boss.currentHp;
        chargeCollider.enabled = false;
        bossCollider.enabled = true;
        if(beforeChargeHP - afterChargeHP >= chargeCancleDamage)
        {
            BossBattleManager.Instance.bossAnimator.SetBool("isFaint", true);
        }
        else
        {
            BossBattleManager.Instance.bossAnimator.SetBool("isChargePunch", true);
        }
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
            Vector2 playerPos = P.transform.position;
            Vector2 bossPos = BossBattleManager.Instance.spawnedBoss.transform.position;
            Vector2 knockbackDirection = bossPos.x < playerPos.x ? new Vector2(1, 0) : new Vector2(-1, 0);
            condition.ApplyKnockback(knockbackDirection, 5);
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
