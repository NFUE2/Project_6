using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_BowQ : MonoBehaviour, P_ISkill
{
    public GameObject wireArrow;

    public void SkillAction()
    {
        GameObject go = Instantiate(wireArrow);
        go.GetComponent<P_WireArrow>().player = transform;
    }
}
