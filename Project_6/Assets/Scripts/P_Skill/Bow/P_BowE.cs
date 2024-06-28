using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_BowE : MonoBehaviour, P_ISkill
{
    public GameObject bombArrow;
    public int fireAngle;
    public void SkillAction()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject go = Instantiate(bombArrow);

        }
    }
}
