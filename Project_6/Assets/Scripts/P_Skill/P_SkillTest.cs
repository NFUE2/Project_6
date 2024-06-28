using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Skill
{
    SkillQ,
    SkillE
}

//public enum ClassType
//{
//    Sword,
//    Hammer,
//    Bow,
//    Gun
//}

public class P_SkillTest : MonoBehaviour
{
    private P_ISkill[] skills;
    //public ClassType type;
    public Transform hand; //원거리 무기

    private void Awake()
    {
        skills = GetComponents<P_ISkill>();
    }

    private void Update()
    {
        if(hand != null)
        {
            Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            hand.localEulerAngles = new Vector3(0,0,angle);
        }

        if (Input.GetKeyDown(KeyCode.Q))
            skills[(int)Skill.SkillQ].SkillAction();

        if (Input.GetKeyDown(KeyCode.E))
            skills[(int)Skill.SkillE].SkillAction();
    }
}
