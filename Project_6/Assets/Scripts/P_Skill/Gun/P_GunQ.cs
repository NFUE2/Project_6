using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class P_GunQ : MonoBehaviour, P_ISkill
{
    //플레이어 객체 가져와야함
    public GameObject bullet;
    private Transform hand;
    public bool fanningReady { get; private set; }

    private void Awake()
    {
        //hand = GetComponent<P_SkillTest>().hand;
    }

    public void SkillAction()
    {
        Debug.Log(1);
        //재장전 스크립트 필요
        if (fanningReady) return;

        fanningReady = GetComponent<PlayerController_Gun>().fanningReady =true;
        StartCoroutine(Fanning());
    }

    IEnumerator Fanning()
    {
        while(!Input.GetMouseButtonDown(0))
            yield return null;

        for(int i = 0; i < 6; i++)
        {
            float fireAngle = Random.Range(-3f, 3f);
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(mousePos.y,mousePos.x) * Mathf.Rad2Deg;

            Debug.Log(mousePos);

            GameObject go = Instantiate(bullet, transform.position, Quaternion.identity);
            go.transform.localEulerAngles = /*hand.localEulerAngles + */new Vector3(0, 0,angle + fireAngle);

            yield return new WaitForSeconds(0.1f);
        }
        fanningReady = GetComponent<PlayerController_Gun>().fanningReady = false;

        StartCoroutine(GetComponent<PlayerController_Gun>().AttackCooldown());
    }
}
