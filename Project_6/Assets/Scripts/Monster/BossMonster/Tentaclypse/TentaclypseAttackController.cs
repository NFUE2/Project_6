using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class TentaclypseAttackController : BossAttackController
{
    public GameObject razorObject;
    public GameObject bullet;
    public GameObject dispenser;
    public AudioClip razorAudioClip;

    private Vector3[] verticalPositions = new Vector3[7];
    private Vector3[] horizontalPositions = new Vector3[4];

    public override void SelectAttack()
    {
        base.SelectAttack();
        countOfAttack = 4;
        int index = Random.Range(0, countOfAttack);
        switch(index)
        {
            case 0:
                RazorReady();
                break;
            case 1:
                AllRoundAttack(); 
                break;
            case 2:
                RazorRainReady(); 
                break;
            case 3:
                DispenserReady(); 
                break;
        }
    }

    // 레이저
    public void RazorReady()
    {
        BossBattleManager.Instance.ToggleIsAttacking();
        BossBattleManager.Instance.bossAnimator.SetBool("isRazorReady", true);
    }

    private void Razor()
    {
        BossBattleManager.Instance.bossAnimator.SetBool("isRazorReady", false);
        BossBattleManager.Instance.bossAnimator.SetBool("isRazor", true);
        StartCoroutine(RazorObjectCoroutine());
    }
    private IEnumerator RazorObjectCoroutine()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 razorPosition = BossBattleManager.Instance.targetPlayer.transform.position;
            var razor = PhotonNetwork.Instantiate("Boss/" + razorObject.name, Vector2.zero, Quaternion.identity);
            //var razor = Instantiate(razorObject, transform);
            //razor.transform.SetParent(BossBattleManager.Instance.spawnedBoss.transform);
            razor.transform.position = razorPosition;
            Vector3 currentRotation = razor.transform.eulerAngles;
            currentRotation.z = Random.Range(0, 180);
            razor.transform.eulerAngles = currentRotation;
            yield return new WaitForSeconds(1.5f);
        }
    }

    private void RazorEnd()
    {
        BossBattleManager.Instance.bossAnimator.SetBool("isRazor", false);
        ExitAttack();
    }

    // 전방위 탄막
    public void AllRoundAttackReady()
    {
        BossBattleManager.Instance.ToggleIsAttacking();
        BossBattleManager.Instance.bossAnimator.SetBool("isAllRoundAttackReady", true);
    }

    private void AllRoundAttack()
    {
        BossBattleManager.Instance.bossAnimator.SetBool("isAllRoundAttackReady", false);
        BossBattleManager.Instance.bossAnimator.SetBool("isAllRoundAttack", true);
        StartCoroutine(AllRoundAttackCoroutine());
    }

    private IEnumerator AllRoundAttackCoroutine()
    {
        for (int i = 0; i < 5; i++)
        {
            int bulletCount = Random.Range(10, 20);
            int speed = 50;
            float angle = 360 / bulletCount;
            for (int j = 0; j < bulletCount; j++)
            {
                //GameObject go = Instantiate(bullet, BossBattleManager.Instance.spawnedBoss.transform.position, Quaternion.identity);
                GameObject go = PhotonNetwork.Instantiate("Boss/" + bullet.name, BossBattleManager.Instance.spawnedBoss.transform.position, Quaternion.identity);
                //go.transform.SetParent(BossBattleManager.Instance.spawnedBoss.transform);
                go.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * Mathf.Cos(Mathf.PI * 2 * j / bulletCount), speed * Mathf.Sin(Mathf.PI * j * 2 / bulletCount)), ForceMode2D.Force);

                go.transform.Rotate(new Vector3(0, 0, 360 * j / bulletCount - 90));
            }
            yield return new WaitForSeconds(0.75f);
        }
    }

    private void AllRoundAttackEnd()
    {
        BossBattleManager.Instance.bossAnimator.SetBool("isAllRoundAttack", false);
        ExitAttack();
    }

    // 레이저 레인
    public void RazorRainReady()
    {
        BossBattleManager.Instance.ToggleIsAttacking();
        BossBattleManager.Instance.bossAnimator.SetBool("isRazorRainReady", true);
        SetRazorPosition();
    }

    private void SetRazorPosition()
    {
        var bossPosition = BossBattleManager.Instance.spawnedBoss.transform.position;
        for(int i = 0; i < 7; i++)
        {
            verticalPositions[i] = new Vector3(bossPosition.x - 7.5f + (2.5f * i), bossPosition.y, bossPosition.z);
        }
        for(int j = 0; j < 4; j++)
        {
            horizontalPositions[j] = new Vector3(bossPosition.x, bossPosition.y - 4f + (2.5f * j), bossPosition.z);
        }
    }

    private void RazorRain()
    {
        BossBattleManager.Instance.bossAnimator.SetBool("isRazorRainReady", false);
        BossBattleManager.Instance.bossAnimator.SetBool("isRazorRain", true);
        StartCoroutine(RazorRainCoroutine());
    }

    private IEnumerator RazorRainCoroutine()
    {
        GameObject[] vertRazors = new GameObject[7];
        GameObject[] horiRazors = new GameObject[4];
        
        for(int i = 0; i < 7; i++)
        {
            //(복원)var vertRazor1 = PhotonNetwork.Instantiate("Boss/" + razorObject.name, Vector3.zero, Quaternion.identity);
            //vertRazors[i] = Instantiate(razorObject, transform);
            vertRazors[i] = PhotonNetwork.Instantiate("Boss/" + razorObject.name, Vector3.zero, Quaternion.identity);
            //vertRazors[i].transform.SetParent(BossBattleManager.Instance.spawnedBoss.transform);
            Vector3 vertRotation = vertRazors[i].transform.eulerAngles;
            vertRotation.z = 90f;
            vertRazors[i].transform.eulerAngles = vertRotation;
            vertRazors[i].transform.position = verticalPositions[i];
            yield return new WaitForSeconds(0.25f);
        }

       for(int j = 0; j < 4; j++)
        {
            //var horiRazor1 = PhotonNetwork.Instantiate("Boss/" + razorObject.name, Vector3.zero, Quaternion.identity);
            //horiRazors[j] = Instantiate(razorObject, transform);
            horiRazors[j] = PhotonNetwork.Instantiate("Boss/" + razorObject.name, Vector3.zero, Quaternion.identity);
            //horiRazors[j].transform.SetParent(BossBattleManager.Instance.spawnedBoss.transform);
            Vector3 horiRotation = horiRazors[j].transform.eulerAngles;
            horiRotation.z = 0f;
            horiRazors[j].transform.eulerAngles = horiRotation;
            horiRazors[j].transform.position = horizontalPositions[j];
            yield return new WaitForSeconds(0.45f);
        }
    }

    private void RazorRainEnd()
    {
        BossBattleManager.Instance.bossAnimator.SetBool("isRazorRain", false);
        ExitAttack();
    }

    // 디스펜서
    public void DispenserReady()
    {
        BossBattleManager.Instance.ToggleIsAttacking();
        BossBattleManager.Instance.bossAnimator.SetBool("isDispenserReady", true);
    }

    private void Dispenser()
    {
        BossBattleManager.Instance.bossAnimator.SetBool("isDispenserReady", false);
        BossBattleManager.Instance.bossAnimator.SetBool("isDispenser", true);
        var bossPosition = BossBattleManager.Instance.spawnedBoss.transform.position;
        Vector3 randPosLeft = new Vector3(bossPosition.x - Random.Range(4, 8), bossPosition.y + Random.Range(2.5f, 4f), 0);
        Vector3 randPosRight = new Vector3(bossPosition.x + Random.Range(4, 8), bossPosition.y + Random.Range(2.5f, 4f), 0);
        StartCoroutine(DispenserAttack(randPosLeft, randPosRight));
    }

    private IEnumerator DispenserAttack(Vector3 randPosLeft, Vector3 randPosRight)
    {
        var dispenserLeft = PhotonNetwork.Instantiate("Boss/" + dispenser.name, transform.localPosition, Quaternion.identity);
        //var dispenserLeft = Instantiate(dispenser);
        //dispenserLeft.transform.SetParent(BossBattleManager.Instance.spawnedBoss.transform);
        //dispenserLeft.transform.position = BossBattleManager.Instance.spawnedBoss.transform.position;

        var dispenserRight = PhotonNetwork.Instantiate("Boss/" + dispenser.name, transform.localPosition, Quaternion.identity);
        //var dispenserRight = Instantiate(dispenser);
        //dispenserRight.transform.SetParent(BossBattleManager.Instance.spawnedBoss.transform);

        //dispenserRight.transform.position = BossBattleManager.Instance.spawnedBoss.transform.position;
        float countTime = 0;

        while (countTime < 2f)
        {
            Debug.Log("이동중");
            dispenserLeft.transform.position = Vector3.Lerp(dispenserLeft.transform.position, randPosLeft, countTime / 2f);
            dispenserRight.transform.position = Vector3.Lerp(dispenserRight.transform.position, randPosRight, countTime / 2f);
            countTime += Time.deltaTime;
            yield return null;
        }
        dispenserLeft.transform.position = randPosLeft;
        dispenserRight.transform.position = randPosRight;
        StartCoroutine(DispenserAttackCoroutine(randPosLeft, randPosRight));
    }

    private IEnumerator DispenserAttackCoroutine(Vector3 randPosLeft, Vector3 randPosRight)
    {
        for (int i = 0; i < 3; i++)
        {
            int bulletCount = Random.Range(7, 14);
            int speed = 50;
            float angle = 360 / bulletCount;
            for (int j = 0; j < bulletCount; j++)
            {
                GameObject go1 = PhotonNetwork.Instantiate("Boss/" + bullet.name, dispenser.transform.position, Quaternion.identity);
                //GameObject go1 = Instantiate(bullet, dispenser.transform.position, Quaternion.identity);
                //go1.transform.SetParent(BossBattleManager.Instance.spawnedBoss.transform);

                go1.transform.position = randPosLeft;
                go1.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * Mathf.Cos(Mathf.PI * 2 * j / bulletCount), speed * Mathf.Sin(Mathf.PI * j * 2 / bulletCount)), ForceMode2D.Force);
                go1.transform.Rotate(new Vector3(0, 0, 360 * j / bulletCount - 90));

                GameObject go2 = PhotonNetwork.Instantiate("Boss/" + bullet.name, dispenser.transform.position, Quaternion.identity);
                //GameObject go2 = Instantiate(bullet, dispenser.transform.position, Quaternion.identity);
                //go2.transform.SetParent(BossBattleManager.Instance.spawnedBoss.transform);
                go2.transform.position = randPosRight;
                go2.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * Mathf.Cos(Mathf.PI * 2 * j / bulletCount), speed * Mathf.Sin(Mathf.PI * j * 2 / bulletCount)), ForceMode2D.Force);
                go2.transform.Rotate(new Vector3(0, 0, 360 * j / bulletCount - 90));
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void DispenserAttackEnd()
    {
        BossBattleManager.Instance.bossAnimator.SetBool("isDispenser", false);
        ExitAttack();
    }
}