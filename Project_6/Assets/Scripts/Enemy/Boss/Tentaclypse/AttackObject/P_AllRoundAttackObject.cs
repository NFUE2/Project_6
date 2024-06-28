using UnityEngine;

public class P_AllRoundAttackObject : MonoBehaviour
{
    private GameObject boss;
    private P_Tentaclypse tentaclypse;
    private float keepingTime = 5;
    private float curTime = 0;

    private void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        tentaclypse = boss.GetComponent<P_Tentaclypse>();
    }

    private void Update()
    {
        curTime += Time.deltaTime;
        if(curTime >= keepingTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCondition player = collision.GetComponent<PlayerCondition>();
            player.TakeDamage(tentaclypse.bossPower);
            Destroy(gameObject);
        }
    }
}