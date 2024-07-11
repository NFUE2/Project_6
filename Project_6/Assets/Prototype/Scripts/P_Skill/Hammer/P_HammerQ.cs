using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P_HammerQ : MonoBehaviour, P_ISkill
{
    [Header("Skill Q")]
    public GameObject shield;
    private GameObject createShield;
    //public float shieldTime;

    public float actionTime;
    private float lastAction;

    private void Awake()
    {
        lastAction = - actionTime;
    }
    public void SkillAction()
    {
        if (createShield != null) return;
        if (Time.time - lastAction < actionTime) return;
        //createShield = Instantiate(shield, transform.position, Quaternion.identity);
        createShield = PhotonNetwork.Instantiate("Prototype/" + shield.name, transform.position, Quaternion.identity);
        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        createShield.transform.localEulerAngles = new Vector3(0,0,angle);
        
        //Invoke("ShieldDestroy", shieldTime);
        StartCoroutine(CoolTime());
    }

    private void Update()
    {
        if (createShield != null && createShield.activeInHierarchy) createShield.transform.position = transform.position;
    }

    //private void ShieldDestroy()
    //{
    //    if (createShield != null) Destroy(createShield);
    //}
    IEnumerator CoolTime()
    {
        lastAction = Time.time;
        Text coolTimeText = GetComponent<PlayerController_Hammer>().cooltimeQText;

        while (Time.time - lastAction < actionTime)
        {
            coolTimeText.text = (actionTime - (Time.time - lastAction)).ToString("F1");
            yield return null;
        }

        coolTimeText.text = "준비완료";
    }
}

