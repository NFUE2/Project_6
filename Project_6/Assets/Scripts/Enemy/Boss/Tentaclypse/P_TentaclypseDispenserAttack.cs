using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_TentaclypseDispenserAttack : MonoBehaviour, IAttackPattern
{
    private GameObject boss;
    private P_Tentaclypse tentaclypse;
    private GameObject dispenser;
    private GameObject bullet;

    private void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        tentaclypse = boss.GetComponent<P_Tentaclypse>();
        dispenser = tentaclypse.dispenser;
        bullet = tentaclypse.allRoundAttackObject;
    }

    public void ExecuteAttack()
    {
        Vector3 randPosLeft = new Vector3(Random.Range(-8, -4), Random.Range(2.5f, 4), 0);
        Vector3 randPosRight = new Vector3(Random.Range(8, 4), Random.Range(2.5f, 4), 0);
        Debug.Log("폭죽 공격 패턴 실시");
        StartCoroutine(DispenserAttack(randPosLeft, randPosRight));
    }

    private IEnumerator DispenserAttack(Vector3 randPosLeft, Vector3 randPosRight)
    {
        var dispenserLeft = Instantiate(dispenser);
        dispenserLeft.transform.position = boss.transform.position;
        var dispenserRight = Instantiate(dispenser);
        dispenserRight.transform.position = boss.transform.position;
        float countTime = 0;

        while(countTime < 2f)
        {
            Debug.Log("이동중");
            dispenserLeft.transform.position = Vector3.Lerp(dispenserLeft.transform.position, randPosLeft, countTime / 2f);
            dispenserRight.transform.position = Vector3.Lerp(dispenserRight.transform.position, randPosRight, countTime / 2f);
            countTime += Time.deltaTime;
            yield return null;
        }
        dispenserLeft.transform.position = randPosLeft;
        dispenserRight.transform.position = randPosRight;
        StartCoroutine(AllRoundAttackCoroutine(randPosLeft, randPosRight ));
    }
    private IEnumerator AllRoundAttackCoroutine(Vector3 randPosLeft, Vector3 randPosRight)
    {
        for (int i = 0; i < 3; i++)
        {
            int bulletCount = Random.Range(10, 20);
            int speed = 100;
            float angle = 360 / bulletCount;
            for (int j = 0; j < bulletCount; j++)
            {
                GameObject go1 = Instantiate(bullet, dispenser.transform.position, Quaternion.identity);
                go1.transform.position = randPosLeft;
                go1.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * Mathf.Cos(Mathf.PI * 2 * j / bulletCount), speed * Mathf.Sin(Mathf.PI * j * 2 / bulletCount)), ForceMode2D.Force);
                go1.transform.Rotate(new Vector3(0, 0, 360 * j / bulletCount - 90));

                GameObject go2 = Instantiate(bullet, dispenser.transform.position, Quaternion.identity);
                go2.transform.position = randPosRight;
                go2.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * Mathf.Cos(Mathf.PI * 2 * j / bulletCount), speed * Mathf.Sin(Mathf.PI * j * 2 / bulletCount)), ForceMode2D.Force);
                go2.transform.Rotate(new Vector3(0, 0, 360 * j / bulletCount - 90));
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
