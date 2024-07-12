using UnityEngine;

public class BurningFieldHitBox : HitBox
{
    private float timeSinceLastTriggerStay = 0f;
    private float triggerStayInterval = 0.25f;

    private void OnEnable()
    {
        curDuration = 0;
        duration = 5f;
    }

    private void Update()
    {
        curDuration += Time.deltaTime;
        if (curDuration >= duration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Time.time >= timeSinceLastTriggerStay + triggerStayInterval)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log($"{collision.gameObject.name}�� �����ǿ� ���� ��Ʈ �������� �԰� �ֽ��ϴ�.");
                //if(collision.TryGetComponent<IDamagable>(out IDamagable P))
                //{
                //    float damage = BossBattleManager.Instance.boss.attackPower * 0.1f;
                //    P.TakeDamage(damage);
                //}
                timeSinceLastTriggerStay = Time.time;
            }
        }
    }
}