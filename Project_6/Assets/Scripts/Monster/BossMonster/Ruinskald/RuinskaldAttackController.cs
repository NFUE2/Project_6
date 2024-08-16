using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuinskaldAttackController : BossAttackController
{
    private Rigidbody2D bossRB;
    public MovableBoss movableBoss;
    private float originalSpeed;
    private string rollAnimation = "Roll";
    private float rollForce = 10f;

    [Header("Slash Attack")]
    public GameObject slashObjectR;
    public GameObject slashObjectL;

    [Header("Explosion Attack")]
    public GameObject decisionObject;
    public GameObject explosionObjectInside;
    public GameObject explosionObjectOutsideL;
    public GameObject explosionObjectOutsiedR;
    private string expAnimation = "Explosion";
    private bool isInside = false;

    [Header("Counter Attack")]
    public GameObject counterAttackHitBox;
    private string defenseAnimation = "CounterAttack";
    private string counterAttackAnimation = "CounterAttackSlash";
    private float currentBossHP;
    public float counterStandard = 75;

    [Header("Boomerang Attack")]
    public GameObject boomerangHitBox;
    private string boomerangAnimation = "BoomerangAttack";

    private void Start()
    {
        bossRB = BossBattleManager.Instance.spawnedBoss.GetComponent<Rigidbody2D>();
        originalSpeed = BossBattleManager.Instance.boss.moveSpeed;
    }
    public override void SelectAttack()
    {
        base.SelectAttack();

        int index = Random.Range(0, 5);

        switch (index)
        {
            case 0:
                Roll();
                break;
            case 1:
                Slash();
                break;
            case 2:
                Explosion();
                break;
            case 3:
                CounterAttack();
                break;
            case 4:
                Boomerang();
                break;
        }
    }

    // 패턴 전 구르기
    private void Roll()
    {
        float rollDirection = BossBattleManager.Instance.spawnedBoss.transform.position.x < BossBattleManager.Instance.targetPlayer.transform.position.x ? 1 : -1;
        bossRB.velocity = new Vector2(rollForce * rollDirection, bossRB.velocity.y);
        BossBattleManager.Instance.bossAnimator.Play(rollAnimation);
    }

    private void AfterRoll()
    {
        bossRB.velocity = Vector2.zero;
        int action = Random.Range(0, 5);
        if(action == 1)
        {
            Slash();
        }
        else if(action == 2)
        {
            Boomerang();
        }
    }
    // 견제형 패턴

    private void Boomerang()
    {
        movableBoss.speed = 0;
        BossBattleManager.Instance.bossAnimator.Play(boomerangAnimation);
        ShootBoomerang();
    }

    private void ShootBoomerang()
    {
        var boomerang = Instantiate(boomerangHitBox);
        boomerang.transform.SetParent(transform.parent);
        boomerang.transform.position = BossBattleManager.Instance.spawnedBoss.transform.position;
        Vector3 goalPos1;
        Vector3 goalPos2;
        if(BossBattleManager.Instance.targetPlayer.transform.position.x >= BossBattleManager.Instance.spawnedBoss.transform.position.x)
        {
            goalPos1 = boomerang.transform.position + new Vector3(10, 0, 0);
            goalPos2 = boomerang.transform.position - new Vector3(10, 0, 0);
        }
        else
        {
            goalPos2 = boomerang.transform.position + new Vector3(10, 0, 0);
            goalPos1 = boomerang.transform.position - new Vector3(10, 0, 0);
        }

        StartCoroutine(BoomerangCoroutine(boomerang, goalPos1, goalPos2));
    }

    private IEnumerator BoomerangCoroutine(GameObject go, Vector3 goatPos1, Vector3 goatPos2)
    {
        Vector3 startPosition = go.transform.position;
        float elapsedTime1 = 0;
        float elapsedTime2 = 0;
        while (elapsedTime1 < 0.5f)
        {
            float t = elapsedTime1 / 0.5f;
            go.transform.position = Vector3.Lerp(startPosition, goatPos1, t);
            elapsedTime1 += Time.deltaTime;
            yield return null;
        }
        startPosition = go.transform.position;
        while (elapsedTime2 < 1.0f)
        {
            float t = (elapsedTime2 / 1.0f);
            go.transform.position = Vector3.Lerp(startPosition, goatPos2, t);
            elapsedTime2 += Time.deltaTime;
            yield return null;
        }
        Destroy(go);
        ExitAttack();
    }


    private void Slash()
    {
        movableBoss.speed = 0;
        BossBattleManager.Instance.bossAnimator.SetBool("isSlash", true);
    }

    private void SlashObjectEnabled()
    {
        if(BossBattleManager.Instance.targetPlayer.transform.position.x >= BossBattleManager.Instance.spawnedBoss.transform.position.x)
        {
            slashObjectR.SetActive(true);
            Collider2D[] hit = Physics2D.OverlapBoxAll(slashObjectR.transform.position, slashObjectR.transform.localScale, 0);
            foreach(Collider2D col in hit)
            {
                if(col.TryGetComponent<IDamagable>(out IDamagable P))
                {
                    float damage = BossBattleManager.Instance.boss.attackPower * 0.8f;
                    P.TakeDamage(damage);
                }
            }
        }
        else
        {
            slashObjectL.SetActive(true);
            Collider2D[] hit = Physics2D.OverlapBoxAll(slashObjectL.transform.position, slashObjectL.transform.localScale, 0);
            foreach (Collider2D col in hit)
            {
                if (col.TryGetComponent<IDamagable>(out IDamagable P))
                {
                    float damage = BossBattleManager.Instance.boss.attackPower * 0.8f;
                    P.TakeDamage(damage);
                }
            }
        }
        
    }

    private void SlashAnimControll()
    {
        if(BossBattleManager.Instance.bossAnimator.GetBool("isSlash") == true)
        {
            int attack = Random.Range(0, 3);
            if(attack == 0)
            {
                BossBattleManager.Instance.bossAnimator.SetBool("isSlash", false);
                ExitAttack();
            }
            else if(attack == 1)
            {
                BossBattleManager.Instance.bossAnimator.SetBool("isSlash", false);
                BossBattleManager.Instance.bossAnimator.SetBool("isSlash2", true);
            }
            else if(attack == 2)
            {
                BossBattleManager.Instance.bossAnimator.SetBool("isSlash", false);
                BossBattleManager.Instance.bossAnimator.SetBool("isSlash3", true);
            }
        }
        else if(BossBattleManager.Instance.bossAnimator.GetBool("isSlash2") == true)
        {
            int attack = Random.Range(0, 2);
            if(attack == 0)
            {
                BossBattleManager.Instance.bossAnimator.SetBool("isSlash2", false);
                ExitAttack();
            }
            else if( attack == 1)
            {
                BossBattleManager.Instance.bossAnimator.SetBool("isSlash2", false);
                BossBattleManager.Instance.bossAnimator.SetBool("isSlash3", true);
            }
        }
        else if (BossBattleManager.Instance.bossAnimator.GetBool("isSlash3") == true)
        {
            BossBattleManager.Instance.bossAnimator.SetBool("isSlash3", false);
            ExitAttack();
        }
        else
        {
            ExitAttack();
        }
    }

    private void Explosion()
    {
        movableBoss.speed = 0;
        BossBattleManager.Instance.bossAnimator.SetBool("isWalk", false);
        int dir = Random.Range(0, 2);
        if (dir == 0) // in
        {
            isInside = true;
            var dirObject = Instantiate(decisionObject);
            dirObject.transform.SetParent(gameObject.transform);
            Vector3 bossPos = BossBattleManager.Instance.spawnedBoss.transform.position;
            dirObject.transform.position = new Vector3(bossPos.x + 5, bossPos.y + 3, 0);
            Vector3 goalPos = new Vector3(dirObject.transform.position.x - 10, dirObject.transform.position.y, 0);
            StartCoroutine(DecisionObjectMoveCoroutine(dirObject, goalPos));
        }
        else
        {
            isInside = false;
            var dirObject = Instantiate(decisionObject);
            dirObject.transform.SetParent(gameObject.transform);
            Vector3 bossPos = BossBattleManager.Instance.spawnedBoss.transform.position;
            dirObject.transform.position = new Vector3(bossPos.x - 5, bossPos.y + 3, 0);
            Vector3 goalPos = new Vector3(dirObject.transform.position.x + 10, dirObject.transform.position.y, 0);
            StartCoroutine(DecisionObjectMoveCoroutine(dirObject, goalPos));
        }
    }

    private IEnumerator DecisionObjectMoveCoroutine(GameObject o, Vector3 goalPos)
    {
        Vector3 startPosition = o.transform.position;
        float elapsedTime = 0;
        while(elapsedTime < 2.0f)
        {
            float t = elapsedTime / 2.0f;
            o.transform.position = Vector3.Lerp(startPosition, goalPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        o.transform.position = goalPos;
        DoExplosion();
    }

    private void DoExplosion()
    {
        BossBattleManager.Instance.bossAnimator.Play(expAnimation);
        if (isInside)
        {
            explosionObjectInside.SetActive(true);
            Collider2D[] hit = Physics2D.OverlapBoxAll(explosionObjectInside.transform.position, explosionObjectInside.transform.localScale, 0);
            foreach (Collider2D col in hit)
            {
                if (col.TryGetComponent<IDamagable>(out IDamagable P))
                {
                    float damage = BossBattleManager.Instance.boss.attackPower * 1.5f;
                    P.TakeDamage(damage);
                }
            }
        }
        else
        {
            explosionObjectOutsideL.SetActive(true) ;
            explosionObjectOutsiedR.SetActive(true) ;
            Collider2D[] hit1 = Physics2D.OverlapBoxAll(explosionObjectOutsideL.transform.position, explosionObjectOutsideL.transform.localScale, 0);
            Collider2D[] hit2 = Physics2D.OverlapBoxAll(explosionObjectOutsiedR.transform.position, explosionObjectOutsiedR.transform.localScale, 0);
            List<Collider2D> hitList = new List<Collider2D>(hit1);
            hitList.AddRange(hit2);
            foreach (Collider2D col in hitList)
            {
                if (col.TryGetComponent<IDamagable>(out IDamagable P))
                {
                    float damage = BossBattleManager.Instance.boss.attackPower * 1.5f;
                    P.TakeDamage(damage);
                }
            }
        }
    }

    private void CounterAttack()
    {
        movableBoss.speed = 0;
        currentBossHP = BossBattleManager.Instance.boss.currentHp;
        BossBattleManager.Instance.bossAnimator.Play(defenseAnimation);
    }

    private void DecisionCounterAttack()
    {
        if(currentBossHP - BossBattleManager.Instance.boss.currentHp >= counterStandard)
        {
            BossBattleManager.Instance.bossAnimator.Play(counterAttackAnimation);
        }
        else
        {
            ExitAttack();
        }
    }

    private void CounterAttackDamage()
    {
        float damage = BossBattleManager.Instance.boss.attackPower;
        counterAttackHitBox.SetActive(true);
        Collider2D[] hit = Physics2D.OverlapBoxAll(counterAttackHitBox.transform.position, counterAttackHitBox.transform.localScale, 0);
        foreach (Collider2D col in hit)
        {
            if (col.TryGetComponent<IDamagable>(out IDamagable P))
            {
                P.TakeDamage(damage);
            }
        }
    }

    private void ExitTestAttack()
    {
        movableBoss.speed = originalSpeed;
        BossBattleManager.Instance.ToggleIsAttacking();
        BossBattleManager.Instance.bossStateMachine.ChangeState(BossBattleManager.Instance.bossStateMachine.IdleState);
    }
    public new void ExitAttack()
    {
        movableBoss.speed = originalSpeed;
        BossBattleManager.Instance.ToggleIsAttacking();
        BossBattleManager.Instance.bossStateMachine.ChangeState(BossBattleManager.Instance.bossStateMachine.IdleState);
    }
    // 
}
