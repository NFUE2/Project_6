using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingAura : MonoBehaviour
{
    public float healAmount = 10f; // ����
    public float healDuration = 5f; // �� ���ӽð�
    public float buffDuration = 10f; // ���� ���ӽð�
    public float defenseBoost = 5f; // ���� ������
    public float radius = 10f; // �� ����
    private List<GameObject> healedPlayers = new List<GameObject>();

    void Start()
    {
        StartCoroutine(TriggerHealingAura());
    }

    IEnumerator TriggerHealingAura()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                healedPlayers.Add(hitCollider.gameObject);
                StartCoroutine(HealOverTime(hitCollider.gameObject));
            }
        }

        yield return new WaitForSeconds(healDuration);

        foreach (var player in healedPlayers)
        {
            StartCoroutine(ApplyDefenseBuff(player));
        }

        healedPlayers.Clear();
    }

    IEnumerator HealOverTime(GameObject player)
    {
        float elapsedTime = 0;
        while (elapsedTime < healDuration)
        {
            player.GetComponent<PlayerHealth>().Heal(healAmount * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator ApplyDefenseBuff(GameObject player)
    {
        PlayerStats stats = player.GetComponent<PlayerStats>();
        stats.defense += defenseBoost;
        yield return new WaitForSeconds(buffDuration);
        stats.defense -= defenseBoost;
    }
}

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;

    public void Heal(float amount)
    {
        health += amount;
        // �ִ� ü�� �ʰ� ����
        health = Mathf.Min(health, 100f);
    }
}

public class PlayerStats : MonoBehaviour
{
    public float defense = 10f;
}