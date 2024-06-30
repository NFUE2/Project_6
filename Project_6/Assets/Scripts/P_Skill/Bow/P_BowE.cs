using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.XR;

public class P_BowE : MonoBehaviour, P_ISkill
{
    public GameObject bombArrow;
    public float fireAngle;

    private Transform hand;

    private void Awake()
    {
        //hand = GetComponent<P_SkillTest>().hand;
    }

    public void SkillAction()
    {
        //float startAngle = hand.localEulerAngles.z - fireAngle;

        //Debug.Log(hand.localEulerAngles);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - fireAngle;

        for (int i = 0; i < 3; i++)
        {
            GameObject go = Instantiate(bombArrow,/*hand.GetChild(0)*/transform.position,Quaternion.identity);
            go.transform.localEulerAngles = new Vector3(0,0,angle + i * fireAngle);
        }
    }
}
