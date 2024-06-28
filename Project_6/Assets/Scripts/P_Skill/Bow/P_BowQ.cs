using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_BowQ : MonoBehaviour, P_ISkill
{
    public GameObject wireArrow;
    public Transform hand;

    private void Awake()
    {
        hand = GetComponent<P_SkillTest>().hand;
    }

    public void SkillAction()
    {
        GameObject go = Instantiate(wireArrow,hand.GetChild(0).position,Quaternion.identity);
        go.transform.localEulerAngles = new Vector3(0, 0, hand.localEulerAngles.z);
        go.GetComponent<P_WireArrow>().player = transform;
    }
}
