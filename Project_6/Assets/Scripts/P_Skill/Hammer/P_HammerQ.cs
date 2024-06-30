using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_HammerQ : MonoBehaviour, P_ISkill
{
    public GameObject shield;
    private GameObject createShield;
    public float shieldTime;
    public Vector3 shieldPos;

    private void Awake()
    {
        shieldPos = transform.position;
    }

    public void SkillAction()
    {
        if (createShield != null) return;

        createShield = Instantiate(shield,shieldPos,Quaternion.identity);
        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        createShield.transform.localEulerAngles = new Vector3(0,0,angle);
        
        Invoke("ShieldDestroy", shieldTime);
    }

    private void Update()
    {
        transform.position = shieldPos;
    }

    private void ShieldDestroy()
    {
        if (createShield != null) Destroy(createShield);
    }
}

