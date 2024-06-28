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
        hand = GetComponent<P_SkillTest>().hand;
    }

    public void SkillAction()
    {
        float startAngle = hand.localEulerAngles.z - fireAngle;

        Debug.Log(hand.localEulerAngles);

        for (int i = 0; i < 3; i++)
        {
            GameObject go = Instantiate(bombArrow,hand.GetChild(0).position,Quaternion.identity);
            go.transform.localEulerAngles = new Vector3(0,0,startAngle + i * fireAngle);
        }
    }
}
