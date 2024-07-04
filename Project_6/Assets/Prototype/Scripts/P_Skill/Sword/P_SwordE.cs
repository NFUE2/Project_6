using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class P_SwordE :MonoBehaviour, P_ISkill
{
    //투사체
    public GameObject projectile;
    public Animator animator;

    public float actionTime;
    private float lastAction;

    private void Awake()
    {
        //animator = GetComponent<Animator>();
        lastAction = -actionTime;
    }

    public void SkillAction()
    {
        //투사체 복사 및 날리기
        //animator.SetTrigger("SkillE");
        if (Time.time - lastAction < actionTime) return;

        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;

        //Transform go = Instantiate(projectile, transform.position, Quaternion.identity).transform;
        GameObject go = PhotonNetwork.Instantiate("Prototype/" + projectile.name, transform.position, Quaternion.identity);

        go.transform.localEulerAngles = new Vector3(0,0,angle);

        StartCoroutine(CoolTime());
    }
    IEnumerator CoolTime()
    {
        Text coolTimeText = GetComponent<PlayerController_Melee>().cooltimeEText;
        lastAction = Time.time;

        while (Time.time - lastAction < actionTime)
        {
            coolTimeText.text = (actionTime - (Time.time - lastAction)).ToString("F1");
            yield return null;
        }
        coolTimeText.text = "준비완료";
    }
}
