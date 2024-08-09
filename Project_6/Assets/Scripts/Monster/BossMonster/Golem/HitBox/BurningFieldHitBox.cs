using UnityEngine;

public class BurningFieldHitBox : HitBox
{
    private float timeSinceLastTriggerStay = 0f;
    private float triggerStayInterval = 0.25f;
    private float curCoolDown;
    private float damageCoolDown = 1f;

    private void OnEnable()
    {
        curDuration = 0;
        duration = 5f;
        curCoolDown = 1f;
    }

    private void Update()
    {
        curDuration += Time.deltaTime;
        curCoolDown += Time.deltaTime;
        if (curDuration >= duration)
        {
            Destroy(gameObject);
        }
        if(curCoolDown >= damageCoolDown)
        {
            curCoolDown = 0;
            Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0);
            foreach (Collider2D col in hit)
            {
                if (col.TryGetComponent<IDamagable>(out IDamagable P) && col.TryGetComponent<IKnockBackable>(out IKnockBackable K))
                {
                    float damage = 5;
                    P.TakeDamage(damage);
                }
            }
        }
    }

    
}