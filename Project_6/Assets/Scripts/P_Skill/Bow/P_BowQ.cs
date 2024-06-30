using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_BowQ : MonoBehaviour, P_ISkill
{
    public GameObject wireArrow;
    public Transform hand;

    private void Awake()
    {
        //hand = GetComponent<P_SkillTest>().hand;
    }

    public void SkillAction()
    {
        GameObject go = Instantiate(wireArrow,/*hand.GetChild(0).*/transform.position,Quaternion.identity);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        go.transform.localEulerAngles = new Vector3(0, 0, angle);
        go.GetComponent<P_WireArrow>().player = transform;
    }
}
