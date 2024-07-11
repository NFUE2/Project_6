using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float damage;
    public float radius;
    public float dotDuration;

    private void Start()
    {
        // 일정 시간 후 폭발
        Invoke("Explode", 3f);
    }

    public void Initialize(float damage, float radius, float dotDuration)
    {
        this.damage = damage;
        this.radius = radius;
        this.dotDuration = dotDuration;
    }

    private void Explode()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Enemy"));

        foreach (var enemy in hitEnemies)
        {
            StartCoroutine(ApplyDotDamage(enemy));
            // 주위 범위 데미지 적용
            //enemy.GetComponent<Enemy>().TakeDamage(damage);
        }

        Destroy(gameObject); // 투사체 파괴
    }

    private IEnumerator ApplyDotDamage(Collider2D enemy)
    {
        float elapsed = 0f;
        float interval = 1f;
        float dotDamage = damage / dotDuration;

        while (elapsed < dotDuration)
        {
            if (enemy != null)
            {
                //enemy.GetComponent<Enemy>().TakeDamage(dotDamage);
            }

            elapsed += interval;
            yield return new WaitForSeconds(interval);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}