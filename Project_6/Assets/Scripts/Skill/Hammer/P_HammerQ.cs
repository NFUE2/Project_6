using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_HammerQ : MonoBehaviour, P_ISkill
{
    public GameObject shield;
    public void SkillAction()
    {
        Transform go = Instantiate(shield).transform;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition).normalized;

        go.LookAt(mousePos);
    }
}
