using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_TentaclypseAllRoundShotAttack : MonoBehaviour, IAttackPattern
{
    private GameObject boss;
    private P_Tentaclypse tentaclypse;
    private GameObject bullet;

    private void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        tentaclypse = boss.GetComponent<P_Tentaclypse>();
        bullet = tentaclypse.allRoundAttackObject;
    }

    public void ExecuteAttack()
    {
        Debug.Log("전 방위 탄막 사격 개시");
        StartCoroutine(AllRoundAttackCoroutine());

    }

    private IEnumerator AllRoundAttackCoroutine()
    {
        for(int i = 0; i < 5; i++)
        {
            int bulletCount = Random.Range(15, 30);
            int speed = Random.Range(75, 100);
            float angle = 360 / bulletCount;
            for (int j = 0; j < bulletCount; j++)
            {
                GameObject go = Instantiate(bullet, boss.transform.position, Quaternion.identity);
                go.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * Mathf.Cos(Mathf.PI * 2 * j / bulletCount), speed * Mathf.Sin(Mathf.PI * j * 2 / bulletCount)), ForceMode2D.Force);

                go.transform.Rotate(new Vector3(0, 0, 360 * j / bulletCount - 90));
            }
            yield return new WaitForSeconds(0.75f);
        }
    }
}
