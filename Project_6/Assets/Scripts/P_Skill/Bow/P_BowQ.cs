using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P_BowQ : MonoBehaviour, P_ISkill
{
    public GameObject wireArrow;
    public Transform hand;

    public float actionTime;
    private float lastAction;
    private void Awake()
    {
        //hand = GetComponent<P_SkillTest>().hand;
        lastAction = -actionTime;
    }

    public void SkillAction()
    {
        if (Time.time - lastAction < actionTime) return;

        GameObject go = Instantiate(wireArrow,/*hand.GetChild(0).*/transform.position,Quaternion.identity);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        go.transform.localEulerAngles = new Vector3(0, 0, angle);
        go.GetComponent<P_WireArrow>().player = transform;

        StartCoroutine(CoolTime());
    }

    IEnumerator CoolTime()
    {
        lastAction = Time.time;
        Text coolTimeText = GetComponent<PlayerController_Bow>().cooltimeQText;

        while (Time.time - lastAction < actionTime)
        {
            coolTimeText.text = (actionTime - (Time.time - lastAction)).ToString("F1");
            yield return null;
        }

        coolTimeText.text = "준비완료";
    }
}
