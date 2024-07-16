using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;


public class P_BowE : MonoBehaviour, P_ISkill
{
    public GameObject bombArrow;
    public float fireAngle;

    private Transform hand;

    public float actionTime;
    private float lastAction;

    private void Awake()
    {
        //hand = GetComponent<P_SkillTest>().hand;\
        lastAction = -actionTime;

    }

    public void SkillAction()
    {
        if (Time.time - lastAction < actionTime) return;

        //float startAngle = hand.localEulerAngles.z - fireAngle;

        //Debug.Log(hand.localEulerAngles);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - fireAngle;

        for (int i = 0; i < 3; i++)
        {
            //GameObject go = Instantiate(bombArrow,/*hand.GetChild(0)*/transform.position,Quaternion.identity);
            GameObject go = PhotonNetwork.Instantiate("Prototype/" + bombArrow.name,/*hand.GetChild(0)*/transform.position, Quaternion.identity);

            go.transform.localEulerAngles = new Vector3(0,0,angle + i * fireAngle);
        }
        StartCoroutine(CoolTime());
    }

    IEnumerator CoolTime()
    {
        lastAction = Time.time;
        //Text coolTimeText = GetComponent<PlayerController_Bow>().cooltimeEText;

        while (Time.time - lastAction < actionTime)
        {
            //coolTimeText.text = (actionTime - (Time.time - lastAction)).ToString("F1");
            yield return null;
        }

        //coolTimeText.text = "준비완료";
    }
}
