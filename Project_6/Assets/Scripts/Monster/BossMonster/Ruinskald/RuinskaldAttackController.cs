using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuinskaldAttackController : BossAttackController
{
    private Rigidbody2D bossRB;
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

    private void Start()
    {
        bossRB = BossTestManager.Instance.spawnedBoss.GetComponent<Rigidbody2D>();
    }
    public override void SelectAttack()
    {
        base.SelectAttack();

        int index = 2;//Random.Range(0, 1);

        switch (index)
        {
            case 0:
                Roll();
                //Slash();
                break;
            case 1:
                Slash();
                break;
            case 2:
                Explosion();
                break;
        }
    }

    // 패턴 전 구르기
    private void Roll()
    {
        float rollDirection = BossTestManager.Instance.spawnedBoss.transform.position.x < BossTestManager.Instance.targetPlayer.transform.position.x ? 1 : -1;
        bossRB.velocity = new Vector2(rollForce * rollDirection, bossRB.velocity.y);
        BossTestManager.Instance.bossAnimator.Play(rollAnimation);
    }

    private void AfterRoll()
    {
        bossRB.velocity = Vector2.zero;
        int action = Random.Range(0, 3);
        if(action == 1)
        {
            Slash();
        }
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
            Collider2D[] hit = Physics2D.OverlapBoxAll(slashObjectR.transform.position, slashObjectR.transform.localScale, 0);
            foreach(Collider2D col in hit)
            {
                if(col.TryGetComponent<IDamagable>(out IDamagable P))
                {
                    float damage = BossTestManager.Instance.boss.attackPower * 0.8f;
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
                    float damage = BossTestManager.Instance.boss.attackPower * 0.8f;
                    P.TakeDamage(damage);
                }
            }
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
        else
        {
            ExitTestAttack();
        }
    }

    private void Explosion()
    {
        int dir = Random.Range(0, 2);
        if (dir == 0) // in
        {
            isInside = true;
            var dirObject = Instantiate(decisionObject);
            dirObject.transform.SetParent(gameObject.transform);
            Vector3 bossPos = BossTestManager.Instance.spawnedBoss.transform.position;
            dirObject.transform.position = new Vector3(bossPos.x + 5, bossPos.y + 3, 0);
            Vector3 goalPos = new Vector3(dirObject.transform.position.x - 10, dirObject.transform.position.y, 0);
            StartCoroutine(DecisionObjectMoveCoroutine(dirObject, goalPos));
        }
        else
        {
            isInside = false;
            var dirObject = Instantiate(decisionObject);
            dirObject.transform.SetParent(gameObject.transform);
            Vector3 bossPos = BossTestManager.Instance.spawnedBoss.transform.position;
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
        BossTestManager.Instance.bossAnimator.Play(expAnimation);
        if (isInside)
        {
            explosionObjectInside.SetActive(true);
            Collider2D[] hit = Physics2D.OverlapBoxAll(explosionObjectInside.transform.position, explosionObjectInside.transform.localScale, 0);
            foreach (Collider2D col in hit)
            {
                if (col.TryGetComponent<IDamagable>(out IDamagable P))
                {
                    float damage = BossTestManager.Instance.boss.attackPower * 1.5f;
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
                    float damage = BossTestManager.Instance.boss.attackPower * 1.5f;
                    P.TakeDamage(damage);
                }
            }
        }
    }

    private void ExitTestAttack()
    {
        BossTestManager.Instance.ToggleIsAttacking();
        BossTestManager.Instance.bossStateMachine.ChangeState(BossTestManager.Instance.bossStateMachine.IdleState);
    }
    // 
}
