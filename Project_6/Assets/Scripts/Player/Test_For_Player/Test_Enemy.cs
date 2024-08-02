using UnityEngine;

public class Test_Enemy : MonoBehaviour, IDamagable
{
    public float health = 100;

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name}���� {damage}�� �������� �Ծ����ϴ�. ���� ü��: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name}��(��) �׾����ϴ�.");
        // �״� �ִϸ��̼� �Ǵ� ���� �߰�
        Destroy(gameObject);
    }
}
